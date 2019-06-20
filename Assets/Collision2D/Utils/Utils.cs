using System;
using UnityEngine;
using System.Collections.Generic;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    public static class Utils {
        public static bool TestCollision(Circle a, OBB b){
            return TestCircleOBB(a.pos, a.radius
                , b.pos, b.radius, b.size, b.up
            );
        }

        public static bool TestCircleOBB(LVector2 posA, LFloat rA, LVector2 posB, LFloat rB, LVector2 sizeB, LVector2 up){
            var diff = posA - posB;
            var allRadius = rA + rB;
            //circle 判定CollisionHelper
            if (diff.sqrMagnitude > allRadius * allRadius) {
                return false;
            }

            //空间转换
            var absX = LMath.Abs(LVector2.Dot(diff, new LVector2(up.y, -up.x)));
            var absY = LMath.Abs(LVector2.Dot(diff, up));

            var size = sizeB;
            var radius = rA;

            var x = LMath.Max(absX - size.x, LFloat.zero);
            var y = LMath.Max(absY - size.y, LFloat.zero);
            return x * x + y * y < radius * radius;
        }

        public static bool TestCollision(AABB a, OBB b){
            return TestAABBOBB(a.pos, a.radius, a.size
                , b.pos, b.radius, b.size, b.up
            );
        }

        public static bool TestAABBOBB(LVector2 posA, LFloat rA, LVector2 sizeA, LVector2 posB, LFloat rB, LVector2 sizeB,
            LVector2 upB){
            var diff = posA - posB;
            var allRadius = rA + rB;
            //circle 判定
            if (diff.sqrMagnitude > allRadius * allRadius) {
                return false;
            }

            var absUPX = LMath.Abs(upB.x); //abs(up dot aabb.right)
            var absUPY = LMath.Abs(upB.y); //abs(right dot aabb.right)
            {
                //轴 投影 AABBx
                var distX = absUPX * sizeB.y + absUPY * sizeB.x;
                if (LMath.Abs(diff.x) > distX + sizeA.x) {
                    return false;
                }

                //轴 投影 AABBy
                //absUPX is abs(right dot aabb.up)
                //absUPY is abs(up dot aabb.up)
                var distY = absUPY * sizeB.y + absUPX * sizeB.x;
                if (LMath.Abs(diff.y) > distY + sizeA.y) {
                    return false;
                }
            }

            {
                var right = new LVector2(upB.y, -upB.x);
                var diffPObbX = LVector2.Dot(diff, right);
                var diffPObbY = LVector2.Dot(diff, upB);

                //absUPX is abs(aabb.up dot right )
                //absUPY is abs(aabb.right dot right)
                //轴 投影 OBBx
                var distX = absUPX * sizeA.y + absUPY * sizeA.x;
                if (LMath.Abs(diffPObbX) > distX + sizeB.x) {
                    return false;
                }

                //absUPX is abs(aabb.right dot up )
                //absUPY is abs(aabb.up dot up)
                //轴 投影 OBBy
                var distY = absUPY * sizeA.y + absUPX * sizeA.x;
                if (LMath.Abs(diffPObbY) > distY + sizeB.y) {
                    return false;
                }
            }

            return true;
        }


        public static bool TestCollision(OBB a, OBB b){
            return TestOBBOBB(a.pos, a.radius, a.size, a.up
                , b.pos, b.radius, b.size, b.up
            );
        }

        public static bool TestOBBOBB(LVector2 posA, LFloat rA, LVector2 sizeA, LVector2 upA, LVector2 posB, LFloat rB,
            LVector2 sizeB,
            LVector2 upB){
            var diff = posA - posB;
            var allRadius = rA + rB;
            //circle 判定
            if (diff.sqrMagnitude > allRadius * allRadius) {
                return false;
            }

            var rightA = new LVector2(upA.y, -upA.x);
            var rightB = new LVector2(upB.y, -upB.x);

            {
                //轴投影到 A.right
                var BuProjAr = LMath.Abs(LVector2.Dot(upB, rightA));
                var BrProjAr = LMath.Abs(LVector2.Dot(rightB, rightA));
                var DiffProjAr = LMath.Abs(LVector2.Dot(diff, rightA));
                var distX = BuProjAr * sizeB.y + BrProjAr * sizeB.x;
                if (DiffProjAr > distX + sizeA.x) {
                    return false;
                }

                //轴投影到 A.up
                var BuProjAu = LMath.Abs(LVector2.Dot(upB, upA));
                var BrProjAu = LMath.Abs(LVector2.Dot(rightB, upA));
                var DiffProjAu = LMath.Abs(LVector2.Dot(diff, upA));
                var distY = BuProjAu * sizeB.y + BrProjAu * sizeB.x;
                if (DiffProjAu > distY + sizeA.y) {
                    return false;
                }
            }

            {
                //轴投影到 B.right
                var AuProjBr = LMath.Abs(LVector2.Dot(upA, rightB));
                var ArProjBr = LMath.Abs(LVector2.Dot(rightA, rightB));
                var DiffProjBr = LMath.Abs(LVector2.Dot(diff, rightB));
                var distX = AuProjBr * sizeA.y + ArProjBr * sizeA.x;
                if (DiffProjBr > distX + sizeB.x) {
                    return false;
                }

                //轴投影到 B.right
                var AuProjBu = LMath.Abs(LVector2.Dot(upA, upB));
                var ArProjBu = LMath.Abs(LVector2.Dot(rightA, upB));
                var DiffProjBu = LMath.Abs(LVector2.Dot(diff, upB));
                var distY = AuProjBu * sizeA.y + ArProjBu * sizeA.x;
                if (DiffProjBu > distY + sizeB.x) {
                    return false;
                }
            }

            return true;
        }

        //Circle & Circle
        public static bool TestCollision(Circle a, Circle b){
            var diff = a.pos - b.pos;
            var allRadius = a.radius + b.radius;
            return diff.sqrMagnitude <= allRadius * allRadius;
        }

        public static bool TestCircleCircle(LVector2 posA, LFloat rA, LVector2 posB, LFloat rB){
            var diff = posA - posB;
            var allRadius = rA + rB;
            return diff.sqrMagnitude <= allRadius * allRadius;
        }

        //Circle & AABB
        public static bool TestCollision(Circle a, AABB b){
            return TestCircleAABB(a.pos, a.radius, b.pos, b.radius, b.size);
        }

        public static bool TestCircleAABB(LVector2 posA, LFloat rA, LVector2 posB, LFloat rB, LVector2 sizeB){
            var diff = posA - posB;
            var allRadius = rA + rB;
            //circle 判定
            if (diff.sqrMagnitude > allRadius * allRadius) {
                return false;
            }


            var absX = LMath.Abs(diff.x);
            var absY = LMath.Abs(diff.y);

            //AABB & circle
            var size = sizeB;
            var radius = rA;

            var x = LMath.Max(absX - size.x, LFloat.zero);
            var y = LMath.Max(absY - size.y, LFloat.zero);
            return x * x + y * y < radius * radius;
        }


        //AABB & AABB
        public static bool TestCollision(AABB a, AABB b){
            return TestAABBAABB(a.pos, a.radius, a.size, b.pos, b.radius, b.size);
        }

        public static bool TestAABBAABB(LVector2 posA, LFloat rA, LVector2 sizeA, LVector2 posB, LFloat rB, LVector2 sizeB){
            var diff = posA - posB;
            var allRadius = rA + rB;
            //circle 判定
            if (diff.sqrMagnitude > allRadius * allRadius) {
                return false;
            }

            var absX = LMath.Abs(diff.x);
            var absY = LMath.Abs(diff.y);

            //AABB and AABB
            var allSize = sizeA + sizeB;
            if (absX > allSize.x) return false;
            if (absY > allSize.y) return false;
            return true;
        }

        //AABB & Tile
        public static bool TestCollision(AABB a, Vector2Int tilePos){
            return TestAABBAABB(
                a.pos, a.radius, a.size, //AABB1
                tilePos.ToLVector2() + LVector2.half, new LFloat(true,707), LVector2.half //AABB2
            );
        }

        public static bool TestCollision(Circle a, Vector2Int tilePos){
            return TestCircleAABB(
                a.pos, a.radius, //Circle1
                tilePos.ToLVector2() + LVector2.half, new LFloat(true,707), LVector2.half //AABB2
            );
        }
    }
}