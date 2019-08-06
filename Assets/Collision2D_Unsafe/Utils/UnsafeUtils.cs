using Lockstep.Math;
using Lockstep.UnsafeCollision2D;
using UnityEngine;
namespace Lockstep.Collision2D {

    public static unsafe partial class Utils {
        public static bool TestCollision(Circle* a, OBB2D* b){
            return TestCircleOBB(a->pos, a->radius
                , b->pos, b->radius, b->size, b->up
            );
        }

        public static bool TestCollision(AABB2D* a, OBB2D* b){
            return TestAABBOBB(a->pos, a->radius, a->size
                , b->pos, b->radius, b->size, b->up
            );
        }

        public static bool TestCollision(OBB2D* a, OBB2D* b){
            return TestOBBOBB(a->pos, a->radius, a->size, a->up
                , b->pos, b->radius, b->size, b->up
            );
        }

        //Circle* & Circle
        public static bool TestCollision(Circle* a, Circle* b){
            var diff = a->pos - b->pos;
            var allRadius = a->radius + b->radius;
            return diff.sqrMagnitude <= allRadius * allRadius;
        }

        //Circle* & AABB
        public static bool TestCollision(Circle* a, AABB2D* b){
            return TestCircleAABB(a->pos, a->radius, b->pos, b->radius, b->size);
        }

        //AABB & AABB
        public static bool TestCollision(AABB2D* a, AABB2D* b){
            return TestAABBAABB(a->pos, a->radius, a->size, b->pos, b->radius, b->size);
        }

        //AABB & Tile
        public static bool TestCollision(AABB2D* a, Vector2Int tilePos){
            return TestAABBAABB(
                a->pos, a->radius, a->size, //AABB1
                tilePos.ToLVector2() + LVector2.half, new LFloat(true, 707), LVector2.half //AABB2
            );
        }

        public static bool TestCollision(Circle* a, Vector2Int tilePos){
            return TestCircleAABB(
                a->pos, a->radius, //Circle1
                tilePos.ToLVector2() + LVector2.half, new LFloat(true, 707), LVector2.half //AABB2
            );
        }
    }
}