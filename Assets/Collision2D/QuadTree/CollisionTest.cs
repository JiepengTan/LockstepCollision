using Lockstep.Math;

namespace Lockstep.Collision2D {
    public class CollisionTest {
        private static LFloat[] _distances = new LFloat[6];

        public static bool TestAABBAABB(LVector2 posA, LFloat rA, LVector2 sizeA, LVector2 posB, LFloat rB,
            LVector2 sizeB){
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

        public static bool TestAABB(LVector3 min0, LVector3 max0, LVector3 min1, LVector3 max1,
            ref CollisionResult result,
            bool twod = true){
            _distances[0] = max1[0] - min0[0];
            _distances[1] = max0[0] - min1[0];
            _distances[2] = max1[2] - min0[2];
            _distances[3] = max0[2] - min1[2];

            var iter = 4;

            // test y if 3d
            if (!twod) {
                _distances[4] = max1[1] - min0[1];
                _distances[5] = max0[1] - min1[1];
                iter = 6;
            }

            for (int i = 0; i < iter; i++) {
                if (_distances[i] < LFloat.zero)
                    return false;

                if ((i == 0) || _distances[i] < result.Penetration) {
                    result.Penetration = _distances[i];
                    //result.Normal = _aabbNormals[i];
                }
            }

            return true;
        }
    }
}