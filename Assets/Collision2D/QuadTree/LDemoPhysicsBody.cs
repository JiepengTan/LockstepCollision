using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Collision2D {
    public unsafe class LDemoPhysicsBody : MonoBehaviour, ICollisionBody {
        private Color _gizmoColor = Color.green;

        public Sphere2D* ColPtr { get; set; }
        public Bounds Bounds;
        public bool TwoD;

        ///// Properties /////
        /// p
        public LFloat Y { get; set; }

        public LVector2 Center { get; set; }

        public LVector2 Extents { get; set; }

        public LVector2 Min {
            get { return Center - Extents; }
        }

        public LVector2 Max {
            get { return Center + Extents; }
        }

        public int RefId { get; set; }

        public bool Sleeping {
            get { return false; }
        }


        public LVector2 Position => new LVector2(transform.position.x.ToLFloat(), transform.position.z.ToLFloat());

        public bool QuadTreeIgnore => false;
        
        

        private void Awake(){
            var collider = GetComponent<BoxCollider>();
            Extents = collider.bounds.extents.ToLVector2XZ();
            Center = transform.position.ToLVector2XZ();
            Y = transform.position.y.ToLFloat();
        }

        public bool isDebug = false;
        private void Update(){
            var curPos = transform.position.ToLVector2XZ();
            if (Center != curPos) {
                CollisionSystem.MarkDirty(this);
            }

            Center = curPos;
            Y = transform.position.y.ToLFloat();
            if (ColPtr != null) {
                ColPtr->Pos = Center;
            }
        }
     
        private void OnDrawGizmos(){
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 1.25f);
        }

        public void OnCollision(CollisionResult result, ICollisionBody other){
            _gizmoColor = result.Type == CollisionType.Exit ? Color.green : Color.red;
            if (isDebug&& result.Type == CollisionType.Enter) {
                Debug.LogError("Enter Collision");
            }
        }
    }
}