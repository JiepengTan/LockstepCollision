using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Lockstep.Math;
using Random = UnityEngine.Random;

#if false// UNITY_EDITOR
namespace Lockstep.UnsafeCollision2D {
    public class TestCollision2D : MonoBehaviour {
        public List<ColliderConfig[]> allPairs = new List<ColliderConfig[]>();
        [Header("移动距离")]
        public float gridGap = 9;
        public float moveDist = 4;
        [Header("初始值")]
        public float rawRotateDeg = 720;
        public float rawSize = 1;
        public float rawMoveTime = 2;
        [Header("浮动率")]
        public float timeFloatRate = 0.3f;
        public float sizeFloatRate = 0.4f;
        public float degFloatRate = 0.4f;

        private void Start(){
            var count = (int) EShape2D.EnumCount;
            for (int i = 0; i < count; i++) {
                for (int j = 0; j < count; j++) {
                    LVector3 pos = new LVector3(true,(i * gridGap), (j * gridGap), 0);
                    var shape1 = CreateShape(i, (i * count + j) * 2);
                    var shape2 = CreateShape(j, (i * count + j) * 2 + 1);
                    shape1.SetPosition(pos);
                    shape2.SetPosition(pos + new LVector3(true,moveDist, 0f, 0f));
                    allPairs.Add(new ColliderConfig[2] {shape1  , shape2 });
                    StartCoroutine(
                        PingPongMove(shape1, shape1.pos, pos + new LVector3(true,moveDist, moveDist, 0f), rawMoveTime));
                    StartCoroutine(PingPongMove(shape2, shape2.pos, pos + new LVector3(true,0f, moveDist, 0f), rawMoveTime));
                }
            }
        }

        private void Update(){
            foreach (var pair in allPairs) {
                var color = Color.white;
                if (pair[0].Collider.TestWithShape(pair[1].Collider)) {
                    color = Color.red;
                }

                pair[0].DebugGizomColor = color;
                pair[1].DebugGizomColor = color;
            }
        }


        IEnumerator PingPongMove(ColliderConfig comp, LVector3 sPos, LVector3 ePos, float time){
            float timer = 0;
            var degFloat = Random.Range(1.0f - degFloatRate, 1.0f + degFloatRate).ToLFloat();
            var sizeFloat1 = Random.Range(1.0f - sizeFloatRate, 1.0f + sizeFloatRate).ToLFloat();
            var sizeFloat2 = Random.Range(1.0f - sizeFloatRate, 1.0f + sizeFloatRate).ToLFloat();
            var obb = comp.Collider as OBB2D;
            var aabb = comp.Collider as AABB2D;
            var circle = comp.Collider as Circle;
            var startDeg = comp.deg;
            var endDeg = startDeg + degFloat * (rawRotateDeg.ToLFloat());

            var startRadius = comp.Collider.radius;
            var endRandius = sizeFloat1 * rawSize.ToLFloat();
            var rawSizeVec = LVector2.one;
            var endSizeVec = LVector2.one;
            if (circle == null) {
                rawSizeVec = obb != null ? obb.size : aabb.size;
                endSizeVec = new LVector2(sizeFloat1 * rawSize.ToLFloat(), sizeFloat2 * rawSize.ToLFloat());
            }

            while (true) {
                timer += Time.deltaTime;
                if (timer > time) {
                    break;
                }

                var timeRate = (timer / time).ToLFloat();
                //change position
                comp.SetPosition(LVector3.Lerp(sPos, ePos, timeRate));
                var collider = comp.Collider;
                //change rotation
                if (aabb == null) {
                    comp.SetRotation(LMath.Lerp(startDeg, endDeg, timeRate));
                }

                //change size
                if (circle != null) {
                    circle.radius = LMath.Lerp(startRadius, endRandius, timeRate);
                }

                if (aabb != null) {
                    aabb.size = LVector2.Lerp(rawSizeVec, endSizeVec, timeRate);
                }

                if (obb != null) {
                    obb.size = LVector2.Lerp(rawSizeVec, endSizeVec, timeRate);
                }

                yield return null;
            }

            StartCoroutine(PingPongMove(comp, ePos, sPos,
                rawMoveTime * Random.Range(1.0f - timeFloatRate, 1.0f + timeFloatRate)));
        }


        public ColliderConfig CreateShape(int itype, int idx){
            var go = new GameObject("Pair" + idx / 2 + " :" + idx % 2);
            var col = go.AddComponent<ColliderConfig>();
            var type = (EShape2D) itype;
            switch (type) {
                case EShape2D.Circle:
                    col.AddCircle(LVector2.zero, LVector2.zero, 1.ToLFloat());
                    break;
                case EShape2D.AABB:
                    col.AddAABB(LVector2.zero, LVector2.zero, LVector2.one);
                    break;
                case EShape2D.OBB:
                    col.AddOBB(LVector2.zero, LVector2.zero, LVector2.one, 0.ToLFloat());
                    break;
            }

            return col;
        }
    }
}
#endif