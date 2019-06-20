using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Collision {
    public class UnityColliderProxy : ColliderProxy {
        protected Vector3 _lastPosition;
        protected Quaternion _lastRotation;
        
        protected virtual void Start(){
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
        }

        protected virtual void Update(){
//check pos or rotation changed
            var diffPos = _lastPosition != transform.position;

            var diffRot = _lastRotation != transform.rotation;
            if (diffPos) {
                _lastPosition = transform.position;
                UpdatePosition(transform.position.ToLVector3());
            }

            if (diffRot) {
                _lastRotation = transform.rotation;
                UpdateRotation(transform.forward.ToLVector3(), transform.up.ToLVector3());
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos(){
            if (allColliders[0] is Capsule) {
                int i = 0;
            }
            foreach (var col in allColliders) {
                col.OnDrawGizmos(true, Color.green);
            }

            //boundSphere?.OnDrawGizmos(true, Color.red);
        }
#endif

    }
}