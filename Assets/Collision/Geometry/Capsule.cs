using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public struct Capsule
    {
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
    };
}