using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Lockstep.Math;
using UnityEngine;
using UnityEngine.Profiling;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

namespace TQuadTree1 {


    public class TestQuadTree : MonoBehaviour {
        public Vector3 pos;

        // Initial size (metres), initial centre position, minimum node size (metres), looseness
        BoundsQuadTree<Collider> boundsTree;

        public List<Collider> objs = new List<Collider>();
        public List<Material> mats = new List<Material>();

        public float worldSize = 150;
        public float minNodeSize = 1;
        public float loosenessval = 1.25f;

        private void Start(){
            // Initial size (metres), initial centre position, minimum node size (metres), looseness
            boundsTree = new BoundsQuadTree<Collider>(worldSize, pos, minNodeSize, loosenessval);
            // Initial size (metres), initial centre position, minimum node size (metres)
            //pointTree = new PointOctree<GameObject>(150, .pos, 1);
            for (int i = 0; i < count; i++) {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Collider>();
                obj.transform.SetParent(transform, false);
                obj.transform.position = new Vector3(Random.Range(0, worldSize), 0, Random.Range(0, worldSize));
                obj.transform.localScale = new Vector3(Random.Range(1, 4), Random.Range(1, 4), Random.Range(1, 4));

                var mat = new Material(obj.GetComponent<Renderer>().material);
                obj.GetComponent<Renderer>().material = mat;
                if (i % Mathf.CeilToInt(1 / percent) == 0) {
                    StartCoroutine(RandomMove(obj, () => {
                        boundsTree.Remove(obj);
                        boundsTree.Add(obj, obj.bounds.ToRect());
                    }));
                }
                else if (i % Mathf.CeilToInt(1 / percent) == 1) {
                    objs.Add(obj);
                    mats.Add(mat);
                    isCollide.Add(false);
                    StartCoroutine(RandomMove(obj, null));
                }
                else {
                    boundsTree.Add(obj, obj.bounds.ToRect());
                }
            }

            testObj = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            mat = new Material(testObj.GetComponent<Renderer>().material);
            testObj.GetComponent<Renderer>().material = mat;
        }

        public Transform testObj;
        public Material mat;
        public Rect bound;
        public float percent = 0.1f;
        public int count = 100;

        private void Update(){
            Profiler.BeginSample("CheckCollision");
            CheckCollision();
            Profiler.EndSample();
            for (int i = 0; i < objs.Count; i++) {
                mats[i].color = isCollide[i] ? Color.red : Color.green;
            }
        }

        private List<bool> isCollide = new List<bool>();

        private void CheckCollision(){
            for (int i = 0; i < objs.Count; i++) {
                var obj = objs[i];
                isCollide[i] = boundsTree.IsColliding(obj.bounds.ToRect());
            }
        }

        public IEnumerator RandomMove(Collider obj, Action func){
            float timer = 0;
            Vector3 targetPos = new Vector3();
            float spd = 3;

            while (true) {
                timer += Time.deltaTime;
                if (timer > 2) {
                    timer = 0;
                    targetPos = new Vector3(Random.Range(0, worldSize), 0, Random.Range(0, worldSize));
                }

                yield return null;
                obj.transform.position += (targetPos - obj.transform.position).normalized * Time.deltaTime * spd;
                func?.Invoke();
            }
        }

        void OnDrawGizmos(){
            if (boundsTree == null) return;

            //boundsTree.DrawAllBounds(); // Draw node boundaries
            boundsTree.DrawAllObjects(); // Draw object boundaries
            boundsTree.DrawCollisionChecks(); // Draw the last *numCollisionsToSave* collision check boundaries

            // pointTree.DrawAllBounds(); // Draw node boundaries
            // pointTree.DrawAllObjects(); // Mark object positions
        }
    }
}