using System.Numerics;
using System.Runtime.CompilerServices;
using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public partial class Rect : BaseShape
    {
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType
        {
            get { return EColType.Rect; }
        }

        /// <summary>
        /// // center point of rectangle
        /// </summary>
        public Point c;

        /// <summary>
        ///  unit vectors determining local x and y axes for the rectangle
        /// </summary>
        public Axis2D u;

        /// <summary>
        /// the halfwidth extents of the rectangle along the axes
        /// </summary>
        public LVector2D e;

     
    };
}