#if UNITY_5_3_OR_NEWER
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var _temp = quaternion2Euler(transform.rotation, RotSeq.yxz) * Mathf.Rad2Deg;
        convertResult = new Vector3(_temp.y,_temp.z,_temp.x);
        myret =  operatorMul(q1,q2);
        unityret = q1 * q2;
    }

    public Vector3 convertResult;
    public RotSeq rotseq;
    
    public  enum RotSeq
    {
        zyx, zyz, zxy, zxz, yxz, yxy, yzx, yzy, xyz, xyx, xzy,xzx
    };
 
    //yxz
    Vector3 threeaxisrot(float r11, float r12, float r21, float r31, float r32){
        Vector3 ret = new Vector3();
        ret.x = Mathf.Atan2( r31, r32 );
        ret.y = Mathf.Asin ( r21 );
        ret.z = Mathf.Atan2( r11, r12 );
        return ret;
    }

    public Quaternion q1;
    public Quaternion q2;
    public Quaternion myret;
    public Quaternion unityret;
    Quaternion operatorMul(Quaternion q1, Quaternion q2){
        Quaternion q;
        q.w = q1.w*q2.w - q1.x*q2.x - q1.y*q2.y - q1.z*q2.z;
        q.x = q1.w*q2.x + q1.x*q2.w + q1.y*q2.z - q1.z*q2.y;
        q.y = q1.w*q2.y - q1.x*q2.z + q1.y*q2.w + q1.z*q2.x;
        q.z = q1.w*q2.z + q1.x*q2.y - q1.y*q2.x + q1.z*q2.w;
        return q;
    }
    
    Vector3 quaternion2Euler(Quaternion q, RotSeq rotSeq)
    {
        switch(rotSeq){
 
        case RotSeq.yxz:
            return threeaxisrot( 2*(q.x*q.z + q.w*q.y),
                q.w*q.w - q.x*q.x - q.y*q.y + q.z*q.z,
                -2*(q.y*q.z - q.w*q.x),
                2*(q.x*q.y + q.w*q.z),
                q.w*q.w - q.x*q.x + q.y*q.y - q.z*q.z);
 
       
        default:
            Debug.LogError("No good sequence");
            return Vector3.zero;
 
        }
 
    }
}
#endif
