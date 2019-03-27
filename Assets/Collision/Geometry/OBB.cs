using LockStepMath;
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
        
        public override Sphere GetBoundSphere()
        {
            return new Sphere(c,e.magnitude);
        }
    };
}