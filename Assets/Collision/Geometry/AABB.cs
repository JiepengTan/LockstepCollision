using System.Numerics;
using System.Runtime.CompilerServices;
using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public partial class AABB:BaseShape
    {
        public override EColType ColType{get { return EColType.AABB;}}
        public Point min;
        public Point max;

        /// <summary>
        /// center point of AABB
        /// </summary>
        public Point c
        {
            get { return (max + min) * LFloat.half; }
        }

        /// <summary>
        /// radius or halfwidth extents
        /// </summary>
        public LVector r
        {
            get { return (max - min) * LFloat.half; }
        }

        // Transform AABB a by the matrix m and translation t,
        // find maximum extents, and store result into AABB b.
        public  void UpdateAABB(Matrix33 m, LVector t)
        {
            Point _c = c + t;
            LVector _r = r;
            min = max = _c;
            // For all three axes
            for (int i = 0; i < 3; i++)
            {
                // Form extent by summing smaller and larger terms respectively
                for (int j = 0; j < 3; j++)
                {
                    LFloat e = m[i,j] * _r[j];
                    if (e < LFloat.zero)
                    {
                        min[i] += e;
                        max[i] -= e;
                    }
                    else
                    {
                        min[i] -= e;
                        max[i] += e;
                    }
                }
            }
        }
    }
}