using Lockstep.Math;
using UnityEngine;

namespace Lockstep.UnsafeCollision2D {
    public class GizmosHelper {
        public static unsafe void DrawGizmos(ShapeWrap shapeWrap, Color color){
            var shapeType = (EShape2D) shapeWrap.TypeId;
            switch (shapeType) {
                case EShape2D.Circle:
                    DrawCircle(((ShapeWrapCircle) shapeWrap).shape, color);
                    break;
                case EShape2D.AABB:
                    DrawAABB(((ShapeWrapAABB) shapeWrap).shape, color);
                    break;
                case EShape2D.OBB:
                    DrawOBB(((ShapeWrapOBB) shapeWrap).shape, color);
                    break;
            }
        }

        public static unsafe void DrawGizmos(EShape2D shapeType, Circle* shape, Color color){
            switch (shapeType) {
                case EShape2D.Circle:
                    DrawCircle(*(Circle*) shape, color);
                    break;
                case EShape2D.AABB:
                    DrawAABB(*(AABB2D*) shape, color);
                    break;
                case EShape2D.OBB:
                    DrawOBB(*(OBB2D*) shape, color);
                    break;
            }
        }

        public static void DrawCircle(Circle circle, Color color){
            DebugExtension.DebugCircle(circle.pos.ToVector2(), Vector3.back, color, circle.radius.ToFloat());
        }

        public static void DrawAABB(AABB2D aabb, Color color){
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

        public static void DrawOBB(OBB2D obb, Color color){
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
    }
}