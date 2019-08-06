using System.Collections.Generic;
using Lockstep.Math;
using Lockstep.UnsafeCollision2D;
using UnityEngine;
using UnityEngine.Profiling;
using Random = System.Random;

namespace Lockstep.Collision2D {
    public delegate void FuncGlobalOnTriggerEvent(ColliderProxy a, ColliderProxy b, ECollisionEvent type);

    public class CollisionSystem : ICollisionSystem {
        public uint[] _collisionMask = new uint[32];

        public List<BoundsQuadTree> boundsTrees = new List<BoundsQuadTree>();
        public LFloat worldSize = 150.ToLFloat();
        public LFloat minNodeSize = 1.ToLFloat();
        public LFloat loosenessval = new LFloat(true, 1250);
        public LVector3 pos;

        private Dictionary<int, ColliderProxy> id2Proxy = new Dictionary<int, ColliderProxy>();
        private HashSet<long> _curPairs = new HashSet<long>();
        private HashSet<long> _prePairs = new HashSet<long>();
        public const int LayerCount = 32;
        private List<ColliderProxy> tempLst = new List<ColliderProxy>();

        public FuncGlobalOnTriggerEvent funcGlobalOnTriggerEvent;

        public ColliderProxy GetCollider(int id){
            return id2Proxy.TryGetValue(id, out var proxy) ? proxy : null;
        }


        public int[] AllTypes;
        public int[][] InterestingMasks;

        public void DoStart(int[][] interestingMasks, int[] allTypes){
            this.InterestingMasks = interestingMasks;
            this.AllTypes = allTypes;
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
        public void RemoveCollider(ColliderProxy collider){
            GetBoundTree(collider.LayerType).Remove(collider);
            id2Proxy.Remove(collider.Id);
        }   
        public bool Raycast(int layerType, Ray2D checkRay, out LFloat t,out int id,LFloat maxDistance){
            return GetBoundTree(layerType).Raycast(checkRay, maxDistance,out t,out id);
        }
        public bool Raycast(int layerType, Ray2D checkRay, out LFloat t,out int id){
            return GetBoundTree(layerType).Raycast(checkRay, LFloat.MaxValue,out t,out id);
        }
        
        public static ColliderPrefab CreateColliderPrefab(GameObject fab){
            Collider unityCollider = null;
            var colliders = fab.GetComponents<Collider>();
            foreach (var col in colliders) {
                if (col.isTrigger) {
                    unityCollider = col;
                    break;
                }
            }

            if (unityCollider == null) {
                foreach (var col in colliders) {
                    unityCollider = col;
                    break;
                }
            }

            if (unityCollider == null) return null;
            CBaseShape collider = null;
            if (unityCollider is BoxCollider boxCol) {
                collider = new COBB(boxCol.size.ToLVector2XZ(), LFloat.zero);
            }

            if (unityCollider is SphereCollider cirCol) {
                collider = new CCircle(cirCol.radius.ToLFloat());
            }

            if (unityCollider is CapsuleCollider capCol) {
                collider = new CCircle(capCol.radius.ToLFloat());
            }

            var colFab = new ColliderPrefab();
            colFab.parts.Add(new ColliderPart() {
                transform = new CTransform2D(LVector2.zero),
                collider = collider
            });
            return colFab;
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
                var a = GetCollider((int) (idPair >> 32));
                var b = GetCollider((int) (idPair & 0xffffffff));
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
            funcGlobalOnTriggerEvent?.Invoke(a, b, type);

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
                    a.OnLPTriggerEnter(other);
                    break;
                }
                case ECollisionEvent.Stay: {
                    a.OnLPTriggerStay(other);
                    break;
                }
                case ECollisionEvent.Exit: {
                    a.OnLPTriggerExit(other);
                    break;
                }
            }
        }

        public static void TriggerEvent(ILPTriggerEventHandler a, ColliderProxy other, ECollisionEvent type){
            switch (type) {
                case ECollisionEvent.Enter: {
                    a.OnLPTriggerEnter(other);
                    break;
                }
                case ECollisionEvent.Stay: {
                    a.OnLPTriggerStay(other);
                    break;
                }
                case ECollisionEvent.Exit: {
                    a.OnLPTriggerExit(other);
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