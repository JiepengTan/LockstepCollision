using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public partial class Capsule:BaseShape
    {   
        public override EColType ColType{get { return EColType.Capsule;}}
        /// <summary>
        /// Medial line segment start point
        /// </summary>
        public Point a;

        /// <summary>
        /// Medial line segment end point
        /// </summary>
        public Point b;

        /// <summary>
        /// Radius
        /// </summary>
        public LFloat r;

        public LFloat GetBoudsSphereRadius()
        {
            return (b - a).magnitude * LFloat.half + r;
        }

        public LVector GetBoundSphereCenter()
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
            return Collision.TestSphereCapsule(sphere, this);
        }
        public override bool TestWith(AABB aabb)
        {
            return Collision.TestAABBCapsule(aabb, this);
        }
        public override bool TestWith(Capsule capsule)
        {
            return Collision.TestCapsuleCapsule(this, capsule);
        }

        public override bool TestWith(OBB obb)
        {
            return Collision.TestOBBCapsule(obb, this);
        }
        
        public override bool TestWith(Plane plane)
        {
            return Collision.TestCapsulePlane(this, plane);
        }

        //public override bool TestWith(Ray ray)
        //{
        //    return Collision.IntersectRayCapsule(ray, a, b, r, out LFloat t);
        //}
    };
}