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

        // Initial size (metres), initial centre position, minimum node size (metres), looseness
        BoundsQuadTree<Collider> boundsTree;

        public List<Collider> objs = new List<Collider>();
        public List<Material> mats = new List<Material>();
        public List<Collider> updateObjs = new List<Collider>();
        public List<Collider> staticObjs = new List<Collider>();

        public float worldSize = 150;
        public float minNodeSize = 1;
        public float loosenessval = 1.25f;

        private float halfworldSize => worldSize / 2 - 5;

        private void Start(){
            // Initial size (metres), initial centre position, minimum node size (metres), looseness
            boundsTree = new BoundsQuadTree<Collider>(worldSize, pos, minNodeSize, loosenessval);
            // Initial size (metres), initial centre position, minimum node size (metres)
            //pointTree = new PointOctree<GameObject>(150, .pos, 1);
            for (int i = 0; i < count; i++) {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Collider>();

                obj.transform.SetParent(transform, false);

                obj.transform.position = new Vector3(Random.Range(-halfworldSize, halfworldSize), 0,
                    Random.Range(-halfworldSize, halfworldSize));
                obj.transform.localScale = new Vector3(Random.Range(1, 4), Random.Range(1, 4), Random.Range(1, 4));

                var mat = new Material(obj.GetComponent<Renderer>().material);
                obj.GetComponent<Renderer>().material = mat;
                if (i < percent * count) {
                    StartCoroutine(RandomMove(obj, null));
                    updateObjs.Add(obj);
                }
                else if (i < percent * count * 2) {
                    objs.Add(obj);
                    mats.Add(mat);
                    isCollide.Add(false);
                    StartCoroutine(RandomMove(obj, null));
                }
                else {
                    staticObjs.Add(obj);
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

        private bool hasInit = false;

        private void Update(){
            if (!hasInit) {
                hasInit = true;
                foreach (var obj in staticObjs) {
                    boundsTree.UpdateObj(obj, obj.bounds.ToRect());
                }
            }

            //class version 1.41ms
            Profiler.BeginSample("CheckCollision");
            CheckCollision();
            Profiler.EndSample();
            //0.32~0.42ms
            Profiler.BeginSample("UpdateObj");
            CheckUpdate();
            Profiler.EndSample();
            for (int i = 0; i < objs.Count; i++) {
                mats[i].color = isCollide[i] ? Color.red : Color.green;
            }
        }

        private void CheckUpdate(){
            for (int i = 0; i < updateObjs.Count; i++) {
                var obj = updateObjs[i];
                boundsTree.UpdateObj(obj, obj.bounds.ToRect());
            }
        }

        private List<bool> isCollide = new List<bool>();

        private void CheckCollision(){
            for (int i = 0; i < objs.Count; i++) {
                var obj = objs[i];
                isCollide[i] = false;
                boundsTree.CheckCollision(obj, obj.bounds.ToRect(), (_obj) => { isCollide[i] = true; });
            }
        }

        //class version 1.81 ~2.23 ms
        public IEnumerator RandomMove(Collider obj, Action func){
            float timer = 0;
            Vector3 targetPos = new Vector3();
            float spd = 3;
            targetPos = new Vector3(Random.Range(-halfworldSize, halfworldSize), 0,
                Random.Range(-halfworldSize, halfworldSize));
            while (true) {
                timer += Time.deltaTime;
                if (timer > 50) {
                    timer = 0;
                    targetPos = new Vector3(Random.Range(-halfworldSize, halfworldSize), 0,
                        Random.Range(-halfworldSize, halfworldSize));
                }

                yield return null;
                if ((obj.transform.position - targetPos).sqrMagnitude < 1) {
                    targetPos = new Vector3(Random.Range(-halfworldSize, halfworldSize), 0,
                        Random.Range(-halfworldSize, halfworldSize));
                }

                obj.transform.position += (targetPos - obj.transform.position).normalized * Time.deltaTime * spd;


                func?.Invoke();
            }
        }

        void OnDrawGizmos(){
            if (boundsTree == null) return;

            boundsTree.DrawAllBounds(); // Draw node boundaries
            boundsTree.DrawAllObjects(); // Draw object boundaries
            boundsTree.DrawCollisionChecks(); // Draw the last *numCollisionsToSave* collision check boundaries

            // pointTree.DrawAllBounds(); // Draw node boundaries
            // pointTree.DrawAllObjects(); // Mark object positions
        }
    }
}