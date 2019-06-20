#if UNITY_5_3_OR_NEWER
using Lockstep.Math;
using UnityEngine;
#endif
namespace Lockstep.Collision2D {
    public class UnityColliderProxy2D :ColliderProxy2D  {
#if UNITY_EDITOR
        public Color DebugGizomColor = Color.white;
        private void OnDrawGizmos(){
            var count = allOffsets.Count;
            for (int i = 0; i < count; i++) {
                var shape = allColliders[i];
                var circle = shape as Circle;
                if (circle != null) {
                    DrawCircle(circle, DebugGizomColor);
                }

                var aabb = shape as AABB;
                if (aabb != null) {
                    DrawAABB(aabb, DebugGizomColor);
                }

                var obb = shape as OBB;
                if (obb != null) {
                    DrawOBB(obb, DebugGizomColor);
                }
            }
        }

        public static void DrawCircle(Circle circle, Color color){
            DebugExtension.DebugCircle(circle.pos.ToVector2(), Vector3.back, color, circle.radius.ToFloat());
        }

        public static void DrawAABB(AABB aabb, Color color){
            var right = aabb.size.x * LVector2.right;
            var up = aabb.size.y * LVector2.up;
            var lb = aabb.pos - up - right;
            var lt = aabb.pos + up - right;
            var rt = aabb.pos + up + right;
            var rb = aabb.pos - up + right;
            Debug.DrawLine(lb.ToVector2(), lt.ToVector2(), color);
            Debug.DrawLine(lt.ToVector2(), rt.ToVector2(), color);
            Debug.DrawLine(rt.ToVector2(), rb.ToVector2(), color);
            Debug.DrawLine(rb.ToVector2(), lb.ToVector2(), color);
        }

        public static void DrawOBB(OBB obb, Color color){
            var right = obb.size.x * obb.right;
            var up = obb.size.y * obb.up;
            var lb = obb.pos - up - right;
            var lt = obb.pos + up - right;
            var rt = obb.pos + up + right;
            var rb = obb.pos - up + right;
            Debug.DrawLine(lb.ToVector2(), lt.ToVector2(), color);
            Debug.DrawLine(lt.ToVector2(), rt.ToVector2(), color);
            Debug.DrawLine(rt.ToVector2(), rb.ToVector2(), color);
            Debug.DrawLine(rb.ToVector2(), lb.ToVector2(), color);
        }
#endif
    }
}