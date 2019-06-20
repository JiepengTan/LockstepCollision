#if UNITY_5_3_OR_NEWER
using System.Collections.Generic;
using UnityEngine;
using Lockstep.Collision;
using Lockstep.Math;

public class DebugUnityColliderProxy : UnityColliderProxy {
    public Material mat;

    protected override void Start(){
        base.Start();
        mat = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = mat;
    }

}
#endif