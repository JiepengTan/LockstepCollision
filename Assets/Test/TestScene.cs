using System.Collections;
using System.Collections.Generic;
using LockStepCollision;
using Test;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    public int count = 6;  
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            CreateCollider(PrimitiveType.Cube, new Vector3(2, i * 2, 0));
            CreateCollider(PrimitiveType.Capsule, new Vector3(4, i * 2, 0));
            CreateCollider(PrimitiveType.Sphere, new Vector3(6, i * 2, 0));
        }
    }

    void CreateCollider(PrimitiveType type,Vector3 pos)
    {
        var col = GameObject.CreatePrimitive(type);
        col.transform.position = pos;
        col.AddComponent<DebugColliderProxy>().AddTestCollider(col,type);
    }
}
