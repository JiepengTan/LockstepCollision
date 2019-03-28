using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using LockStepMath;

/*
*/
namespace LockStepCollision
{
    public partial class BaseShape
    {
        public virtual void OnDrawGizmos(bool isGizmo,Color color)
        {
        }
    }
    
    public partial class AABB
    {
        public override void OnDrawGizmos(bool isGizmo,Color color)
        {
#if UNITY_EDITOR
            DebugExtension.DebugLocalCube(Matrix4x4.TRS(c.ToVector3,Quaternion.identity, Vector3.one),r.ToVector3, color);
#endif
        }
    }
    
    public partial class OBB
    {
        public override void OnDrawGizmos(bool isGizmo,Color color)
        {
#if UNITY_EDITOR
            DebugExtension.DebugLocalCube(Matrix4x4.TRS(c.ToVector3,Quaternion.identity, Vector3.one),e.ToVector3, color);
#endif
        }
    }

    public partial class Sphere
    {
        public override void OnDrawGizmos(bool isGizmo,Color color)
        {
#if UNITY_EDITOR
            DebugExtension.DebugWireSphere(c.ToVector3, color,r.ToFloat);
#endif
        }
    }
    
    public partial class Capsule
    {
        public override void OnDrawGizmos(bool isGizmo,Color color)
        {
#if UNITY_EDITOR
            DebugExtension.DrawCapsule(a.ToVector3, b.ToVector3,color,r.ToFloat);
#endif
        }
    }
}

