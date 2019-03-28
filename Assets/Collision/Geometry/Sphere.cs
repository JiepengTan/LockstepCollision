using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    [System.Serializable]
    public partial class Sphere : BaseShape
    {
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType{get { return EColType.Sphere;}}

        /// <summary>
        /// // Sphere center
        /// </summary>
        public Point c;

        /// <summary>
        /// Sphere radius
        /// </summary>
        public LFloat r;
        
        public Sphere(Point c, LFloat r)
        {
            this.c = c;
            this.r = r;
        }

        public Sphere()
        {
        }
        public override Sphere GetBoundSphere()
        {
            return this;
        }
        
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
        
        public override bool TestWith(Sphere sphere)
        {
            return Collision.TestSphereSphere(sphere, this);
        }
        public override bool TestWith(AABB aabb)
        {
            return Collision.TestSphereAABB(this,aabb);
        }
        public override bool TestWith(Capsule capsule)
        {
            return Collision.TestSphereCapsule(this, capsule);
        }

        public override bool TestWith(OBB obb)
        {
            return Collision.TestSphereOBB( this,obb,out LVector p);
        }
        
        public override bool TestWith(Plane plane)
        {
            return Collision.TestSpherePlane(this, plane);
        }
        
        public override bool TestWith(Ray ray)
        {
            return Collision.IntersectRaySphere(ray.o, ray.d,this, out LFloat t,out LVector p);
        }
    };
}