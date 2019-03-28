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
        public Point o;
        /// <summary>
        /// dir
        /// </summary>
        public LVector d;

        public Ray()
        {
        }

        public Ray(Point o, LVector d)
        {
            this.o = o;
            this.d = d;
        }
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
    }
}