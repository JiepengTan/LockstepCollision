using Lockstep.Math;


namespace Lockstep.Collision2D {
    [System.Serializable]

    public class AABB : BaseShaper2D {
        public LVector2 size;

        public AABB(LVector2 pos, LVector2 size){
            this.pos = pos;
            this.size = size;
            radius = size.magnitude;
        }
        public override bool TestWithShape(BaseShaper2D a){
            return a.TestWith(this);
        }

        public override bool TestWith(AABB shape){return Utils.TestCollision(this,shape);}

        public override bool TestWith(OBB shape){return Utils.TestCollision(this,shape);}

        public override bool TestWith(Circle shape){return Utils.TestCollision(shape,this);}

        public override void UpdatePosition(LVector2 pos){ this.pos = pos; }
        public override void UpdateRotation(LFloat deg){throw new System.NotSupportedException("AABB can not rotate");}
    }
}