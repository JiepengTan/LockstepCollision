using Lockstep.Math;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
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
        public LVector3 c;

        /// <summary>
        /// Sphere radius
        /// </summary>
        public LFloat r;
        
        public Sphere(LVector3 c, LFloat r)
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
            return Utils.TestSphereSphere(sphere, this);
        }
        public override bool TestWith(AABB aabb)
        {
            return Utils.TestSphereAABB(this,aabb);
        }
        public override bool TestWith(Capsule capsule)
        {
            return Utils.TestSphereCapsule(this, capsule);
        }

        public override bool TestWith(OBB obb)
        {
            return Utils.TestSphereOBB( this,obb,out LVector3 p);
        }
        
        public override bool TestWith(Plane plane)
        {
            return Utils.TestSpherePlane(this, plane);
        }
        
        public override bool TestWith(Ray ray)
        {
            return Utils.IntersectRaySphere(ray.o, ray.d,this, out LFloat t,out LVector3 p);
        }
    };
}