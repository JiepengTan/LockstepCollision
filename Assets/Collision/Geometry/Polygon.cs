namespace LockStepCollision
{
    public partial class Polygon:BaseShape
    {
               
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType{get { return EColType.Polygon;}}

    }
}