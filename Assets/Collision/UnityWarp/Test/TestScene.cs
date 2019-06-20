#if UNITY_5_3_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Lockstep.Collision;
using Lockstep.Math;
using Random = UnityEngine.Random;

public class TestScene : MonoBehaviour {
    public enum ETestShape3D {
        Obb,
        Capsule,
        Sphere,
        EnumCount
    }

    protected void Update(){
        DebugTest();
    }


    public List<DebugUnityColliderProxy[]> allPairs = new List<DebugUnityColliderProxy[]>();
    [Header("移动距离")] public float gridGap = 9;
    public float moveDist = 4;
    [Header("初始值")] public float rawRotateDeg = 720;
    public float rawSize = 1;
    public float rawMoveTime = 2;
    [Header("浮动率")] public float timeFloatRate = 0.3f;
    public float sizeFloatRate = 0.4f;
    public float degFloatRate = 0.4f;

    private void Start(){
        var count = (int) ETestShape3D.EnumCount;
        for (int i = 0; i < count; i++) {
            for (int j = 0; j < count; j++) {
                LVector3 pos = new LVector3(true, (i * gridGap), (j * gridGap), 0);
                var shape1 = CreateShape(i, (i * count + j) * 2,
                    pos.ToVector3());
                var shape2 = CreateShape(j, (i * count + j) * 2 + 1,
                    (pos + new LVector3(true, moveDist, 0f, 0f)).ToVector3());
                allPairs.Add(new DebugUnityColliderProxy[2] {shape1, shape2});
                StartCoroutine(
                    PingPongMove(shape1, shape1.transform.position.ToLVector3(),
                        pos + new LVector3(true, moveDist, moveDist, 0f), rawMoveTime));
                StartCoroutine(
                    PingPongMove(shape2, shape2.transform.position.ToLVector3(),
                        pos + new LVector3(true, 0f, moveDist, 0f), rawMoveTime));
            }
        }
    }

    IEnumerator PingPongMove(DebugUnityColliderProxy comp, LVector3 sPos, LVector3 ePos, float time){
        float timer = 0;
        Vector3 deg;
        deg.x = Random.Range(1.0f - degFloatRate, 1.0f + degFloatRate);
        deg.y = Random.Range(1.0f - degFloatRate, 1.0f + degFloatRate);
        deg.z = Random.Range(1.0f - degFloatRate, 1.0f + degFloatRate);
        var startDeg = comp.transform.localRotation.eulerAngles.ToLVector3();
        var endDeg = (startDeg.ToVector3() + (deg * rawRotateDeg)).ToLVector3();

        var obb = comp.Collider as OBB;
        var aabb = comp.Collider as AABB;
        var sphere = comp.Collider as Sphere;

        while (true) {
            timer += Time.deltaTime;
            if (timer > time) {
                break;
            }

            var timeRate = (timer / time).ToLFloat();
            //change position
            comp.transform.position = (LVector3.Lerp(sPos, ePos, timeRate).ToVector3());
            var collider = comp.Collider;
            //change rotation
            if (aabb == null) {
                Vector3 tdeg = LMath.Lerp(startDeg, endDeg, timeRate).ToVector3();
                comp.transform.rotation = Quaternion.Euler(tdeg.x, tdeg.y, tdeg.z);
            }

            yield return null;
        }

        StartCoroutine(PingPongMove(comp, ePos, sPos,
            rawMoveTime * UnityEngine.Random.Range(1.0f - timeFloatRate, 1.0f + timeFloatRate)));
    }

    private void DebugTest(){
        foreach (var pair in allPairs) {
            var hasCollidedOthers = ColliderProxy.TestColliderProxy(pair[0], pair[1]);
            pair[0].mat.color = hasCollidedOthers ? Color.red : Color.white;
            pair[1].mat.color = hasCollidedOthers ? Color.red : Color.white;
        }
    }

    public DebugUnityColliderProxy CreateShape(int iType, int idx, Vector3 pos){
        PrimitiveType[] types = new PrimitiveType[(int) ETestShape3D.EnumCount] {
            PrimitiveType.Cube,
            PrimitiveType.Capsule,
            PrimitiveType.Sphere,
        };
        var col = GameObject.CreatePrimitive(types[iType]);
        col.name = "Pair" + idx / 2 + " :" + idx % 2;
        col.transform.position = pos;
        var proxy = col.AddComponent<DebugUnityColliderProxy>();
        AddTestCollider(proxy, col, types[iType]);
        Object.Destroy(col.GetComponent<Collider>());
        return proxy;
    }


    public void AddTestCollider(DebugUnityColliderProxy proxy, GameObject obj, PrimitiveType type){
        switch (type) {
            case PrimitiveType.Cube: {
                var _col = new OBB();
                _col.c = (obj.transform.position).ToLVector3();
                _col.e = (transform.localScale * 0.5f).ToLVector3();
                _col.u = new LAxis3D(transform.right.ToLVector3(), transform.up.ToLVector3(),
                    transform.forward.ToLVector3());
                proxy.AddCollider(_col, ColliderLocalInfo.identity);
                break;
            }
            case PrimitiveType.Sphere: {
                var _col = new Sphere(obj.transform.position.ToLVector3(), 0.5f.ToLFloat());
                proxy.AddCollider(_col, ColliderLocalInfo.identity);
                break;
            }
            case PrimitiveType.Capsule: {
                var hDir = (Vector3.up * 0.5f).ToLVector3();
                var c = (obj.transform.position).ToLVector3();
                var r = 0.5f.ToLFloat();
                var _col = new Capsule(c, hDir, r);
                proxy.AddCollider(_col, ColliderLocalInfo.identity);
                break;
            }
        }
    }
}
#endif