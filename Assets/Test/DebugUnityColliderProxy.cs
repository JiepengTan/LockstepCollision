using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Lockstep.Collision;
using Lockstep.Math;

namespace Test {
    public class DebugUnityColliderProxy : UnityColliderProxy {
        public static List<DebugUnityColliderProxy> allProxys = new List<DebugUnityColliderProxy>();
        public Material mat;

        protected override void Start(){
            base.Start();
            allProxys.Add(this);
            mat = new Material(GetComponent<Renderer>().material);
            GetComponent<Renderer>().material = mat;
        }

        protected override void Update(){
            base.Update();
            DebugTest();
        }

        private void DebugTest(){
            bool hasCollidedOthers = false;
            //TODO 暴力测试 改为更加 性能友好的判定方式
            foreach (var col in allProxys) {
                if (col != this) {
                    if (TestColliderProxy(this, col)) {
                        hasCollidedOthers = true;
                        break;
                    }
                }
            }

            mat.color = hasCollidedOthers ? Color.red : Color.white;
        }
        
        public static bool TestColliderProxy(DebugUnityColliderProxy a, DebugUnityColliderProxy b){
            //使用暴力测试方式 
            bool hasCollidedOthers = false;
            var isCollided = Utils.TestSphereSphere(a.boundSphere, b.boundSphere);
            if (isCollided) {
                foreach (var cCola in a.allColliders) {
                    foreach (var cColb in b.allColliders) {
                        if (BaseShape.TestShapeWithShape(cCola, cColb)) {
                            hasCollidedOthers = true;
                            break;
                        }
                    }
                }
            }

            return hasCollidedOthers;
        }
        public void AddTestCollider(GameObject obj, PrimitiveType type){
            switch (type) {
                case PrimitiveType.Cube: {
                    var _col = new OBB();
                    _col.c = (obj.transform.position).ToLVector3();
                    _col.e = (transform.localScale * 0.5f).ToLVector3();
                    _col.u = new LAxis3D(transform.right.ToLVector3(), transform.up.ToLVector3(),
                        transform.forward.ToLVector3());
                    AddCollider(_col, ColliderLocalInfo.identity);
                    break;
                }
                case PrimitiveType.Sphere: {
                    var _col = new Sphere(obj.transform.position.ToLVector3(), 0.5f.ToLFloat());
                    AddCollider(_col, ColliderLocalInfo.identity);
                    break;
                }
                case PrimitiveType.Capsule: {
                    var hDir = (Vector3.up * 0.5f).ToLVector3();
                    var c = (obj.transform.position).ToLVector3();
                    var r = 0.5f.ToLFloat();
                    var _col = new Capsule(c, hDir, r);
                    AddCollider(_col, ColliderLocalInfo.identity);
                    break;
                }
            }
        }

    }
}