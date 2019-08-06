using Lockstep.Math;

namespace Lockstep.Collision2D {
    public interface ICollisionSystem {
        void DoStart(int[][] interestingMasks, int[] allTypes);
        void DoUpdate();
        ColliderProxy GetCollider(int id);
        void AddCollider(ColliderProxy collider);
        void RemoveCollider(ColliderProxy collider);
        bool Raycast(int layerType, Ray2D checkRay, out LFloat t, out int id, LFloat maxDistance);
        bool Raycast(int layerType, Ray2D checkRay, out LFloat t, out int id);
        
        //for debug
        void DrawGizmos();
        int ShowTreeId { get; set; }
    }
}