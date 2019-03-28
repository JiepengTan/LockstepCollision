using LockStepMath;

using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    [System.Serializable]
    public partial class Plane :BaseShape
    {        
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType{get { return EColType.Plane;}}
        
        /// <summary>
        /// Plane normal. Points x on the plane satisfy Dot(n,x) = d
        /// </summary>
        public LVector n;

        /// <summary>
        /// d = dot(n,p) for a given point p on the plane
        /// </summary>
        public LFloat d;

        public Plane()
        {
        }

        public Plane(LVector n, LFloat d)
        {
            this.n = n;
            this.d = d;
        }

        public Plane(Point a, Point b, Point c)
        {
            n = Cross(b - a, c - a).normalized;
            d = Dot(n, a);
        }
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
        
        public override bool TestWith(Sphere sphere)
        {
            return Collision.TestSpherePlane(sphere, this);
        }
        public override bool TestWith(AABB aabb)
        {
            return Collision.TestAABBPlane(aabb,this);
        }
        public override bool TestWith(Capsule capsule)
        {
            return Collision.TestCapsulePlane(capsule,this);
        }

        public override bool TestWith(OBB obb)
        {
            return Collision.TestOBBPlane( obb, this);
        }
        
        public override bool TestWith(Plane plane)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithRay");
        }

        public override bool TestWith(Ray ray)
        {
            return Collision.IntersectRayPlane(ray.o, ray.d, this, out LFloat t, out LVector p);
        }
    };
}