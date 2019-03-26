using LockStepMath;

namespace LockStepCollision
{
    public struct Segment
    {
        public LVector b;
        public LVector e;

        public Segment(LVector b, LVector e)
        {
            this.b = b;
            this.e = e;
        }
    }
}