using System.Collections.Generic;
using System.Xml.Linq;
using Lockstep.Math;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lockstep.Collision2D {
    public unsafe class PhysicsBody : MonoBehaviour, ICollisionBody {
        public ColliderConfig ColliderConfig;
        private Color _gizmoColor = Color.green;
        public Circle* ColPtr { get; set; }
        public int RefId { get; set; }
        public LFloat Y { get; set; }
        public LVector2 Pos { get; set; }
        public LVector2 PreCenter { get; set; }
        
        public bool IsUpdateFromUnity = true;
        public bool Sleeping => false;

        public LVector2 Position => new LVector2(transform.position.x.ToLFloat(), transform.position.z.ToLFloat());

        public bool QuadTreeIgnore => false;

        static float moveSpd = 5;
        public bool isDebug = false;
        
        private void Awake(){
            Pos = transform.position.ToLVector2XZ();
            Y = transform.position.y.ToLFloat();
        }


        
        private void Update(){
            if (IsUpdateFromUnity) {
                var pos = transform.position;
                Pos = pos.ToLVector2XZ();
                Y = pos.y.ToLFloat();
            }

            if (Pos != PreCenter) {
                CollisionSystem.MarkDirty(this);
            }
            PreCenter = Pos;
            if (ColPtr != null) {
                ColPtr->pos = Pos;
            }
        }


        public virtual void OnCollision(CollisionResult result, ICollisionBody other){
            _gizmoColor = result.Type == ECollisionType.Exit ? Color.green : Color.red;
            if (isDebug && result.Type == ECollisionType.Enter) {
                Debug.LogError("Enter Collision");
            }
        }
        private void OnDrawGizmos(){
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 1.25f);
        }
    }
}