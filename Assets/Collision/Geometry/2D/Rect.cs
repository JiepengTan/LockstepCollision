using System.Numerics;
using System.Runtime.CompilerServices;
using Lockstep.Math;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
{
    [System.Serializable]
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
        public LVector3 c;

        /// <summary>
        ///  unit vectors determining local x and y axes for the rectangle
        /// </summary>
        public LAxis2D u;

        /// <summary>
        /// the halfwidth extents of the rectangle along the axes
        /// </summary>
        public LVector2 e;

     
    };
}