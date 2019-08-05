using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Lockstep.Math;

namespace Lockstep.UnsafeCollision2D {
    [CustomEditor(typeof(ColliderConfig))]
    public class EditorCollider2DMono : Editor {
        public ColliderConfig owner;

        private int removeIdx = 0;
        private int addTypeID = 0;

        public override void OnInspectorGUI(){
            base.OnInspectorGUI();
            owner = (ColliderConfig) target;
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Type:");
                if (int.TryParse(GUILayout.TextField(addTypeID.ToString()), out int idx)) {
                    addTypeID = idx;
                    addTypeID = Mathf.Clamp(addTypeID, 0, 2);
                }

                GUILayout.Label("removeIdx:");
                if (int.TryParse(GUILayout.TextField(removeIdx.ToString()), out int _idx)) {
                    removeIdx = Mathf.Clamp(_idx, 0, owner.allOffsets.Count - 1);
                }

                GUILayout.EndHorizontal();
            }
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("+")) {
                    IShape2D shape = null;
                    var type = (EShape2D) addTypeID;
                    if (type == EShape2D.Circle) {
                        owner.AddCircle(LVector2.zero, owner.transform.position.ToLVector3(), 1.ToLFloat());
                    }
                    else if (type == EShape2D.AABB) {
                        owner.AddAABB(LVector2.zero, owner.transform.position.ToLVector3(), LVector2.one);
                    }
                    else if (type == EShape2D.OBB) {
                        owner.AddOBB(LVector2.zero, owner.transform.position.ToLVector3(), LVector2.one, 01.ToLFloat());
                    }
                }

                if (GUILayout.Button("-")) {
                    if (removeIdx >= owner.allOffsets.Count)
                        return;
                    owner.Remove(removeIdx);
                }

                GUILayout.EndHorizontal();
            }
            //Draw Property
            {
                var count = owner.allOffsets.Count;
                for (int i = 0; i < count; i++) {
                    var shapeWrap = owner.allColliders[i];
                    var shapeType = (EShape2D) shapeWrap.TypeId;
                    switch (shapeType) {
                        case EShape2D.Circle:
                            ShowShape(ref ((ShapeWrapCircle) shapeWrap).shape, i);
                            break;
                        case EShape2D.AABB:
                            ShowShape(ref ((ShapeWrapAABB) shapeWrap).shape, i);
                            break;
                        case EShape2D.OBB:
                            ShowShape(ref ((ShapeWrapOBB) shapeWrap).shape, i);
                            break;
                    }
                }
            }
        }


        void ShowShape(ref Circle circle, int idx){
            ShowShape(ref circle, idx, DrawProperty);
        }

        void ShowShape(ref AABB2D aabb, int idx){
            ShowShape(ref aabb, idx, DrawProperty);
        }

        void ShowShape(ref OBB2D obb, int idx){
            ShowShape(ref obb, idx, DrawProperty);
        }

        public static void DrawProperty(ref Circle shape){
            shape.radius = EditorGUILayoutExt.FloatField("Radius", shape.radius);
        }

        public static void DrawProperty(ref AABB2D shape){
            shape.size = EditorGUILayoutExt.Vector2Field("Size", shape.size);
        }

        public static void DrawProperty(ref OBB2D shape){
            shape.size = EditorGUILayoutExt.Vector2Field("Size", shape.size);
            var deg = EditorGUILayoutExt.FloatField("Deg", shape.deg);
            if (deg != shape.deg) {
                shape.SetDeg(deg);
            }
        }

        delegate void FuncDrawProperty<T>(ref T shape) where T : IShape2D;

        void ShowShape<T>(ref T circle, int idx, FuncDrawProperty<T> _Func) where T : IShape2D{
            GUILayout.BeginVertical();
            var offset = EditorGUILayoutExt.Vector2Field("Offset", owner.allOffsets[idx]);
            owner.allOffsets[idx] = offset;
            circle.UpdatePosition(owner.pos + offset);
            {
                GUILayout.BeginHorizontal();
                _Func(ref circle);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}