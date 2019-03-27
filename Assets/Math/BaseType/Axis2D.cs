using LockStepMath;
using UnityEngine;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
   
    public struct Axis2D
    {
        public LVector x;
        public LVector y;
        
        public static readonly Axis2D identity = new Axis2D(LVector.right, LVector.up);
        public Axis2D(LVector x, LVector y)
        {
            this.x = x;
            this.y = y;
        }
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
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default: throw new System.IndexOutOfRangeException("vector idx invalid" + index);
                }
            }
        }
    }


  
}