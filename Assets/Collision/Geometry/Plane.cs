using LockStepMath;

using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public struct Plane
    {
        /// <summary>
        /// Plane normal. Points x on the plane satisfy Dot(n,x) = d
        /// </summary>
        public LVector n;

        /// <summary>
        /// d = dot(n,p) for a given point p on the plane
        /// </summary>
        public LFloat d;

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
    };
}