using System.Collections.Generic;
using Lockstep.Collision2D;
using Lockstep.Math;
using UnityEngine;
using UnityEngine.Profiling;
using Random = System.Random;

namespace TQuadTree1 {
    public class CBaseShape {
        public virtual int TypeId => (int) EShape2D.EnumCount;
        public int id;
        public LFloat high;
    }

    public class CCircle : CBaseShape {
        public override int TypeId => (int) EShape2D.Circle;
        public LFloat radius;

        public CCircle() : this(LFloat.zero){ }

        public CCircle(LFloat radius){
            this.radius = radius;
        }
    }

    public class CAABB : CCircle {
        public override int TypeId => (int) EShape2D.AABB;
        public LVector2 size;

        public CAABB() : base(){ }

        public CAABB(LVector2 size){
            this.size = size;
            radius = size.magnitude;
        }
    }

    public class COBB : CAABB {
        public override int TypeId => (int) EShape2D.OBB;
        public LFloat deg;
        public LVector2 up;

        public COBB(LVector2 size, LFloat deg) : base(size){
            this.deg = deg;
            SetDeg(deg);
        }

        public COBB(LVector2 size, LVector2 up) : base(size){
            SetUp(up);
        }

        //CCW 旋转角度
        public void Rotate(LFloat rdeg){
            deg += rdeg;
            if (deg > 360 || deg < -360) {
                deg = deg - (deg / 360 * 360);
            }

            SetDeg(deg);
        }

        public void SetUp(LVector2 up){
            this.up = up;
            this.deg = LMath.Atan2(-up.x, up.y);
        }

        public void SetDeg(LFloat rdeg){
            deg = rdeg;
            var rad = LMath.Deg2Rad * deg;
            var c = LMath.Cos(rad);
            var s = LMath.Sin(rad);
            up = new LVector2(-s, c);
        }
    }

    public class CPolygon : CCircle {
        public override int TypeId => (int) EShape2D.Polygon;
        public int vertexCount;
        public LFloat deg;
        public LVector2[] vertexes;
    }

    public class CSegment : CBaseShape {
        public override int TypeId => (int) EShape2D.Segment;
        public LVector2 pos1;
        public LVector2 pos2;
    }

    public class CRay : CBaseShape {
        public override int TypeId => (int) EShape2D.Ray;
        public LVector2 pos;
        public LVector2 dir;
    }

    public class CTransform2D {
        public LVector2 pos;
        public LFloat y;
        public LFloat deg;

        public CTransform2D(LVector2 pos, LFloat y) : this(pos, y, LFloat.zero){ }
        public CTransform2D(LVector2 pos) : this(pos, LFloat.zero, LFloat.zero){ }

        public CTransform2D(LVector2 pos, LFloat y, LFloat deg){
            this.pos = pos;
            this.y = y;
            this.deg = deg;
        }


        public void Reset(){
            pos = LVector2.zero;
            y = LFloat.zero;
            deg = LFloat.zero;
        }

        public static Transform2D operator +(CTransform2D a, CTransform2D b){
            return new Transform2D {pos = a.pos + b.pos, y = a.y + b.y, deg = a.deg + b.deg};
        }
    }

    public class ColliderPart {
        public CBaseShape collider;
        public CTransform2D transform;
    }

    public class ColliderPrefab {
        public List<ColliderPart> parts = new List<ColliderPart>();
        public CBaseShape collider => parts[0].collider;
        public CTransform2D transform => parts[0].transform;

        public Rect GetBounds(){
            //TODO
            var col = collider;
            var tran = transform;
            var type = (EShape2D) col.TypeId;
            switch (type) {
                case EShape2D.Circle: {
                    var radius = ((CCircle) col).radius;
                    return LRect.CreateRect(tran.pos, new LVector2(radius, radius)).ToRect();
                }
                case EShape2D.AABB: {
                    var halfSize = ((CAABB) col).size;
                    return LRect.CreateRect(tran.pos, halfSize).ToRect();
                }
                case EShape2D.OBB: {
                    var radius = ((COBB) col).radius;
                    return LRect.CreateRect(tran.pos, new LVector2(radius, radius)).ToRect();
                }
            }

            Debug.LogError("No support type" + type);

            return new Rect();
        }
    }

    public class CollisionSystem : ICollisionSystem {
        public uint[] _collisionMask = new uint[32];

        public List<BoundsQuadTree> boundsTrees = new List<BoundsQuadTree>();
        public float worldSize = 150;
        public float minNodeSize = 1;
        public float loosenessval = 1.25f;
        public Vector3 pos;

        private Dictionary<uint, ColliderProxy> id2Proxy = new Dictionary<uint, ColliderProxy>();
        private HashSet<long> _curPairs = new HashSet<long>();
        private HashSet<long> _prePairs = new HashSet<long>();
        public const int LayerCount = 32;
        private List<ColliderProxy> tempLst = new List<ColliderProxy>();

        public ColliderProxy GetCollider(uint id){
            return id2Proxy.TryGetValue(id, out var proxy) ? proxy : null;
        }


        int[] allTypes => new int[] {0, 1, 2};

        public int[][] InterestingMasks;
        
        public void DoStart(int[][] interestingMasks){
            this.InterestingMasks = interestingMasks;
            //init _collisionMask//TODO read from file
            for (int i = 0; i < _collisionMask.Length; i++) {
                _collisionMask[i] = (uint) (~(1 << i));
            }
            // Initial size (metres), initial centre position, minimum node size (metres), looseness
            foreach (var type in allTypes) {
                var boundsTree = new BoundsQuadTree(worldSize, pos, minNodeSize, loosenessval);
                boundsTrees.Add(boundsTree);
            }

            BoundsQuadTree.FuncCanCollide = NeedCheck;
            BoundsQuadTree.funcOnCollide = OnQuadTreeCollision;
        }

        public BoundsQuadTree GetBoundTree(int layer){
            if (layer > boundsTrees.Count || layer < 0) return null;
            return boundsTrees[layer];
        }

        public void AddCollider(ColliderProxy collider){
            GetBoundTree(collider.LayerType).Add(collider, collider.GetBounds());
            id2Proxy[collider.Id] = collider;
        }


        //public List<>
        public void DoUpdate(){
            tempLst.Clear();
            //deal layer
            foreach (var pair in BoundsQuadTreeNode.obj2Node) {
                var val = pair.Key;
                if (!val.IsStatic && val._isMoved) {
                    val._isMoved = false;
                    tempLst.Add(val);
                }
            }

            //swap
            var temp = _prePairs;
            _prePairs = _curPairs;
            _curPairs = temp;
            _curPairs.Clear();
            ////class version 1.41ms
            Profiler.BeginSample("UpdateObj");
            foreach (var val in tempLst) {
                val._isMoved = false;
                var bound = val.GetBounds();
                var boundsTree = GetBoundTree(val.LayerType);
                boundsTree.UpdateObj(val, bound);
            }

            Profiler.EndSample();
            ////0.32~0.42ms
            Profiler.BeginSample("CheckCollision");
            foreach (var val in tempLst) {
                val._isMoved = false;
                var bound = val.GetBounds();
                var targetLayers = InterestingMasks[val.LayerType];
                foreach (var layerType in targetLayers) {
                    var boundsTree = GetBoundTree(layerType);
                    boundsTree.CheckCollision(val, bound);
                }
            }

            Profiler.EndSample();
            Profiler.BeginSample("CheckLastFrameCollison");
            foreach (var pairId in _curPairs) {
                _prePairs.Remove(pairId);
            }

            //check stay leave event
            foreach (var idPair in _prePairs) {
                var a = GetCollider((uint) (idPair >> 32));
                var b = GetCollider((uint) (idPair & 0xffffffff));
                if (a == null || b == null) {
                    continue;
                }

                bool isCollided = CollisionHelper.CheckCollision
                    (a.Prefab, a.Transform2D, b.Prefab, b.Transform2D);
                if (isCollided) {
                    _curPairs.Add(idPair);
                    NotifyCollisionEvent(a, b, ECollisionEvent.Stay);
                }
                else {
                    NotifyCollisionEvent(a, b, ECollisionEvent.Exit);
                }
            }

            Profiler.EndSample();
        }

        bool NeedCheck(ColliderProxy a, ColliderProxy b){
            var val = _collisionMask[a.LayerType];
            var val2 = 1 << b.LayerType;
            var needCheck = (val & val2) != 0;
            return needCheck;
        }

        public void OnQuadTreeCollision(ColliderProxy a, ColliderProxy b){
            var pairId = (((long) a.Id) << 32) + b.Id;
            if (_curPairs.Contains(pairId)) return;
            bool isCollided = CollisionHelper.CheckCollision
                (a.Prefab, a.Transform2D, b.Prefab, b.Transform2D);
            if (isCollided) {
                _curPairs.Add(pairId);
                var type = _prePairs.Contains(pairId) ? ECollisionEvent.Stay : ECollisionEvent.Enter;
                NotifyCollisionEvent(a, b, type);
            }
        }

        public void NotifyCollisionEvent(ColliderProxy a, ColliderProxy b, ECollisionEvent type){
            if (!a.IsStatic) {
                a.OnTriggerEvent?.Invoke(b, type);
                //TriggerEvent(a, b, type);
            }

            if (!b.IsStatic) {
                b.OnTriggerEvent?.Invoke(a, type);
                //TriggerEvent(b, a, type);
            }
        }

        void TriggerEvent(ColliderProxy a, ColliderProxy other, ECollisionEvent type){
            switch (type) {
                case ECollisionEvent.Enter: {
                    a.OnTriggerEnter(other);
                    break;
                }
                case ECollisionEvent.Stay: {
                    a.OnTriggerStay(other);
                    break;
                }
                case ECollisionEvent.Exit: {
                    a.OnTriggerExit(other);
                    break;
                }
            }
        }

        public int ShowTreeId { get; set; }

        public void DrawGizmos(){
            var boundsTree = GetBoundTree(ShowTreeId);
            if (boundsTree == null) return;
            boundsTree.DrawAllBounds(); // Draw node boundaries
            boundsTree.DrawAllObjects(); // Draw object boundaries
            boundsTree.DrawCollisionChecks(); // Draw the last *numCollisionsToSave* collision check boundaries

            // pointTree.DrawAllBounds(); // Draw node boundaries
            // pointTree.DrawAllObjects(); // Mark object positions
        }
    }
}