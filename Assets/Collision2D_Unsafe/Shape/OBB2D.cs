using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.UnsafeCollision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct OBB2D :IShape2D{
        public Circle BoundSphere;

        public int TypeId {
            get => BoundSphere.TypeId;
        }

        public int Id {
            get => BoundSphere.Id;
            set => BoundSphere.Id = value;
        }

        public LVector2 pos {
            get => BoundSphere.pos;
            set => BoundSphere.pos = value;
        }

        public LFloat radius {
            get => BoundSphere.radius;
            set => BoundSphere.radius = value;
        }

        public LFloat deg;
        public LVector2 size;
        public LVector2 up;

        public LVector2 right => new LVector2(up.y, -up.x);

        //CCW 旋转角度
        public void Rotate(LFloat rdeg){
            deg += rdeg;
            if (deg > 360 || deg < -360) {
                deg = deg - (deg / 360 * 360);
            }

            SetDeg(deg);
        }

        public void SetDeg(LFloat rdeg){
            deg = rdeg;
            var rad = LMath.Deg2Rad * deg;
            var c = LMath.Cos(rad);
            var s = LMath.Sin(rad);
            up = new LVector2(-s, c);
        }

        public OBB2D(int id, LVector2 pos, LVector2 size, LVector2 up){
            BoundSphere = new Circle((int) EShape2D.AABB, id, pos, size.magnitude);
            this.size = size;
            this.up = up;
            this.deg = LMath.Atan2(-up.x, up.y);
        }

        public OBB2D(int id, LVector2 pos, LVector2 size, LFloat deg){
            BoundSphere = new Circle((int) EShape2D.AABB, id, pos, size.magnitude);
            this.size = size;
            this.deg = deg;
            var rad = LMath.Deg2Rad * deg;
            var c = LMath.Cos(rad);
            var s = LMath.Sin(rad);
            up = new LVector2(-s, c);
        }       
        public void UpdatePosition(LVector2 pos){this.pos = pos;}

        public void UpdateRotation(LFloat deg){SetDeg(deg);}
    }
}