using System.Collections;
using System.Collections.Generic;
using Lockstep.Math;
using UnityEngine;
using UnityEngine.Profiling;

namespace Lockstep.Collision2D {
    public enum CollisionType {
        /// <summary>
        /// First frame of collision
        /// </summary>
        Enter,

        /// <summary>
        /// Collision occuring over multiple frames
        /// </summary>
        Stay,

        /// <summary>
        /// First frame collision is no longer occuring
        /// </summary>
        Exit
    }

    public class CollisionTest {
        private static LFloat[] _distances = new LFloat[6];

        public static bool TestAABB(LVector3 min0, LVector3 max0, LVector3 min1, LVector3 max1,
            ref CollisionResult result,
            bool twod = true){
            _distances[0] = max1[0] - min0[0];
            _distances[1] = max0[0] - min1[0];
            _distances[2] = max1[2] - min0[2];
            _distances[3] = max0[2] - min1[2];

            var iter = 4;

            // test y if 3d
            if (!twod) {
                _distances[4] = max1[1] - min0[1];
                _distances[5] = max0[1] - min1[1];
                iter = 6;
            }

            for (int i = 0; i < iter; i++) {
                if (_distances[i] < LFloat.zero)
                    return false;

                if ((i == 0) || _distances[i] < result.Penetration) {
                    result.Penetration = _distances[i];
                    //result.Normal = _aabbNormals[i];
                }
            }

            return true;
        }

        public static bool SegmentIntersects(ICollisionShape collider, LVector3 start, LVector3 end){
            LVector3 d = (end - start) * LFloat.half;
            LVector3 e = (collider.Max - collider.Min) * LFloat.half;
            LVector3 c = start + d - (collider.Min + collider.Max) * LFloat.half;

            LVector3 ad = new LVector3(LMath.Abs(d.x), LMath.Abs(d.y), LMath.Abs(d.z));

            if (LMath.Abs(c.x) > e.x + ad.x)
                return false;
            if (LMath.Abs(c.y) > e.y + ad.y)
                return false;
            if (LMath.Abs(c.z) > e.z + ad.z)
                return false;

            if (LMath.Abs(d.y * c.z - d.z * c.y) > e.y * ad.z + e.z * ad.y)
                return false;
            if (LMath.Abs(d.z * c.x - d.x * c.z) > e.z * ad.x + e.x * ad.z)
                return false;
            if (LMath.Abs(d.x * c.y - d.y * c.x) > e.x * ad.y + e.y * ad.x)
                return false;

            return true;
        }
    }

    public abstract class CollisionSystem {
        ///// Fields /////

        protected List<ICollisionBody> bodyList = new List<ICollisionBody>(MaxCollisionBodies);
        private HashSet<int> _pairs = new HashSet<int>();
        private List<int> _pairCache = new List<int>();
        private int _uniqueIndex;

        public const int MaxCollisionBodies = 10000;

        ///// Methods /////

        public abstract void DetectBodyVsBody();
        public abstract bool LineOfSight(LVector3 start, LVector3 end);

        /// <summary>
        /// Adds a body to the CollisionSystem
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public virtual bool AddBody(ICollisionBody body){
            if (!bodyList.Contains(body) && bodyList.Count < MaxCollisionBodies) {
                body.RefId = _uniqueIndex;
                _uniqueIndex++;
                bodyList.Add(body);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a body from the CollisionSystem
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public virtual bool RemoveBody(ICollisionBody body){
            return bodyList.Remove(body);
        }

        /// <summary>
        /// Process CollisionSystem by one step
        /// </summary>
        public virtual void Step(){
            
            //raw 3.6 ~4.3ms
            //LMath 9.5~11ms
            Profiler.BeginSample("DetectBodyVsBody");
            DetectBodyVsBody();
            Profiler.EndSample();

            // This was implemented for CollisionSystem implementations with broad phases
            // When two colliders are paired and one of them is moved to a far away position 
            // on the same frame, they wont be tested next frame due to broad phasing, but they will still be paired.
            // This simply checks all pairs that weren't checked this frame

            foreach (var i in _pairs) {
                var body1 = FindCollisionBody(i / (MaxCollisionBodies + 1));
                var body2 = FindCollisionBody(i % (MaxCollisionBodies + 1));
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
        }

        public ICollisionBody FindCollisionBody(int refId){
            for (int i = 0; i < bodyList.Count; i++) {
                if (bodyList[i].RefId == refId)
                    return bodyList[i];
            }

            return null;
        }

        public void DrawGizmos(){
            Gizmos.color = Color.black;
            for (var i = 0; i < bodyList.Count; i++) {
                var center = bodyList[i].CollisionShape.Center;
                if (center == LVector3.zero) continue;
                center.y += 2;
                Gizmos.DrawWireCube(center.ToVector3(), (bodyList[i].CollisionShape.Extents * 2.ToLFloat()).ToVector3());
            }
        }

        /// <summary>
        ///  Executes collision between two bodies
        /// </summary>
        /// <param name="body1"></param>
        /// <param name="body2"></param>
        /// <param name="removePair"></param>
        /// <returns></returns>
        protected bool Test(ICollisionBody body1, ICollisionBody body2, bool removePair = true){
            var result = new CollisionResult();
            var paired = FindCollisionPair(body1, body2, removePair);

            if (TestCollisionShapes(body1.CollisionShape, body2.CollisionShape, ref result)) {
                result.Type = paired ? CollisionType.Stay : CollisionType.Enter;
                CacheCollisionPair(body1, body2);
                body2.OnCollision(result, body1);
                result.Normal *= -LFloat.one;
                result.First = true;
                body1.OnCollision(result, body2);
                return true;
            }
            else {
                if (paired) {
                    result.Type = CollisionType.Exit;
                    body2.OnCollision(result, body1);
                    result.Normal *= -LFloat.one;
                    result.First = true;
                    body1.OnCollision(result, body2);
                    return true;
                }
            }

            return false;
        }

        private bool FindCollisionPair(ICollisionBody a, ICollisionBody b, bool remove = true){
            var idx = a.RefId * (MaxCollisionBodies + 1) + b.RefId;
            if (remove) return _pairs.Remove(idx);
            else return _pairs.Contains(idx);
        }

        private void CacheCollisionPair(ICollisionBody a, ICollisionBody b){
            var idx = a.RefId * (MaxCollisionBodies + 1) + b.RefId;
            _pairCache.Add(idx);
        }

        private static bool TestCollisionShapes(ICollisionShape a, ICollisionShape b, ref CollisionResult result){
            result = a.TestCollision(b);
            return result.Collides;
        }
    }

    /// <summary>
    /// Queries a QuadTree to test for collisions with only nearby bodies
    /// </summary>
    public class CollisionSystemQuadTree : CollisionSystem {
        ///// Constructor /////

        public CollisionSystemQuadTree(QuadTree tree){
            _quadTree = tree;
        }

        ///// Fields /////

        private QuadTree _quadTree;

        ///// Methods /////

        public override void DetectBodyVsBody(){
            for (int i = 0; i < bodyList.Count; i++) {
                if (bodyList[i].Sleeping)
                    continue;

                // todo: something better maybe?
                var maxDist = bodyList[i].CollisionShape.Extents.x;
                maxDist = LMath.Max(maxDist, bodyList[i].CollisionShape.Extents.y);
                maxDist = LMath.Max(maxDist, bodyList[i].CollisionShape.Extents.z);

                var ents = _quadTree.GetBodies(bodyList[i].CollisionShape.Center, maxDist);
                for (int j = 0; j < ents.Count; j++) {
                    var body2 = ents[j] as ICollisionBody;
                    if (body2 == null
                        || body2.Sleeping
                        || ReferenceEquals(bodyList[i], body2)) {
                        continue;
                    }

                    Test(bodyList[i], body2);
                }
            }
        }

        public override bool LineOfSight(LVector3 start, LVector3 end){
            for (var i = 0; i < bodyList.Count; i++) {
                if (CollisionTest.SegmentIntersects(bodyList[i].CollisionShape, start, end))
                    return false;
            }

            return true;
        }
    }

    public struct CollisionResult {
        public bool Collides;
        public LVector3 Normal;
        public LFloat Penetration;
        public CollisionType Type;
        public bool First;
    }

    public interface ICollisionShape {
        LVector3 Center { get; set; }
        LVector3 Extents { get; }
        LVector3 Min { get; }
        LVector3 Max { get; }
        CollisionResult TestCollision(ICollisionShape other);
    }

    public interface ICollisionBody {
        int RefId { get; set; }

        /// <summary>
        /// Skip this body when testing for collisions
        /// </summary>
        bool Sleeping { get; }

        /// <summary>
        /// The body's collision shape
        /// </summary>
        ICollisionShape CollisionShape { get; }

        /// <summary>
        /// Called each frame of collision
        /// </summary>
        /// <param name="result"></param>
        /// <param name="other"></param>
        void OnCollision(CollisionResult result, ICollisionBody other);
    }

    public interface IQuadTreeBody {
        LVector2 Position { get; }
        bool QuadTreeIgnore { get; }
    }

    public partial class QuadTree {
        public void DrawGizmos(){
            //draw children
            if (_childA != null) _childA.DrawGizmos();
            if (_childB != null) _childB.DrawGizmos();
            if (_childC != null) _childC.DrawGizmos();
            if (_childD != null) _childD.DrawGizmos();

            //draw rect
            Gizmos.color = Color.cyan;
            var p1 = new LVector3(_bounds.position.x, 0.1f.ToLFloat(), _bounds.position.y);
            var p2 = new LVector3(p1.x + _bounds.width, 0.1f.ToLFloat(), p1.z);
            var p3 = new LVector3(p1.x + _bounds.width, 0.1f.ToLFloat(), p1.z + _bounds.height);
            var p4 = new LVector3(p1.x, 0.1f.ToLFloat(), p1.z + _bounds.height);
            DrawLine(p1, p2);
            DrawLine(p2, p3);
            DrawLine(p3, p4);
            DrawLine(p4, p1);
        }

        public void DrawLine(LVector3 a, LVector3 b){
            Gizmos.DrawLine(a.ToVector3(), b.ToVector3());
        }
    }

    public partial class QuadTree {
        private static class QuadTreePool {
            ///// Fields /////

            private static Queue<QuadTree> _pool;
            private static int _maxPoolCount = 1024;
            private static int _defaultMaxBodiesPerNode = 6;
            private static int _defaultMaxLevel = 6;

            ///// Methods /////

            public static QuadTree GetQuadTree(LRect bounds, QuadTree parent){
                if (_pool == null) Init();
                QuadTree tree = null;
                if (_pool.Count > 0) {
                    tree = _pool.Dequeue();
                    tree._bounds = bounds;
                    tree._parent = parent;
                    tree._maxLevel = parent._maxLevel;
                    tree._maxBodiesPerNode = parent._maxBodiesPerNode;
                    tree._curLevel = parent._curLevel + 1;
                }
                else tree = new QuadTree(bounds, parent);

                return tree;
            }

            public static void PoolQuadTree(QuadTree tree){
                if (tree == null) return;
                tree.Clear();
                if (_pool.Count > _maxPoolCount) return;
                _pool.Enqueue(tree);
            }

            private static void Init(){
                _pool = new Queue<QuadTree>();
                for (int i = 0; i < _maxPoolCount; i++) {
                    _pool.Enqueue(new QuadTree(LRect.zero, _defaultMaxBodiesPerNode, _defaultMaxLevel));
                }
            }
        }

        ///// Constructors /////

        public QuadTree(LRect bounds, int maxBodiesPerNode = 6, int maxLevel = 6){
            _bounds = bounds;
            _maxBodiesPerNode = maxBodiesPerNode;
            _maxLevel = maxLevel;
            _bodies = new List<IQuadTreeBody>(maxBodiesPerNode);
        }

        private QuadTree(LRect bounds, QuadTree parent)
            : this(bounds, parent._maxBodiesPerNode, parent._maxLevel){
            _parent = parent;
            _curLevel = parent._curLevel + 1;
        }

        ///// Fields /////

        private QuadTree _parent;
        private LRect _bounds;
        private List<IQuadTreeBody> _bodies;
        private int _maxBodiesPerNode;
        private int _maxLevel;
        private int _curLevel;
        private QuadTree _childA;
        private QuadTree _childB;
        private QuadTree _childC;
        private QuadTree _childD;
        private List<IQuadTreeBody> _entCache;

        ///// Methods /////

        public void AddBody(IQuadTreeBody body){
            if (_childA != null) {
                var child = GetQuadrant(body.Position);
                child.AddBody(body);
            }
            else {
                _bodies.Add(body);
                if (_bodies.Count > _maxBodiesPerNode && _curLevel < _maxLevel) {
                    Split();
                }
            }
        }

        public List<IQuadTreeBody> GetBodies(LVector3 point, LFloat radius){
            var p = new LVector2(point.x, point.z);
            return GetBodies(p, radius);
        }

        public List<IQuadTreeBody> GetBodies(LVector2 point, LFloat radius){
            if (_entCache == null) _entCache = new List<IQuadTreeBody>(64);
            else _entCache.Clear();
            GetBodies(point, radius, _entCache);
            return _entCache;
        }

        public List<IQuadTreeBody> GetBodies(LRect rect){
            if (_entCache == null) _entCache = new List<IQuadTreeBody>(64);
            else _entCache.Clear();
            GetBodies(rect, _entCache);
            return _entCache;
        }

        private void GetBodies(LVector2 point, LFloat radius, List<IQuadTreeBody> bods){
            //no children
            if (_childA == null) {
                for (int i = 0; i < _bodies.Count; i++)
                    bods.Add(_bodies[i]);
            }
            else {
                if (_childA.ContainsCircle(point, radius))
                    _childA.GetBodies(point, radius, bods);
                if (_childB.ContainsCircle(point, radius))
                    _childB.GetBodies(point, radius, bods);
                if (_childC.ContainsCircle(point, radius))
                    _childC.GetBodies(point, radius, bods);
                if (_childD.ContainsCircle(point, radius))
                    _childD.GetBodies(point, radius, bods);
            }
        }

        private void GetBodies(LRect rect, List<IQuadTreeBody> bods){
            //no children
            if (_childA == null) {
                for (int i = 0; i < _bodies.Count; i++)
                    bods.Add(_bodies[i]);
            }
            else {
                if (_childA.ContainsRect(rect))
                    _childA.GetBodies(rect, bods);
                if (_childB.ContainsRect(rect))
                    _childB.GetBodies(rect, bods);
                if (_childC.ContainsRect(rect))
                    _childC.GetBodies(rect, bods);
                if (_childD.ContainsRect(rect))
                    _childD.GetBodies(rect, bods);
            }
        }

        public bool ContainsCircle(LVector2 circleCenter, LFloat radius){
            var center = _bounds.center;
            var dx = LMath.Abs(circleCenter.x - center.x);
            var dy = LMath.Abs(circleCenter.y - center.y);
            if (dx > (_bounds.width / 2 + radius)) {
                return false;
            }

            if (dy > (_bounds.height / 2 + radius)) {
                return false;
            }

            if (dx <= (_bounds.width / 2)) {
                return true;
            }

            if (dy <= (_bounds.height / 2)) {
                return true;
            }

            var dsx = (dx - _bounds.width / 2);
            var dsy = (dy - _bounds.height / 2);
            var cornerDist = dsx * dsx + dsy * dsy;
            return cornerDist <= (radius * radius);
        }

        public bool ContainsRect(LRect rect){
            return _bounds.Overlaps(rect);
        }

        private QuadTree GetLowestChild(LVector2 point){
            var ret = this;
            while (ret != null) {
                var newChild = ret.GetQuadrant(point);
                if (newChild != null) ret = newChild;
                else break;
            }

            return ret;
        }

        public void Clear(){
            QuadTreePool.PoolQuadTree(_childA);
            QuadTreePool.PoolQuadTree(_childB);
            QuadTreePool.PoolQuadTree(_childC);
            QuadTreePool.PoolQuadTree(_childD);
            _childA = null;
            _childB = null;
            _childC = null;
            _childD = null;
            _bodies.Clear();
        }


        private void Split(){
            var hx = _bounds.width / 2;
            var hz = _bounds.height / 2;
            var sz = new LVector2(hx, hz);

            //split a
            var aLoc = _bounds.position;
            var aRect = new LRect(aLoc, sz);
            //split b
            var bLoc = new LVector2(_bounds.position.x + hx, _bounds.position.y);
            var bRect = new LRect(bLoc, sz);
            //split c
            var cLoc = new LVector2(_bounds.position.x + hx, _bounds.position.y + hz);
            var cRect = new LRect(cLoc, sz);
            //split d
            var dLoc = new LVector2(_bounds.position.x, _bounds.position.y + hz);
            var dRect = new LRect(dLoc, sz);

            //assign QuadTrees
            _childA = QuadTreePool.GetQuadTree(aRect, this);
            _childB = QuadTreePool.GetQuadTree(bRect, this);
            _childC = QuadTreePool.GetQuadTree(cRect, this);
            _childD = QuadTreePool.GetQuadTree(dRect, this);

            for (int i = _bodies.Count - 1; i >= 0; i--) {
                var child = GetQuadrant(_bodies[i].Position);
                child.AddBody(_bodies[i]);
                _bodies.RemoveAt(i);
            }
        }

        private QuadTree GetQuadrant(LVector2 point){
            if (_childA == null) return null;
            if (point.x > _bounds.x + _bounds.width / 2) {
                if (point.y > _bounds.y + _bounds.height / 2) return _childC;
                else return _childB;
            }
            else {
                if (point.y > _bounds.y + _bounds.height / 2) return _childD;
                return _childA;
            }
        }
    }
}