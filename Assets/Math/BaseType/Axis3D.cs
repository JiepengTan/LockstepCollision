using LockStepMath;
using UnityEngine;
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
        public static readonly Axis3D identity = new Axis3D(LVector.right, LVector.up, LVector.forward);

        public Axis3D(LVector x, LVector y, LVector z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public LVector WorldToLocal(LVector vec)
        {
            var _x = Dot(x, vec);
            var _y = Dot(y, vec);
            var _z = Dot(z, vec);
            return new LVector(_x, _y, _z);
        }
        public LVector LocalToWorld(LVector vec)
        {
            return x * vec.x + y * vec.y + z * vec.z;
        }

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
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default: throw new System.IndexOutOfRangeException("vector idx invalid" + index);
                }
            }
        }
    }
}