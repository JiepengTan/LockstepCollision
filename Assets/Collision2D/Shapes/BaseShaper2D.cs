using Lockstep.Math;

namespace Lockstep.Collision2D {
    
    public enum EShape2D {
        Circle,
        AABB,
        OBB,
        EnumCount
    }
    
    [System.Serializable]
    public abstract class BaseShaper2D  {
        public LVector2 pos;
        public LFloat sqrRadius => radius * radius;

        public LFloat radius;

        public virtual bool TestWithShape(BaseShaper2D a){return false;}
        public virtual bool TestWith(AABB shape){ return false;}
        public virtual bool TestWith(OBB2D shape){ return false;}
        public virtual bool TestWith(Circle shape){ return false;}
        
        public virtual void UpdatePosition(LVector2 pos){ }
        public virtual void UpdateRotation(LFloat deg){ }
    }
}