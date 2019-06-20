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

        public LVector b;
        public LVector e;

        public Segment(LVector b, LVector e)
        {
            this.b = b;
            this.e = e;
        }
        
    }
}