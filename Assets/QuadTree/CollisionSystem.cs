using System.Collections.Generic;
using Lockstep.Collision2D;
using Lockstep.Math;
using UnityEngine;

namespace TQuadTree1 {
    public class CBaseShape {
        public virtual int TypeId => (int) EShape2D.EnumCount;
        public int id;
    }

    public class CCircle : CBaseShape {
        public override int TypeId => (int) EShape2D.Circle;
        public LFloat radius;
    }

    public class CAABB : CCircle {
        public override int TypeId => (int) EShape2D.AABB;
        public LVector2 size;
    }

    public class COBB : CAABB {
        public override int TypeId => (int) EShape2D.OBB;
        public LFloat deg;
        public LVector2 up;
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
    }

    public class ColliderProxy {
        public int Id;
        public int LayerType { get; private set; }
        public ColliderPrefab Prefab;
        public CTransform2D Transform2D;
        public LFloat Y;
        public LFloat Height;
        public bool IsTrigger = true;

        public Rect GetBounds(){
            return new Rect();
        }

        public void OnTriggerEnter(ColliderProxy other){ }
        public void OnTriggerStay(ColliderProxy other){ }
        public void OnTriggerExit(ColliderProxy other){ }
        public void OnCollisionEnter(ColliderProxy other){ }
        public void OnCollisionStay(ColliderProxy other){ }
        public void OnCollisionExit(ColliderProxy other){ }
    }

    public class CollisionSystem {
        public uint[] _collisionMask = new uint[32];

        public Vector3 pos;
        public BoundsQuadTree<Collider> boundsTree;
        public float worldSize = 150;
        public float minNodeSize = 1;
        public float loosenessval = 1.25f;

        private Dictionary<int, ColliderProxy> id2Proxy = new Dictionary<int, ColliderProxy>();
        private HashSet<long> _pairs = new HashSet<long>();
        private List<long> _pairCache = new List<long>();

        public ColliderProxy GetCollider(int id){
            return id2Proxy.TryGetValue(id, out var proxy) ? proxy : null;
        }

        public void DoStart(){
            //init _collisionMask//TODO read from file
            for (int i = 0; i < _collisionMask.Length; i++) {
                _collisionMask[i] = 0xffffffff;
            }

            // Initial size (metres), initial centre position, minimum node size (metres), looseness
            boundsTree = new BoundsQuadTree<Collider>(worldSize, pos, minNodeSize, loosenessval);
        }

        public const int LayerCount = 32;
        //public List<>
        public void DoUpdate(){
            //
            for (int i = 0; i < LayerCount; i++) {
                for (int j = i; j < LayerCount; j++) {
                    
                }
            }
            //check stay leave event
            foreach (var idPair in _pairs) {
                var body1 = GetCollider((int) (idPair >> 32));
                var body2 = GetCollider((int) (idPair & 0xffffffff));
                if (body1 == null || body2 == null) {
                    continue;
                }
                OnQuadTreeCollision(body1, body2, false);
            }
            _pairs.Clear();
            foreach (var t in _pairCache) {
                _pairs.Add(t);
            }
            _pairCache.Clear();
        }

        bool NeedCheck(ColliderProxy a, ColliderProxy b){
            return NeedCheck(a.LayerType, b.LayerType);
        }

        bool NeedCheck(int layerType1, int layerType2){
            var val = _collisionMask[layerType1];
            return (val & 1 << layerType2) != 0;
        }

        public void OnQuadTreeCollision(ColliderProxy a, ColliderProxy b, bool remove = true){
            bool isCollided = CollisionHelper.CheckCollision
                (a.Prefab, a.Transform2D, b.Prefab, b.Transform2D);
            var paired = FindCollisionPair(a, b, remove);
            ECollisionType type = ECollisionType.Stay;
            if (isCollided) {
                type = paired ? ECollisionType.Stay : ECollisionType.Enter;
                CacheCollisionPair(a, b);
                NotifyCollisionEvent(a, b, type);
            }
            else {
                if (paired) {
                    type = ECollisionType.Exit;
                    NotifyCollisionEvent(a, b, type);
                }
            }
        }

        public void NotifyCollisionEvent(ColliderProxy a, ColliderProxy b, ECollisionType type){
            switch (type) {
                case ECollisionType.Exit: {
                    a.OnTriggerExit(b);
                    b.OnTriggerExit(a);
                }
                    break;
            }
        }

        private bool FindCollisionPair(ColliderProxy a, ColliderProxy b, bool remove = true){
            var idx = ((long) a.Id) << 32 + b.Id;
            return remove ? _pairs.Remove(idx) : _pairs.Contains(idx);
        }

        private void CacheCollisionPair(ColliderProxy a, ColliderProxy b){
            var idx = ((long) a.Id) << 32 + b.Id;
            _pairCache.Add(idx);
        }
    }
}