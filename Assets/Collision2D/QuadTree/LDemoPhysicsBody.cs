using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Collision2D {
    public class BoxShape : ICollisionShape {
        ///// Constructor /////

        public BoxShape(Bounds bounds, bool twoD = true){
            Bounds = bounds;
            Center = bounds.center.ToLVector3();
            Extents = bounds.extents.ToLVector3();
            TwoD = twoD;
        }

        ///// Fields /////

        public Bounds Bounds;
        public bool TwoD;

        ///// Properties /////

        public LVector3 Center { get; set; }

        public LVector3 Extents { get; set; }

        public LVector3 Min {
            get { return Center - Extents; }
        }

        public LVector3 Max {
            get { return Center + Extents; }
        }

        ///// Methods /////

        public CollisionResult TestCollision(ICollisionShape other){
            var result = new CollisionResult();

            if (other is BoxShape) {
                result.Collides = BoxVsBox(this, (BoxShape) other, ref result, TwoD);
            }
            else {
                Debug.LogErrorFormat("Collision test not implemented: {0}-{1}", GetType(), other.GetType());
            }

            return result;
        }

        public static bool BoxVsBox(BoxShape a, BoxShape b, ref CollisionResult result, bool twoD){
            return CollisionTest.TestAABB(a.Min, a.Max, b.Min, b.Max, ref result, twoD);
        }
    }

    public class LDemoPhysicsBody : MonoBehaviour, ICollisionBody,IQuadTreeBody {
        private ICollisionShape _shape;
        private Color _gizmoColor = Color.green;

        private void Awake(){
            var collider = GetComponent<BoxCollider>();
            _shape = new BoxShape(collider.bounds, false);
            _shape.Center = transform.position.ToLVector3();
        }

        private void Update(){
            _shape.Center = transform.position.ToLVector3();
        }

        private void OnDrawGizmos(){
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 1.25f);
        }

        #region ICollisionBody

        public int RefId { get; set; }

        public bool Sleeping {
            get { return false; }
        }

        public ICollisionShape CollisionShape {
            get { return _shape; }
        }

        public void OnCollision(CollisionResult result, ICollisionBody other){
            _gizmoColor = result.Type == CollisionType.Exit ? Color.green : Color.red;
        }

        #endregion

        #region IQuadTreeBody

        public LVector2 Position {
            get { return new LVector2(transform.position.x.ToLFloat(), transform.position.z.ToLFloat()); }
        }

        public bool QuadTreeIgnore {
            get { return false; }
        }

        #endregion
    }
}