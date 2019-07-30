using System;
using System.Runtime.InteropServices;

namespace Lockstep {
    public unsafe class NativeHelper {

        public static void Free(IntPtr ptr){
            Marshal.FreeHGlobal(ptr);
        }

        public static IntPtr Alloc(int bytes){
            return Marshal.AllocHGlobal(bytes);
        }
        
        public static unsafe void Zero(byte* ptr, int size){
            for (; size >= 4; size -= 4) {
                *(int*) ptr = 0;
                ptr += 4;
            }
            for (; size > 0; --size) {
                *ptr = 0;
            }
        }

        public static unsafe void Copy(void* dest, void* src, int size){
            Copy((byte*) dest, (byte*) src, size);
        }

        public static unsafe void Copy(byte* dest, byte* src, int size)
        {
            for (; size >= 4; size -= 4)
            {
                *(int*) dest = *(int*) src;
                dest += 4;
                src += 4;
            }
            for (; size > 0; --size)
            {
                *dest = *src;
                ++dest;
                ++src;
            }
        }
        public static byte* AllocAndZero(int bytes){
            var ptr = (byte*) (Marshal.AllocHGlobal(bytes).ToPointer());
            Zero(ptr, bytes);
            return ptr;
        }
        public static void NullPointer()
        {
            throw new NullReferenceException("Method invoked on null pointer.");
        }
        public static void ArrayOutOfRange()
        {
            throw new ArgumentOutOfRangeException("Array index out of range");
        }
    }
}