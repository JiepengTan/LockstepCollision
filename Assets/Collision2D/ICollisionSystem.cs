namespace Lockstep.Collision2D {
    public interface ICollisionSystem {
        void DoStart(int[][] interestingMasks,int [] allTypes);
        void DoUpdate();
        void AddCollider(ColliderProxy collider);
        void DrawGizmos();

        int ShowTreeId { get; set; } 
    }
}