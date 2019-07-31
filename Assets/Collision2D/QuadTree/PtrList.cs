using System.Collections.Generic;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    public unsafe class PtrList {
        private const int AlignSize = 64;
        protected int count;
        protected int capacity;
        protected byte* _dirtyBits;
        protected void** _ptrs;
        Dictionary<long, int> _ptr2Pos = new Dictionary<long, int>();

        public void* GetShapePtr(int idx){
            if (idx < 0 || idx >= count) {
                NativeHelper.ArrayOutOfRange();
            }

            return _ptrs[idx];
        }

        public PtrList(int initSize = AlignSize){
            initSize = LMath.Max(AlignSize, initSize);
            if (initSize % AlignSize != 0) {
                initSize = (initSize % AlignSize + 1) * AlignSize;
            }

            capacity = initSize;
            count = 0;
            _ptrs = (void**) NativeHelper.AllocAndZero(sizeof(void*) * initSize);
            _dirtyBits = (byte*) NativeHelper.AllocAndZero(initSize / 8);
        }

        void CheckAndResize(){
            if (count == capacity) {
                _ptrs = (void**) NativeHelper.Resize(_ptrs, sizeof(void*) * capacity,
                    sizeof(void*) * capacity * 2);
                _dirtyBits = (byte*) NativeHelper.Resize(_dirtyBits, capacity * 8 / AlignSize,
                    capacity * 8 / AlignSize * 2);
                capacity *= 2;
            }
        }

        public void AddPtr(void* ptr){
            CheckAndResize();
            var ptrVal = (long) ptr;
            if (!_ptr2Pos.ContainsKey(ptrVal)) {
                _ptrs[count] = ptr;
                //_ptr2Pos[ptrVal] = count;
                count++;
            }
        }

        public void RemovePtr(void* ptr){
            var ptrVal = (long) ptr;
            if (_ptr2Pos.TryGetValue(ptrVal, out var id)) {
                _ptrs[id] = null;
                _ptr2Pos.Remove(ptrVal);
            }
        }

        public Iterator GetEnumerator(){
            return new Iterator(_ptrs,count);
        }

        public unsafe partial struct Iterator {
            private int _count;
            private void** _ptrs;
            private int _index;
            public void* Current;

            public Iterator(void** ptrs, int count){
                _ptrs = ptrs;
                _count = count;
                Current = null;
                _index = -1;
            }

            public bool Next(){
                while (++_index < _count) {
                    if (_ptrs[_index] != null) {
                        Current = _ptrs[_index];
                        return true;
                    }
                }

                Current = null;
                return false;
            }
        }
    }
}