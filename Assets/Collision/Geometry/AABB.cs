using LockStepMath;

using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public struct AABB {
        /// <summary>
        /// // center point of AABB
        /// </summary>
        public Point c; 
        /// <summary>
        /// radius or halfwidth extents (rx, ry, rz)
        /// </summary>
        public LVector r;

        public void Update(LFloat[,] m, LFloat[] t)
        {
            for (int i = 0; i < 3; i++) {
                c[i] = t[i];
                r[i] = LFloat.zero;
                for (int j = 0; j < 3; j++) {
                    c[i] += m[i,j] * c[j];
                    r[i] += Abs(m[i,j]) * r[j];
                }
            }
        }

    };
}