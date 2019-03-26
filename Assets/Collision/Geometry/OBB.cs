using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public struct Axis3D
    {
        public LVector x;
        public LVector y;
        public LVector z;

        public LVector this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new System.IndexOutOfRangeException("vector idx invalid" + index);
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1:y = value; break;
                    case 2:z = value; break;
                    default: throw new System.IndexOutOfRangeException("vector idx invalid" + index);
                }
            }
        }
    }
    public struct Axis2D
    {
        public LVector x;
        public LVector y;

        public LVector this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default: throw new System.IndexOutOfRangeException("vector idx invalid" + index);
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1:y = value; break;
                    default: throw new System.IndexOutOfRangeException("vector idx invalid" + index);
                }
            }
        }
    }


    public struct OBB
    {
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
    };
}