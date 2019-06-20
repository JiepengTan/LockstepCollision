using Lockstep.Math;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
{
    [System.Serializable]
    public partial class Capsule:BaseShape
    {   
        public override EColType ColType{get { return EColType.Capsule;}}
        /// <summary>
        /// Medial line segment start point
        /// </summary>
        public LVector3 a;

        /// <summary>
        /// Medial line segment end point
        /// </summary>
        public LVector3 b;

        /// <summary>
        /// Radius
        /// </summary>
        public LFloat r;

        public LFloat GetBoudsSphereRadius()
        {
            return (b - a).magnitude * LFloat.half + r;
        }

        public LVector3 GetBoundSphereCenter()
        {
            return (a + b) * LFloat.half;
        }

        public override Sphere GetBoundSphere()
        {
            return new Sphere((a+b)*LFloat.half,(b-a).magnitude*LFloat.half + r);
        }
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
        

        public override bool TestWith(Sphere sphere)
        {
            return Utils.TestSphereCapsule(sphere, this);
        }
        public override bool TestWith(AABB aabb)
        {
            return Utils.TestAABBCapsule(aabb, this);
        }
        public override bool TestWith(Capsule capsule)
        {
            return Utils.TestCapsuleCapsule(this, capsule);
        }

        public override bool TestWith(OBB obb)
        {
            return Utils.TestOBBCapsule(obb, this);
        }
        
        public override bool TestWith(Plane plane)
        {
            return Utils.TestCapsulePlane(this, plane);
        }
        
        public override bool TestWith(Ray ray)
        {
            return Utils.IntersectRayCapsule(ray.o,ray.d, a, b, r, out LFloat t);
        }
    };
}