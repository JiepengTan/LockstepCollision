using Lockstep.Math;

namespace Lockstep.Collision2D {
    [System.Serializable]
    public class OBB : BaseShaper2D {
        public LVector2 up;

        public LVector2 right {
            get { return new LVector2(up.y, -up.x); }
        }
        
        public LFloat deg;
        public LVector2 size;

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

        public OBB(LVector2 pos, LVector2 size, LVector2 up){
            this.pos = pos;
            this.size = size;
            radius = size.magnitude;
            this.up = up;
            this.deg = LMath.Atan2(-up.x, up.y);
        }

        public OBB(LVector2 pos, LVector2 size, LFloat deg){
            this.pos = pos;
            this.size = size;
            radius = size.magnitude;
            this.deg = deg;
            var rad = LMath.Deg2Rad * deg;
            var c = LMath.Cos(rad);
            var s = LMath.Sin(rad);
            up = new LVector2(-s, c);
        }

        public override bool TestWithShape(BaseShaper2D a){return a.TestWith(this);}
        public override bool TestWith(AABB shape){return Utils.TestCollision(shape,this);}
        public override bool TestWith(OBB shape){return Utils.TestCollision(shape,this);}
        public override bool TestWith(Circle shape){return Utils.TestCollision(shape,this);}
        public override void UpdatePosition(LVector2 pos){this.pos = pos;}
        public override void UpdateRotation(LFloat deg){ SetDeg(deg);}
    }
}