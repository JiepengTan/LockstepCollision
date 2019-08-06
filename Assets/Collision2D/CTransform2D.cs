using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    public class CTransform2D {
        public LVector2 pos;
        public LFloat y;
        public LFloat deg;

        public CTransform2D(LVector2 pos, LFloat y) : this(pos, y, LFloat.zero){ }
        public CTransform2D(LVector2 pos) : this(pos, LFloat.zero, LFloat.zero){ }

        public CTransform2D(LVector2 pos, LFloat y, LFloat deg){
            this.pos = pos;
            this.y = y;
            this.deg = deg;
        }


        public void Reset(){
            pos = LVector2.zero;
            y = LFloat.zero;
            deg = LFloat.zero;
        }

        public static Transform2D operator +(CTransform2D a, CTransform2D b){
            return new Transform2D {pos = a.pos + b.pos, y = a.y + b.y, deg = a.deg + b.deg};
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct Transform2D  {
        public LVector2 pos;
        public LFloat y;
        public LFloat deg;
    }
}