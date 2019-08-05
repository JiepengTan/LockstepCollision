using System.Collections.Generic;
using Lockstep.Math;
using Lockstep.UnsafeCollision2D;
using UnityEngine;
using UnityEngine.Profiling;
using Random = System.Random;

namespace Lockstep.Collision2D {
    public class CollisionSystem : ICollisionSystem {
        public uint[] _collisionMask = new uint[32];

        public List<BoundsQuadTree> boundsTrees = new List<BoundsQuadTree>();
        public LFloat worldSize = 150.ToLFloat();
        public LFloat minNodeSize = 1.ToLFloat();
        public LFloat loosenessval = new LFloat(true,1250);
        public LVector3 pos;

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