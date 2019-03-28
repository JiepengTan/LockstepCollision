using LockStepMath;
using TMPro;
using UnityEngine;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public partial class OBB : BaseShape

    {
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType
        {
            get { return EColType.OBB; }
        }

        /// <summary>
        /// OBB center point
        /// </summary>
        public Point c;

        /// <summary>
        /// Local x-, y-, and z-axes
        /// </summary>
        public Axis3D u;

        /// <summary>
        /// Positive halfwidth extents of OBB along each axis
        /// </summary>
        public LVector e;
        
        public AABB ToAABB()
        {
            var aabb = new AABB();
            var abse = e.abs;
            aabb.min = c - abse;
            aabb.max = c + abse;
            return aabb;
        }
        
        
        public override Sphere GetBoundSphere()
        {
            return new Sphere(c,e.magnitude);
        }
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
        
        public override bool TestWith(Sphere sphere)
        {
            return Collision.TestSphereOBB(sphere, this,out LVector p);
        }
        public override bool TestWith(AABB aabb)
        {
            //TODO 改为更加高效的判定方式
            return Collision.TestOBBOBB(this, aabb.ToOBB());
        }
        public override bool TestWith(Capsule capsule)
        {
            return Collision.TestOBBCapsule(this, capsule);
        }

        public override bool TestWith(OBB obb)
        {
            return Collision.TestOBBOBB(obb, this);
        }
        
        public override bool TestWith(Plane plane)
        {
            return Collision.TestOBBPlane(this, plane);
        }

        public override bool TestWith(Ray ray)
        {
            return Collision.IntersectRayOBB(ray.o, ray.d, this, out LFloat tmin, out LVector temp);
        }

    };
}