using Lockstep.Math;

namespace Lockstep.Collision
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
        public LVector3[] vertices;
        public LVector3[] triangles;
        public LVector3[] normal;
        public AABB bounds;
        protected Sphere boundSphere;

        public ColMesh(UnityEngine.Mesh mesh)
        {
        }

        public override Sphere GetBoundSphere()
        {
            return boundSphere;
        }

        public override bool TestWithShape(BaseShape shape)
        {
            return shape.TestWith(this);
        }
    }
}