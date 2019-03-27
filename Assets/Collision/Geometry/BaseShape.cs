using LockStepMath;

namespace LockStepCollision
{
    public enum EColType
    {
        Sphere,
        AABB,
        Capsule,
        OBB,
        Plane,
        Rect,
        Segment,
        Polygon,
        EnumCount,
    }

    public partial class BaseShape
    {
        /// <summary>
        /// 碰撞类型
        /// </summary>
        public virtual EColType ColType
        {
            get { return EColType.EnumCount; }
        }

        public virtual Sphere GetBoundSphere()
        {
            return null;
        }

        public virtual void UpdateCollider(bool isDiffPos,bool isDiffRot,LVector targetPos, LVector targetRot )
        {
        }
    }
}