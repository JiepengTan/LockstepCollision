using System.Collections.Generic;
using Lockstep.Math;
using UnityEngine;
using Peril.Physics;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

namespace Lockstep.Collision2D {
    public unsafe class LDemoScript : MonoBehaviour {
        public enum CollisionSystemType {
            Brute,
            QuadTree
        }

        public LDemoPhysicsBody DemoPhysicsBody;
        [Header("CollisionSystem Settings")] public CollisionSystemType CSType;
        public int MaxBodies = 500;

        [Header("QuadTree Settings")] public Vector2 WorldSize = new Vector2(200, 200);
        public int BodiesPerNode = 6;
        public int MaxSplits = 6;

        public QuadTree* _quadTree;
        private CollisionSystemQuadTree _collisionSystem;

        private void Start(){
            OnStart();
        }

        private void Update(){
            //raw 5.91~7.0ms
            //LMath 13.06ms 14.02ms  12.9ms
            //Unsafe LMath 7.0~7.5ms
            Profiler.BeginSample("QuadTreeUpdate");
            OnUpdate();
            Profiler.EndSample();
        }

        private void OnDestroy(){
            if (_quadTree != null) {
                //QuadTreeFactory.FreeQuadTree(_quadTree);
                //_quadTree = null;
            }
        }

        private void OnStart(){
            _quadTree = QuadTreeFactory.AllocQuadTree();
            *_quadTree = new QuadTree(new LRect(LFloat.zero, LFloat.zero,
                WorldSize.x.ToLFloat(), WorldSize.y.ToLFloat()), BodiesPerNode, MaxSplits);
            _collisionSystem = new CollisionSystemQuadTree(_quadTree);
            var tempLst = new List<LDemoPhysicsBody>();
            for (int i = 0; i < MaxBodies; i++) {
                var body = GameObject.Instantiate<LDemoPhysicsBody>(DemoPhysicsBody);
                body.transform.position = new Vector3(Random.Range(0, WorldSize.x), 0, Random.Range(0, WorldSize.y));
                tempLst.Add(body);
            }
            //raw  35.43ms 38.52ms 39.05ms
            //LMath 40.7ms 38.9ms
            //UnsafeLMath 8.6ms 8.7ms
            Profiler.BeginSample("QuadInit");
            foreach (var body in tempLst) {
                AABB2D* boxPtr = CollisionFactory.AllocAABB();
                body.RefId = _collisionSystem.AddBody(body,boxPtr, body.Position, body.Extents);
                body.ColPtr = (Sphere2D*)boxPtr;
                _quadTree->AddBody(boxPtr); // add body to QuadTree
            }

            Profiler.EndSample();
        }

        private void OnUpdate(){
            _collisionSystem.Step();
            countDetectBodyVsBody = _collisionSystem.countDetectBodyVsBody;
            _quadTree->Clear();
            addBodyCount = 0;
            _collisionSystem.IteratePtrs((ptr) => {
                addBodyCount++;
                _quadTree->AddBody(ptr);
            });
        }

        public int addBodyCount = 0;
        public int countDetectBodyVsBody;
        private void OnDrawGizmos(){
            if (_quadTree == null) return;
            _quadTree->DrawGizmos();
        }
    }
}