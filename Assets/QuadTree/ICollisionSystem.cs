namespace TQuadTree1 {
    public interface ICollisionSystem {
        void DoStart();
        void DoUpdate();
        void AddCollider(ColliderProxy collider);
        void DrawGizmos();
    }
}