using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [CustomEditor(typeof(ColliderProxy2D))]
    public class EditorCollider2DMono : Editor {
        public ColliderProxy2D owner;

        private int removeIdx = 0;
        private int addTypeID = 0;

        public override void OnInspectorGUI(){
            base.OnInspectorGUI();
            owner = (ColliderProxy2D) target;
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
                    BaseShaper2D shape = null;
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
                    var shape = owner.allColliders[i];
                    var circle = shape as Circle;
                    if (circle != null) {
                        ShowCircle(circle, i);
                    }

                    var aabb = shape as AABB;
                    if (aabb != null) {
                        ShowAABB(aabb, i);
                    }

                    var obb = shape as OBB;
                    if (obb != null) {
                        ShowOBB(obb, i);
                    }
                }
            }
        }


        void ShowCircle(Circle circle, int idx){
            ShowShape(circle, idx, DrawProperty);
        }

        void ShowAABB(AABB aabb, int idx){
            ShowShape(aabb, idx, DrawProperty);
        }

        void ShowOBB(OBB obb, int idx){
            ShowShape(obb, idx, DrawProperty);
        }

        public static void DrawProperty(Circle shape){
            shape.radius = EditorGUILayoutExt.FloatField("Radius", shape.radius);
        }

        public static void DrawProperty(AABB shape){
            shape.size = EditorGUILayoutExt.Vector2Field("Size", shape.size);
        }

        public static void DrawProperty(OBB shape){
            shape.size = EditorGUILayoutExt.Vector2Field("Size", shape.size);
            var deg = EditorGUILayoutExt.FloatField("Deg", shape.deg);
            if (deg != shape.deg) {
                shape.SetDeg(deg);
            }
        }

        void ShowShape<T>(T circle, int idx, System.Action<T> _Func) where T : BaseShaper2D{
            GUILayout.BeginVertical();
            var offset = EditorGUILayoutExt.Vector2Field("Offset", owner.allOffsets[idx]);
            owner.allOffsets[idx] = offset;
            circle.UpdatePosition(owner.pos + offset);

            {
                GUILayout.BeginHorizontal();
                _Func(circle);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}