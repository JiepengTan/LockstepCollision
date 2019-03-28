using LockStepMath;
using Point = LockStepMath.LVector;

namespace LockStepCollision
{
    public partial class Ray:BaseShape
    {  
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType{get { return EColType.Ray;}}

        /// <summary>
        /// orgin point
        /// </summary>
        public Point b;
        public LVector r;

        public Ray(Point b, LVector r)
        {
            this.b = b;
            this.r = r;
        }
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
    }
}