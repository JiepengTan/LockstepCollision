using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    public int count = 6;  
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var obb = GameObject.CreatePrimitive((PrimitiveType.Cube));
            obb.transform.position = new Vector3(2, i * 2,0);
            var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            capsule.transform.position = new Vector3(4, i * 2,0);
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(6, i * 2,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
