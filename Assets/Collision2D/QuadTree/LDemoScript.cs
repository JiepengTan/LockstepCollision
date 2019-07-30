using System.Collections.Generic;
using Lockstep.Math;
using UnityEngine;
using Peril.Physics;
using UnityEngine.Profiling;
using Random = UnityEngine.Random;

namespace Lockstep.Collision2D {
    public class LDemoScript : MonoBehaviour {
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

        public QuadTree _quadTree;
        private List<IQuadTreeBody> _quadTreeBodies = new List<IQuadTreeBody>();
        private CollisionSystemQuadTree _csQuad;

        private void Start(){
            //raw  35.43ms 38.52ms 39.05ms
            //LMath 40.7ms 38.9ms
            OnStart();
        }

        private void Update(){
            //raw 5.91~7.0ms
            //LMath 13.06ms 14.02ms  12.9ms
            Profiler.BeginSample("QuadTreeUpdate");
            OnUpdate();
            Profiler.EndSample();
        }

        private void OnStart(){
            _quadTree = new QuadTree(new LRect(LFloat.zero, LFloat.zero,
                WorldSize.x.ToLFloat(), WorldSize.y.ToLFloat()), BodiesPerNode, MaxSplits);
            _csQuad = new CollisionSystemQuadTree(_quadTree);
            var tempLst = new List<LDemoPhysicsBody>();
            for (int i = 0; i < MaxBodies; i++) {
                var body = GameObject.Instantiate<LDemoPhysicsBody>(DemoPhysicsBody);
                body.transform.position = new Vector3(Random.Range(0, WorldSize.x), 0, Random.Range(0, WorldSize.y));
                tempLst.Add(body);
            }


            Profiler.BeginSample("QuadInit");
            foreach (var body in tempLst) {
                _csQuad.AddBody(body);
                _quadTree.AddBody(body); // add body to QuadTree
                _quadTreeBodies.Add(body); // cache bodies so we can refresh the tree in update
            }

            Profiler.EndSample();
        }

        private void OnUpdate(){
            switch (CSType) {
                case CollisionSystemType.QuadTree:
                    _csQuad.Step();
                    break;
            }

            // refresh QuadTree each frame if bodies can move
            _quadTree.Clear();
            foreach (var b in _quadTreeBodies) {
                _quadTree.AddBody(b);
            }
        }

        private void OnDrawGizmos(){
            if (_quadTree == null) return;
            _quadTree.DrawGizmos();
        }
    }
}