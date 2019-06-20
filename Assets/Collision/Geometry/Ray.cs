using Lockstep.Math;

namespace Lockstep.Collision
{
    [System.Serializable]
    public partial class Ray : BaseShape
    {
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType
        {
            get { return EColType.Ray; }
        }

        /// <summary>
        /// orgin point
        /// </summary>
        public LVector3 o;

        /// <summary>
        /// dir
        /// </summary>
        public LVector3 d;

        public Ray()
        {
        }

        public Ray(LVector3 o, LVector3 d)
        {
            this.o = o;
            this.d = d;
        }

        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }

        public override bool TestWith(Sphere sphere)
        {
            return Utils.IntersectRaySphere(o, d, sphere, out LFloat t, out LVector3 p);
        }

        public override bool TestWith(AABB aabb)
        {
            return Utils.IntersectRayAABB(o, d, aabb, out LFloat t, out LVector3 p);
        }

        public override bool TestWith(Capsule capsule)
        {
            return Utils.IntersectRayCapsule(o, d, capsule.a, capsule.b, capsule.r, out LFloat t);
        }

        public override bool TestWith(OBB obb)
        {
            return Utils.IntersectRayOBB(o, d, obb, out LFloat t, out LVector3 p);
        }

        public override bool TestWith(Plane plane)
        {
            return Utils.IntersectRayPlane(o, d, plane, out LFloat t, out LVector3 p);
        }

        public override bool TestWith(Ray ray)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithRay");
        }
    }
}