using System;
using System.Collections.Generic;
using Lockstep.Collision2D;
using Lockstep.Math;
using UnityEngine;
using Peril.Physics;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

namespace Lockstep.UnsafeCollision2D {
    public unsafe class LDemoScript : MonoBehaviour {
        public enum CollisionSystemType {
            Brute,
            QuadTree
        }

        public PhysicsBody DemoPhysicsBody;
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
            //LRect Simple 5.3~6.4ms
            //QuadTree MarkDirty 2.4ms
            Profiler.BeginSample("QuadTreeUpdate");
            OnUpdate();
            Profiler.EndSample();
        }

        private void OnDestroy(){
            Debug.Log("Collision Quit :OnDestroy");
            if (_quadTree != null) {
                _quadTree->Clear();
                QuadTreeFactory.FreeQuadTree(_quadTree);
                _quadTree = null;
            }
            NativeFactory.Clear();
            Debug.Log($"RemainMemSize: NativeHelper.MemSize {NativeHelper.MemSize}");
            Debug.Assert(NativeHelper.MemSize == 0,$"NativeHelper.MemSize {NativeHelper.MemSize}");
        }

        public static System.Random random;
        public float RandomMovePercent = 0.1f;
        private void OnStart(){
            random = new System.Random(0);
            _quadTree = QuadTreeFactory.AllocQuadTree();
            *_quadTree = new QuadTree(new LRect(LFloat.zero, LFloat.zero,
                WorldSize.x.ToLFloat(), WorldSize.y.ToLFloat()), BodiesPerNode, MaxSplits);
            _collisionSystem = new CollisionSystemQuadTree(_quadTree);
            var tempLst = new List<PhysicsBody>();
            RandomMove.border = WorldSize;
            for (int i = 0; i < MaxBodies; i++) {
                var body = GameObject.Instantiate<PhysicsBody>(DemoPhysicsBody);
                body.transform.position = new Vector3(
                    random.Next(0, (int) (WorldSize.x * 1000)) * 0.001f, 0,
                    random.Next(0, (int) (WorldSize.y * 1000)) * 0.001f);
                if (i % (int)(1/RandomMovePercent) == 0) {
                    body.gameObject.AddComponent<RandomMove>();
                }

                tempLst.Add(body);
            }

            GameObject.Destroy(DemoPhysicsBody.gameObject);
            //raw  35.43ms 38.52ms 39.05ms
            //LMath 40.7ms 38.9ms
            //UnsafeLMath 8.6ms 8.7ms
            Profiler.BeginSample("QuadInit");
            foreach (var body in tempLst) {
                var config = body.ColliderConfig;
                foreach (var collider in config.allColliders) {
                    var type = (EShape2D)collider.TypeId;
                    switch (type) {
                        case EShape2D.AABB: {
                            AABB2D* boxPtr = CollisionFactory.AllocAABB();
                            var shape = ((ShapeWrapAABB) collider).shape;
                            body.RefId = _collisionSystem.AddBody(body, boxPtr, shape.pos, shape.size);
                            body.ColPtr = (Circle*) boxPtr;
                            _quadTree->AddBody(boxPtr); // add body to QuadTree
                            break;
                        }
                    }
                }
            }

            Profiler.EndSample();
        }

        private void OnUpdate(){
            NativeFactoryMemSize = NativeFactory.MemSize;
            NativeHelperMemSize = NativeHelper.MemSize;
            _collisionSystem.Step();
            countDetectBodyVsBody = _collisionSystem.countDetectBodyVsBody;
            Profiler.BeginSample("Recalc QuadTree");
            addBodyCount = CollisionSystem.dirtyBodys.Count;
            foreach (var body in CollisionSystem.dirtyBodys) {
                if (body.ColPtr == null) throw new Exception("CollisionBody have no unsafe CollisionProxy");
                QuadTree.RemoveNode(body.ColPtr);
                _quadTree->AddBody(body.ColPtr);
            }

            CollisionSystem.CleanDirtyBodies();
            Profiler.EndSample();
        }

        public int addBodyCount = 0;
        public int countDetectBodyVsBody;

        public long NativeFactoryMemSize;
        public long NativeHelperMemSize;
        private void OnDrawGizmos(){
            if (_quadTree == null) return;
            _quadTree->DrawGizmos();
        }
    }
}