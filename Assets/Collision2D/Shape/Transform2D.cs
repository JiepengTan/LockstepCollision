using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct Transform2D  {
        public LVector2 pos;
        public LFloat y;
        public LFloat deg;
    }
}