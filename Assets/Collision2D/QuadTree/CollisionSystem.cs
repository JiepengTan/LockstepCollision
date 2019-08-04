using System.Collections.Generic;
using Lockstep.Math;
using UnityEngine;
using UnityEngine.Profiling;

namespace Lockstep.Collision2D {
    public abstract unsafe class CollisionSystem {
        ///// Fields /////

        protected Dictionary<int, ICollisionBody> _id2Body = new Dictionary<int, ICollisionBody>(MaxCollisionBodies);
        protected Dictionary<int, long> _id2Ptr = new Dictionary<int, long>(MaxCollisionBodies);

        private HashSet<int> _pairs = new HashSet<int>();
        private List<int> _pairCache = new List<int>();
        protected List<long> _shapePtrs = new List<long>();
        public static HashSet<ICollisionBody> dirtyBodys = new HashSet<ICollisionBody>();
        private int _uniqueIndex;

        public const int MaxCollisionBodies = 10000;

        ///// Methods /////
        protected int shapeCount;

        public abstract void DetectBodyVsBody();
        //public abstract bool LineOfSight(LVector3 start, LVector3 end);

        public virtual int AddBody(ICollisionBody body, AABB2D* ptr, LVector2 pos, LVector2 extents){
            var refId = _uniqueIndex++;
            *ptr = new AABB2D(refId, pos, extents);
            _id2Ptr[refId] = (long) ptr;
            _id2Body[refId] = body;
            _shapePtrs.Add((long) ptr);
            return refId;
        }

        /// Removes a body from the CollisionSystem
        public virtual bool RemoveBody(ICollisionBody body){
            _id2Body.Remove(body.RefId);
            _id2Ptr.Remove(body.RefId);
            return true; //bodyList.Remove(body);
        }

        public delegate void OnShapePtr(Circle* shape);

        public void IteratePtrs(OnShapePtr funcShapePtr){
            foreach (var ptrval in _shapePtrs) {
                var ptr = (Circle*) ptrval;
                funcShapePtr(ptr);
            }
        }

        public static void MarkDirty(ICollisionBody body){
            dirtyBodys.Add(body);
        }

        public static void CleanDirtyBodies(){
            dirtyBodys.Clear();
        }

        public int countDetectBodyVsBody = 0;

        /// <summary>
        /// Process CollisionSystem by one step
        /// </summary>
        public virtual void Step(){
            //raw 3.6 ~4.3ms
            //LMath 9.5~11ms
            Profiler.BeginSample("DetectBodyVsBody");
            DetectBodyVsBody();
            Profiler.EndSample();

            Profiler.BeginSample("FindColliderPair");
            foreach (var i in _pairs) {
                var body1 = FindShapePtr(i / (MaxCollisionBodies + 1));
                var body2 = FindShapePtr(i % (MaxCollisionBodies + 1));
                if (body1 == null || body2 == null) {
                    continue;
                }

                Test(body1, body2, false);
            }


            _pairs.Clear();

            for (int i = 0; i < _pairCache.Count; i++) {
                _pairs.Add(_pairCache[i]);
            }

            _pairCache.Clear();
            Profiler.EndSample();
        }

        public ICollisionBody FindCollisionBody(int refId){
            if (_id2Body.TryGetValue(refId, out var val))
                return val;
            return null;
        }

        public Circle* FindShapePtr(int refId){
            if (_id2Ptr.TryGetValue(refId, out var val))
                return (Circle*) val;
            return null;
        }


        public void NotifyCollisionEvent(Circle* shape1, Circle* shape2, ref CollisionResult result){
            var body1 = FindCollisionBody(shape1->Id);
            var body2 = FindCollisionBody(shape2->Id);
            body2.OnCollision(result, body1);
            result.Normal *= -LFloat.one;
            result.First = true;
            body1.OnCollision(result, body2);
        }

        /// <summary>
        ///  Executes collision between two bodies
        /// </summary>
        protected bool Test(Circle* body1, Circle* body2, bool removePair = true){
            var result = new CollisionResult();
            var paired = FindCollisionPair(body1, body2, removePair);

            if (TestCollisionShapes(body1, body2, ref result)) {
                result.Type = paired ? ECollisionType.Stay : ECollisionType.Enter;
                CacheCollisionPair(body1, body2);
                NotifyCollisionEvent(body1, body2, ref result);
                return true;
            }
            else {
                if (paired) {
                    result.Type = ECollisionType.Exit;
                    NotifyCollisionEvent(body1, body2, ref result);
                }
            }
            return false;
        }

        private bool FindCollisionPair(Circle* a, Circle* b, bool remove = true){
            var idx = a->Id * (MaxCollisionBodies + 1) + b->Id;
            if (remove) return _pairs.Remove(idx);
            else return _pairs.Contains(idx);
        }

        private void CacheCollisionPair(Circle* a, Circle* b){
            var idx = a->Id * (MaxCollisionBodies + 1) + b->Id;
            _pairCache.Add(idx);
        }

        private static bool TestCollisionShapes(Circle* a, Circle* b, ref CollisionResult result){
            //var hasCollide = a->TestCollision(b);
            var aabbA = (AABB2D*) a;
            var aabbB = (AABB2D*) b;
            result.Collides = CollisionTest.TestAABBAABB(
                aabbA->pos, aabbA->radius, aabbA->size,
                aabbB->pos, aabbB->radius, aabbB->size
            );
            return result.Collides;
        }


        public void DrawGizmos(){
            Gizmos.color = Color.black;
            IteratePtrs((ptr) => {
                var body = FindCollisionBody(ptr->Id);
                if (body == null) return;
                var center = body.Pos;
                if (center == LVector2.zero) return;
                GizmosHelper.DrawGizmos((EShape2D) ptr->TypeId, ptr, Color.green);
            });
        }
    }
}