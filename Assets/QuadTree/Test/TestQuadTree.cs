using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using Lockstep.Collision2D;
using Lockstep.Math;
using UnityEngine;
using UnityEngine.Profiling;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

namespace TQuadTree1 {
    //Test 1000 count 
    //percent 0.4
    //world size 300
    //loosensess val 2.0f
    public class TestQuadTree : MonoBehaviour {
        public Vector3 pos;

        CollisionSystem collisionSystem;
        public float worldSize = 150;
        public float minNodeSize = 1;
        public float loosenessval = 1.25f;

        private float halfworldSize => worldSize / 2 - 5;

        private List<ColliderPrefab> prefabs = new List<ColliderPrefab>();

        public float percent = 0.1f;
        public int count = 100;

        private void Start(){
            // Initial size (metres), initial centre position, minimum node size (metres), looseness
            collisionSystem = new CollisionSystem() {
                worldSize = worldSize,
                pos = pos,
                minNodeSize = minNodeSize,
                loosenessval = loosenessval
            };
            collisionSystem.DoStart();
            //init prefab 
            const int size = 4;
            for (int i = 1; i < size; i++) {
                for (int j = 1; j < size; j++) {
                    var prefab = new ColliderPrefab();
                    prefab.parts.Add(new ColliderPart() {
                        transform = new CTransform2D(LVector2.zero),
                        collider = new CAABB(new LVector2(i, j))
                    });
                    prefabs.Add(prefab);
                }
            }

            for (int i = 0; i < count; i++) {
                var prefab = prefabs[Random.Range(0, prefabs.Count)];
                var colInfo = (CAABB) prefab.collider;
                var obj = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Collider>();

                obj.transform.SetParent(transform, false);
                obj.transform.position = new Vector3(Random.Range(-halfworldSize, halfworldSize), 0,
                    Random.Range(-halfworldSize, halfworldSize));
                obj.transform.localScale = new Vector3(colInfo.size.x.ToFloat() * 2, 1, colInfo.size.y.ToFloat() * 2);

                var proxy = new ColliderProxy();
                proxy.Init(prefab, obj.transform.position.ToLVector2XZ());
                proxy.UnityTransform = obj.transform;
                var mono = obj.gameObject.AddComponent<ColliderProxyMono>();
                mono.proxy = proxy;
                if (i < percent * count * 2) {
                    obj.gameObject.AddComponent<RandomMove>().halfworldSize = halfworldSize;
                    proxy.LayerType = 1;
                    mono.rawColor = Color.yellow;
                    if (i < percent * count) {
                        mono.rawColor = Color.green;
                        proxy.LayerType = 2;
                    }
                }
                else {
                    proxy.IsStatic = true;
                    proxy.LayerType = 0;
                }

                collisionSystem.AddCollider(proxy);
            }
        }

        public int showTreeId = 0;
        private void Update(){
            
            collisionSystem.showTreeId = showTreeId;
            collisionSystem.DoUpdate();
            ////class version 1.41ms
            //Profiler.BeginSample("CheckCollision");
            //CheckCollision();
            //Profiler.EndSample();
            ////0.32~0.42ms
            //Profiler.BeginSample("UpdateObj");
            //CheckUpdate();
            //Profiler.EndSample();
        }


        void OnDrawGizmos(){
            collisionSystem?.DrawGizmos();
        }
    }
}