using LockStepMath;
using Point = LockStepMath.LVector;

namespace LockStepCollision
{
    [System.Serializable]
    /// <summary>
    /// 碰撞网格  建议仅在静态情况下使用
    /// </summary>
    public partial class ColMesh : BaseShape
    {
        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType
        {
            get { return EColType.ColMesh; }
        }

        public int verticesCount;
        public LVector[] vertices;
        public LVector[] triangles;
        public LVector[] normal;
        public AABB bounds;
        protected Sphere boundSphere;

        public ColMesh(UnityEngine.Mesh mesh)
        {
        }

        public override Sphere GetBoundSphere()
        {
            return boundSphere;
        }

        public override void UpdateCollider(bool isDiffPos, bool isDiffRot, LVector targetPos, LVector targetRot)
        {
        }
        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
    }
}