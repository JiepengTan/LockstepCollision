using Lockstep.Math;
using static Lockstep.Math.LMath;

namespace Lockstep.Collision {
    public static partial class Utils {
        // Compute barycentric coordinates (u, v, w) for 
        // point p with respect to triangle (a, b, c)
        // 使用克莱姆法则
        public static void Barycentric(LVector3 a, LVector3 b, LVector3 c, LVector3 p, out LFloat u, out LFloat v, out LFloat w){
            LVector3 v0 = b - a, v1 = c - a, v2 = p - a;
            LFloat d00 = Dot(v0, v0);
            LFloat d01 = Dot(v0, v1);
            LFloat d11 = Dot(v1, v1);
            LFloat d20 = Dot(v2, v0);
            LFloat d21 = Dot(v2, v1);
            LFloat denom = d00 * d11 - d01 * d01;
            v = (d11 * d20 - d01 * d21) / denom;
            w = (d00 * d21 - d01 * d20) / denom;
            u = LFloat.one - v - w;
        }

        //
        public static LFloat TriArea2D(LFloat x1, LFloat y1, LFloat x2, LFloat y2, LFloat x3, LFloat y3){
            return (x1 - x2) * (y2 - y3) - (x2 - x3) * (y1 - y2);
        }

        public static bool IsConvexQuad(LVector3 a, LVector3 b, LVector3 c, LVector3 d){
            // Quad is nonconvex if Dot(Cross(bd, ba), Cross(bd, bc)) >= 0
            LVector3 bda = Cross(d - b, a - b);
            LVector3 bdc = Cross(d - b, c - b);
            if (Dot(bda, bdc) >= LFloat.zero) return false;
            // Quad is now convex iff Dot(Cross(ac, ad), Cross(ac, ab)) < 0
            LVector3 acd = Cross(c - a, d - a);
            LVector3 acb = Cross(c - a, b - a);
            return Dot(acd, acb) < LFloat.zero;
        }

        // Return index i of point p[i] farthest from the edge ab, to the left of the edge
        public static int PointFarthestFromEdge(LVector2 a, LVector2 b, LVector2[] p, int n){
            // Create edge LVector and LVector (counterclockwise) perpendicular to it
            LVector2 e = b - a, eperp = new LVector2(-e.y, e.x);

            // Track index, ‘distance’ and ‘rightmostness’ of currently best point
            int bestIndex = -1;
            LFloat maxVal = LFloat.FLT_MIN, rightMostVal = LFloat.FLT_MIN;

            // Test all points to find the one farthest from edge ab on the left side
            for (int i = 1; i < n; i++) {
                LFloat d = Dot2D(p[i] - a, eperp); // d is proportional to distance along eperp
                LFloat r = Dot2D(p[i] - a, e); // r is proportional to distance along e
                if (d > maxVal || (d == maxVal && r > rightMostVal)) {
                    bestIndex = i;
                    maxVal = d;
                    rightMostVal = r;
                }
            }

            return bestIndex;
        }

        //TODO 校验 Scalar的正负性
        public static LFloat ScalarTriple(LVector3 a, LVector3 b, LVector3 c){
            return Dot(b, Cross(c, a));
        }

        // BBox Method Definitions
        public static AABB Union(AABB b, LVector3 p){
            AABB ret = new AABB();
            ret.min.x = Min(b.min.x, p.x);
            ret.min.y = Min(b.min.y, p.y);
            ret.min.z = Min(b.min.z, p.z);
            ret.max.x = Max(b.max.x, p.x);
            ret.max.y = Max(b.max.y, p.y);
            ret.max.z = Max(b.max.z, p.z);
            return ret;
        }


        public static AABB Union(AABB b, AABB b2){
            AABB ret = new AABB();
            ret.min.x = Min(b.min.x, b2.min.x);
            ret.min.y = Min(b.min.y, b2.min.y);
            ret.min.z = Min(b.min.z, b2.min.z);
            ret.max.x = Max(b.max.x, b2.max.x);
            ret.max.y = Max(b.max.y, b2.max.y);
            ret.max.z = Max(b.max.z, b2.max.z);
            return ret;
        }
    }


    #region 备选方案

/*

    public static partial class Collision
    {
        // Compute barycentric coordinates (u, v, w) for 
        // point p with respect to triangle (a, b, c)
        // 三角形面积 之比
        public static void Barycentric2(Point a, Point b, Point c, Point p, out LFloat u, out LFloat v, out LFloat w)
        {
            // Unnormalized triangle normal
            LVector m = Cross(b - a, c - a);
            // Nominators and one-over-denominator for u and v ratios
            LFloat nu, nv, ood;
            // Absolute components for determining projection plane
            LFloat x = Abs(m.x), y = Abs(m.y), z = Abs(m.z);

            // Compute areas in plane of largest projection
            if (x >= y && x >= z)
            {
                // x is largest, project to the yz plane
                nu = TriArea2D(p.y, p.z, b.y, b.z, c.y, c.z); // Area of PBC in yz plane
                nv = TriArea2D(p.y, p.z, c.y, c.z, a.y, a.z); // Area of PCA in yz plane
                ood = LFloat.one / m.x; // 1/(2*area of ABC in yz plane)
            }
            else if (y >= x && y >= z)
            {
                // y is largest, project to the xz plane
                nu = TriArea2D(p.x, p.z, b.x, b.z, c.x, c.z);
                nv = TriArea2D(p.x, p.z, c.x, c.z, a.x, a.z);
                ood = LFloat.one / -m.y;
            }
            else
            {
                // z is largest, project to the xy plane
                nu = TriArea2D(p.x, p.y, b.x, b.y, c.x, c.y);
                nv = TriArea2D(p.x, p.y, c.x, c.y, a.x, a.y);
                ood = LFloat.one / m.z;
            }

            u = nu * ood;
            v = nv * ood;
            w = LFloat.one - u - v;
        }

        /*
        /// <summary>
        /// 点是否在线段上  仅仅考虑点在直线上情况
        /// </summary>
        private static bool IsPointOnSegment(ref LVector2 segSrc, ref LVector2 segVec, LVector2 point)
        {
            long num = point._x - (long) segSrc._x;
            long num2 = point._y - (long) segSrc._y;
            return (long) segVec._x * num + (long) segVec._y * num2 >= 0L &&
                   num * num + num2 * num2 <= segVec.rawSqrMagnitude;
        }

        /// <summary>
        /// 判定两线段是否相交 并求交点
        /// https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect/565282#
        /// </summary>
        public static bool IntersectSegment(ref LVector2 seg1Src, ref LVector2 seg1Vec, ref LVector2 seg2Src,
            ref LVector2 seg2Vec, out LVector2 interPoint)
        {
            interPoint = LVector2.zero;
            long denom = (long) seg1Vec._x * seg2Vec._y - (long) seg2Vec._x * seg1Vec._y; //sacle 1000
            if (denom == 0L)
                return false; // Collinear
            bool denomPositive = denom > 0L;
            var s02_x = seg1Src._x - seg2Src._x;
            var s02_y = seg1Src._y - seg2Src._y;
            long s_numer = (long) seg1Vec._x * s02_y - (long) seg1Vec._y * s02_x; //scale 1000
            if ((s_numer < 0L) == denomPositive)
                return false; // No collision

            long t_numer = seg2Vec._x * s02_y - seg2Vec._y * s02_x; //scale 1000
            if ((t_numer < 0L) == denomPositive)
                return false; // No collision

            if (((s_numer > denom) == denomPositive) || ((t_numer > denom) == denomPositive))
                return false; // No collision
            // Collision detected
            var t = t_numer * 1000 / denom; //sacle 1000
            interPoint._x = (int) (seg1Src._x + ((long) ((t * seg1Vec._x)) / 1000));
            interPoint._y = (int) (seg1Src._y + ((long) ((t * seg1Vec._y)) / 1000));
            return true;
        }

        /// <summary>
        ///  判定点是否在多边形内
        /// https://stackoverflow.com/questions/217578/how-can-i-determine-whether-a-2d-point-is-within-a-polygon
        /// </summary>
        public static bool IsPointInPolygon(LVector2 p, LVector2[] polygon)
        {
            var minX = polygon[0]._x;
            var maxX = polygon[0]._x;
            var minY = polygon[0]._y;
            var maxY = polygon[0]._y;
            for (int i = 1; i < polygon.Length; i++)
            {
                LVector2 q = polygon[i];
                minX = LMath.Min(q._x, minX);
                maxX = LMath.Max(q._x, maxX);
                minY = LMath.Min(q._y, minY);
                maxY = LMath.Max(q._y, maxY);
            }

            if (p._x < minX || p._x > maxX || p._y < minY || p._y > maxY)
            {
                return false;
            }

            // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if ((polygon[i]._y > p._y) != (polygon[j]._y > p._y) &&
                    p._x < (polygon[j]._x - polygon[i]._x) * (p._y - polygon[i]._y) / (polygon[j]._y - polygon[i]._y) +
                    polygon[i]._x)
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
    */

    #endregion
}