using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Test;
using Lockstep.Collision;
using Lockstep.Math;

[CustomEditor(typeof(DebugColliderProxy))]
public class EditorColliderProxy : Editor
{
    private DebugColliderProxy owner;

    Vector3 posOffset = new Vector3(0, 0, 0);
    float GizmoSize = 0.4f;
    Transform _goParent;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        owner = (DebugColliderProxy) target;

    }


    void OnSceneGUI()
    {
        
        owner = (DebugColliderProxy) target;
        //DrawPathHeap();
    }
    
    /// <summary>
    /// 绘制寻路堆栈
    /// </summary>
    void DrawPathHeap()
    {
        foreach (var col in owner.allColliders)
        {
            var _colType = col.ColType;
            switch (_colType)
            {
                // case   EColType. Sphere: {var _col = col as AABB; Handles.DrawSphere(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. AABB: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Capsule: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. OBB: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Plane: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Rect: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Segment: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
                // case  EColType. Polygon: {var _col = col as AABB; Handles.DrawWireCube(_col.c.ToVector3(), _col.r.ToVector3* GizmoSize * 10f);break;}
            }
        }
        //Gizmos.DrawCube();
    }
}