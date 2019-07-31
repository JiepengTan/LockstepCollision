using System;
using System.Collections.Generic;
using System.Linq;

namespace Lockstep {
    public unsafe class NativeFactory {
        static Dictionary<int, NativePool> pools = new Dictionary<int, NativePool>();

        public static long MemSize => pools.Sum((a)=>a.Value.MemSize);

        public static NativePool GetPool(int size){
            if (pools.TryGetValue(size, out var pool)) {
                return pool;
            }

            var tPool = new NativePool(size);
            pools[size] = tPool;
            return tPool;
        }
    }

    public unsafe class NativePool {
        private Stack<long> _allPtrs = new Stack<long>();
        private int _typeSize = -1;
        public int MemSize => _typeSize * _allPtrs.Count;

        public NativePool(int typeSize){
            this._typeSize = typeSize;
        }

        public void Return(void* ptr){
            if (ptr == null) NativeHelper.NullPointer();
            _allPtrs.Push((long) ptr);
        }

        public void* Get(){
            if (_allPtrs.Count == 0) return null;
            var ptr = (byte*) _allPtrs.Pop();
            NativeHelper.Zero(ptr, _typeSize);
            return ptr;
        }

        public void* ForceGet(){
            var ptr = Get();
            if (ptr == null) {
                ptr = NativeHelper.AllocAndZero(_typeSize);
            }

            return ptr;
        }

        public void Clear(){
            while (_allPtrs.Count > 0) {
                var ptr = _allPtrs.Pop();
                NativeHelper.Free((IntPtr) ptr);
            }
        }
    }
}