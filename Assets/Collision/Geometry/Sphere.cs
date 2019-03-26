using LockStepMath;

using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public struct Sphere
    {
        /// <summary>
        /// // Sphere center
        /// </summary>
        public Point c; 
        /// <summary>
        /// Sphere radius
        /// </summary>
        public LFloat r; 
    };
}