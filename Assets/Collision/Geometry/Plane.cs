using Lockstep.Math;

using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
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
        public LVector3 n;

        /// <summary>
        /// d = dot(n,p) for a given point p on the plane
        /// </summary>
        public LFloat d;

        public Plane()
        {
        }

        public Plane(LVector3 n, LFloat d)
        {
            this.n = n;
            this.d = d;
        }

        public Plane(LVector3 a, LVector3 b, LVector3 c)
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
            return Utils.TestSpherePlane(sphere, this);
        }
        public override bool TestWith(AABB aabb)
        {
            return Utils.TestAABBPlane(aabb,this);
        }
        public override bool TestWith(Capsule capsule)
        {
            return Utils.TestCapsulePlane(capsule,this);
        }

        public override bool TestWith(OBB obb)
        {
            return Utils.TestOBBPlane( obb, this);
        }
        
        public override bool TestWith(Plane plane)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithRay");
        }

        public override bool TestWith(Ray ray)
        {
            return Utils.IntersectRayPlane(ray.o, ray.d, this, out LFloat t, out LVector3 p);
        }
    };
}