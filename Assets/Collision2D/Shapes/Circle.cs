using Lockstep.Math;


namespace Lockstep.Collision2D {
    [System.Serializable]
    public class Circle : BaseShaper2D {
        public Circle(LVector2 pos, LFloat radius){
            this.pos = pos;
            this.radius = radius;
        }
        
        public override bool TestWithShape(BaseShaper2D a){return a.TestWith(this);}
        public override bool TestWith(AABB shape){return Utils.TestCollision(this,shape);}
        public override bool TestWith(OBB shape){return Utils.TestCollision(this,shape);}
        public override bool TestWith(Circle shape){return Utils.TestCollision(this,shape);}


        public override void UpdatePosition(LVector2 pos){this.pos = pos;}
        public override void UpdateRotation(LFloat deg){ /*do nothing*/}
    }
}