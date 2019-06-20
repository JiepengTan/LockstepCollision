using Lockstep.Math;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
{
    [System.Serializable]
    /// <summary>
    /// TODO implement
    /// </summary>
    public partial class Cylinder:BaseShape
    {   
        public override EColType ColType{get { return EColType.Capsule;}}
        /// <summary>
        /// Medial line segment start point
        /// </summary>
        public LVector3 a;

        /// <summary>
        /// Medial line segment end point
        /// </summary>
        public LVector3 b;

        /// <summary>
        /// Radius
        /// </summary>
        public LFloat r;
        
        public override Sphere GetBoundSphere()
        {
            return new Sphere((a+b)*LFloat.half,(b-a).magnitude*LFloat.half + r);
        }
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
    };
}