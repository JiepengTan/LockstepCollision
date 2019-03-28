using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    /// <summary>
    /// TODO implement
    /// </summary>
    public partial class Cylinder:BaseShape
    {   
        public override EColType ColType{get { return EColType.Capsule;}}
        /// <summary>
        /// Medial line segment start point
        /// </summary>
        public Point a;

        /// <summary>
        /// Medial line segment end point
        /// </summary>
        public Point b;

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