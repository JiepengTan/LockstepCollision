using Lockstep.Math;
using UnityEngine;
using static Lockstep.Math.LMath;
using Point = Lockstep.Math.LVector;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
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