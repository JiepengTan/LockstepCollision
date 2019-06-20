using Lockstep.Math;

namespace Lockstep.Collision
{
    [System.Serializable]
    public partial class Segment:BaseShape
    {  
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType{get { return EColType.Segment;}}

        public LVector3 b;
        public LVector3 e;

        public Segment(LVector3 b, LVector3 e)
        {
            this.b = b;
            this.e = e;
        }
        
    }
}