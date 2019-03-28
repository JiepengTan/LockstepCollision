using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public partial class Sphere : BaseShape
    {
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType{get { return EColType.Sphere;}}

        /// <summary>
        /// // Sphere center
        /// </summary>
        public Point c;

        /// <summary>
        /// Sphere radius
        /// </summary>
        public LFloat r;
        
        public Sphere(Point c, LFloat r)
        {
            this.c = c;
            this.r = r;
        }

        public Sphere()
        {
        }
        public override Sphere GetBoundSphere()
        {
            return this;
        }
        
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
    };
}