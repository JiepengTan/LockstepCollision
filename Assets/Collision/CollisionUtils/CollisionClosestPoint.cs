using Lockstep.Math;
using Lockstep.Math;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
{
    /// <summary>
    /// ClosestPtXxx 获取最近点
    /// DistXxx  获取最近距离
    /// SqDistXxx 最近距离平方
    /// </summary>
    public static partial class Utils
    {
        //TODO 如果Plane 发现最后决定需要进行 归一化 则可以省掉 后面的 Dot 计算
        public static LVector3 ClosestPtPointPlane(LVector3 q, Plane p)
        {
            LFloat t = (Dot(p.n, q) - p.d) / Dot(p.n, p.n);
            return q - t * p.n;
        }

        //TODO n 归一化问题
        public static LFloat DistPointPlane(LVector3 q, Plane p)
        {
            // return Dot(q, p.n) - p.d; if plane equation normalized (||p.n||==1)
            return (Dot(p.n, q) - p.d) / Dot(p.n, p.n);
        }

        // Given segment ab and point c, computes closest point d on ab.
        // Also returns t for the position of d, d(t) = a + t*(b - a)
        public static void ClosestPtPointSegment(LVector3 c, LVector3 a, LVector3 b, out LFloat t, out LVector3 d)
        {
            LVector3 ab = b - a;
            // Project c onto ab, computing parameterized position d(t) = a + t*(b - a)
            t = Dot(c - a, ab) / Dot(ab, ab);
            // If outside segment, clamp t (and therefore d) to the closest endpoint
            if (t < LFloat.zero) t = LFloat.zero;
            if (t > LFloat.one) t = LFloat.one;
            // Compute projected position from the clamped t
            d = a + t * ab;
        }

        /// <summary> Returns the squared distance between point c and segment ab </summary>
        public static LFloat SqDistPointSegment(LVector3 a, LVector3 b, LVector3 c)
        {
            LVector3 ab = b - a, ac = c - a, bc = c - b;
            LFloat e = Dot(ac, ab);
            // Handle cases where c projects outside ab
            if (e <= LFloat.zero) return Dot(ac, ac);
            LFloat f = Dot(ab, ab);
            if (e >= f) return Dot(bc, bc);
            // Handle case where c projects onto ab
            return Dot(ac, ac) - e * e / f;
        }

        /// <summary> 
        /// Computes closest points C1 and C2 of S1(s)=P1+s*(Q1-P1) and
        /// S2(t)=P2+t*(Q2-P2), returning s and t. Function result is squared
        /// distance between between S1(s) and S2(t)
        /// </summary>
        public static LFloat ClosestPtSegmentSegment(LVector3 p1, LVector3 q1, LVector3 p2, LVector3 q2,
            out LFloat s, out LFloat t, out LVector3 c1, out LVector3 c2)
        {
            LVector3 d1 = q1 - p1; // Direction vector of segment S1
            LVector3 d2 = q2 - p2; // Direction vector of segment S2
            LVector3 r = p1 - p2;
            LFloat a = Dot(d1, d1); // Squared length of segment S1, always nonnegative
            LFloat e = Dot(d2, d2); // Squared length of segment S2, always nonnegative
            LFloat f = Dot(d2, r);

            // Check if either or both segments degenerate into points
            if (a <= LFloat.EPSILON && e <= LFloat.EPSILON)
            {
                // Both segments degenerate into points
                s = t = LFloat.zero;
                c1 = p1;
                c2 = p2;
                return Dot(c1 - c2, c1 - c2);
            }

            if (a <= LFloat.EPSILON)
            {
                // First segment degenerates into a point
                s = LFloat.zero;
                t = f / e; // s = 0 => t = (b*s + f) / e = f / e
                t = Clamp(t, LFloat.zero, LFloat.one);
            }
            else
            {
                LFloat c = Dot(d1, r);
                if (e <= LFloat.EPSILON)
                {
                    // Second segment degenerates into a point
                    t = LFloat.zero;
                    s = Clamp(-c / a, LFloat.zero, LFloat.one); // t = 0 => s = (b*t - c) / a = -c / a
                }
                else
                {
                    // The general nondegenerate case starts here
                    LFloat b = Dot(d1, d2);
                    LFloat denom = a * e - b * b; // Always nonnegative

                    // If segments not parallel, compute closest point on L1 to L2, and
                    // clamp to segment S1. Else pick arbitrary s (here 0)
                    if (denom != LFloat.zero)
                    {
                        s = Clamp((b * f - c * e) / denom, LFloat.zero, LFloat.one);
                    }
                    else s = LFloat.zero;

                    // Compute point on L2 closest to S1(s) using
                    // t = Dot((P1+D1*s)-P2,D2) / Dot(D2,D2) = (b*s + f) / e
                    t = (b * s + f) / e;

                    // If t in [0,1] done. Else clamp t, recompute s for the new value
                    // of t using s = Dot((P2+D2*t)-P1,D1) / Dot(D1,D1)= (t*b - c) / a
                    // and clamp s to [0, 1]
                    if (t < LFloat.zero)
                    {
                        t = LFloat.zero;
                        s = Clamp(-c / a, LFloat.zero, LFloat.one);
                    }
                    else if (t > LFloat.one)
                    {
                        t = LFloat.one;
                        s = Clamp((b - c) / a, LFloat.zero, LFloat.one);
                    }
                }
            }

            c1 = p1 + d1 * s;
            c2 = p2 + d2 * t;
            return Dot(c1 - c2, c1 - c2);
        }


        // Given point p, return the point q on or in AABB b, that is closest to p
        public static void ClosestPtPointAABB(LVector3 p, AABB b, out LVector3 q)
        {
            q = p;
            // For each coordinate axis, if the point coordinate value is
            // outside box, clamp it to the box, else keep it as is
            for (int i = 0; i < 3; i++)
            {
                LFloat v = p[i];
                if (v < b.min[i]) v = b.min[i]; // v = max(v, b.min[i])
                if (v > b.max[i]) v = b.max[i]; // v = min(v, b.max[i])
                q[i] = v;
            }
        }

        // Computes the square distance between a point p and an AABB b
        public static LFloat SqDistPointAABB(LVector3 p, AABB b)
        {
            LFloat sqDist = LFloat.zero;
            for (int i = 0; i < 3; i++)
            {
                // For each axis count any excess distance outside box extents
                LFloat v = p[i];
                if (v < b.min[i]) sqDist += (b.min[i] - v) * (b.min[i] - v);
                if (v > b.max[i]) sqDist += (v - b.max[i]) * (v - b.max[i]);
            }

            return sqDist;
        }

        // Given point p, return point q on (or in) OBB b, closest to p
        public static void ClosestPtPointOBB(LVector3 p, OBB b, out LVector3 q)
        {
            LVector3 d = p - b.c;
            // Start result at center of box; make steps from there
            q = b.c;
            // For each OBB axis...
            for (int i = 0; i < 3; i++)
            {
                // ...project d onto that axis to get the distance
                // along the axis of d from the box center
                LFloat dist = Dot(d, b.u[i]);
                // If distance farther than the box extents, clamp to the box
                if (dist > b.e[i]) dist = b.e[i];
                if (dist < -b.e[i]) dist = -b.e[i];
                // Step that distance along the axis to get world coordinate
                q += dist * b.u[i];
            }
        }


        // Computes the square distance between point p and OBB b
        public static LFloat SqDistPointOBB(LVector3 p, OBB b)
        {
            LVector3 v = p - b.c;
            LFloat sqDist = LFloat.zero;
            for (int i = 0; i < 3; i++)
            {
                // Project vector from box center to p on each axis, getting the distance
                // of p along that axis, and count any excess distance outside box extents
                LFloat d = Dot(v, b.u[i]), excess = LFloat.zero;
                if (d < (-b.e[i]))
                    excess = d + b.e[i];
                else if (d > b.e[i])
                    excess = d - b.e[i];
                sqDist += excess * excess;
            }

            return sqDist;
        }


        // Given point p, return point q on (or in) Rect r, closest to p
        public static void ClosestPtPointRect(LVector3 p, Rect r, out LVector3 q)
        {
            LVector3 d = p - r.c;
            // Start result at center of rect; make steps from there
            q = r.c;
            // For each rect axis...
            for (int i = 0; i < 2; i++)
            {
                // ...project d onto that axis to get the distance
                // along the axis of d from the rect center
                LFloat dist = Dot(d, r.u[i]);
                // If distance farther than the rect extents, clamp to the rect
                if (dist > r.e[i]) dist = r.e[i];
                if (dist < -r.e[i]) dist = -r.e[i];
                // Step that distance along the axis to get world coordinate
                q += dist * r.u[i];
            }
        }

        //通过划分Voronoi 区域  来进行判定 最近点 拉格朗日恒等式展开
        //Dot(Cross(a,b),Cross(c,d)) = Dot(a,c)*Dot(b,d) - Dot(a,d)*Dot(b,c)
        public static LVector3 ClosestPtPointTriangle(LVector3 p, LVector3 a, LVector3 b, LVector3 c)
        {
            // Check if P in vertex region outside A
            LVector3 ab = b - a;
            LVector3 ac = c - a;
            LVector3 ap = p - a;
            LFloat d1 = Dot(ab, ap);
            LFloat d2 = Dot(ac, ap);
            if (d1 <= LFloat.zero && d2 <= LFloat.zero) return a; // barycentric coordinates (1,0,0)

            // Check if P in vertex region outside B
            LVector3 bp = p - b;
            LFloat d3 = Dot(ab, bp);
            LFloat d4 = Dot(ac, bp);
            if (d3 >= LFloat.zero && d4 <= d3) return b; // barycentric coordinates (0,1,0)

            // Check if P in edge region of AB, if so return projection of P onto AB
            LFloat vc = d1 * d4 - d3 * d2;
            if (vc <= LFloat.zero && d1 >= LFloat.zero && d3 <= LFloat.zero)
            {
                LFloat _v = d1 / (d1 - d3);
                return a + _v * ab; // barycentric coordinates (1-v,v,0)
            }

            // Check if P in vertex region outside C
            LVector3 cp = p - c;
            LFloat d5 = Dot(ab, cp);
            LFloat d6 = Dot(ac, cp);
            if (d6 >= LFloat.zero && d5 <= d6) return c; // barycentric coordinates (0,0,1)

            // Check if P in edge region of AC, if so return projection of P onto AC
            LFloat vb = d5 * d2 - d1 * d6;
            if (vb <= LFloat.zero && d2 >= LFloat.zero && d6 <= LFloat.zero)
            {
                LFloat _w = d2 / (d2 - d6);
                return a + _w * ac; // barycentric coordinates (1-w,0,w)
            }

            // Check if P in edge region of BC, if so return projection of P onto BC
            LFloat va = d3 * d6 - d5 * d4;
            if (va <= LFloat.zero && (d4 - d3) >= LFloat.zero && (d5 - d6) >= LFloat.zero)
            {
                LFloat _w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return b + _w * (c - b); // barycentric coordinates (0,1-w,w)
            }

            // P inside face region. Compute Q through its barycentric coordinates (u,v,w)
            LFloat denom = LFloat.one / (va + vb + vc);
            LFloat v = vb * denom;
            LFloat w = vc * denom;
            return a + ab * v + ac * w; // = u*a + v*b + w*c, u = va * denom = LFloat.one - v - w
        }

        /*
        public static Point ClosestPtPointTriangle(Point p, Point a, Point b, Point c)
        {
            LVector ab = b - a;
            LVector ac = c - a;
            LVector bc = c - b;

            // Compute parametric position s for projection P' of P on AB,
            // P' = A + s*AB, s = snom/(snom+sdenom)
            LFloat snom = Dot(p - a, ab), sdenom = Dot(p - b, a - b);
            // Compute parametric position t for projection P' of P on AC,
            // P' = A + t*AC, s = tnom/(tnom+tdenom)
            LFloat tnom = Dot(p - a, ac), tdenom = Dot(p - c, a - c);

            if (snom <= LFloat.zero && tnom <= LFloat.zero) return a; // Vertex region early out

            // Compute parametric position u for projection P' of P on BC,
            // P' = B + u*BC, u = unom/(unom+udenom)
            LFloat unom = Dot(p - b, bc), udenom = Dot(p - c, b - c);

            if (sdenom <= LFloat.zero && unom <= LFloat.zero) return b; // Vertex region early out
            if (tdenom <= LFloat.zero && udenom <= LFloat.zero) return c; // Vertex region early out


            // P is outside (or on) AB if the triple scalar product [N PA PB] <= 0
            LVector n = Cross(b - a, c - a);
            LFloat vc = Dot(n, Cross(a - p, b - p));
            // If P outside AB and within feature region of AB,
            // return projection of P onto AB
            if (vc <= LFloat.zero && snom >= LFloat.zero && sdenom >= LFloat.zero)
                return a + snom / (snom + sdenom) * ab;

            // P is outside (or on) BC if the triple scalar product [N PB PC] <= 0
            LFloat va = Dot(n, Cross(b - p, c - p));
            // If P outside BC and within feature region of BC,
            // return projection of P onto BC
            if (va <= LFloat.zero && unom >= LFloat.zero && udenom >= LFloat.zero)
                return b + unom / (unom + udenom) * bc;

            // P is outside (or on) CA if the triple scalar product [N PC PA] <= 0
            LFloat vb = Dot(n, Cross(c - p, a - p));
            // If P outside CA and within feature region of CA,
            // return projection of P onto CA
            if (vb <= LFloat.zero && tnom >= LFloat.zero && tdenom >= LFloat.zero)
                return a + tnom / (tnom + tdenom) * ac;

            // P must project inside face region. Compute Q using barycentric coordinates
            LFloat u = va / (va + vb + vc);
            LFloat v = vb / (va + vb + vc);
            LFloat w = LFloat.one - u - v; // = vc / (va + vb + vc)
            return u * a + v * b + w * c;
        }
         */
        // Test if point p lies outside plane through abc
        public static bool PointOutsideOfPlane(LVector3 p, LVector3 a, LVector3 b, LVector3 c)
        {
            return Dot(p - a, Cross(b - a, c - a)) >= LFloat.zero; // [AP AB AC] >= 0
        }

        public static LVector3 ClosestPtPointTetrahedron(LVector3 p, LVector3 a, LVector3 b, LVector3 c, LVector3 d)
        {
            // Start out assuming point inside all halfspaces, so closest to itself
            LVector3 closestPt = p;
            LFloat bestSqDist = LFloat.FLT_MAX;
            // If point outside face abc then compute closest point on abc
            if (PointOutsideOfPlane(p, a, b, c))
            {
                LVector3 q = ClosestPtPointTriangle(p, a, b, c);
                LFloat sqDist = Dot(q - p, q - p);
                // Update best closest point if (squared) distance is less than current best
                if (sqDist < bestSqDist)
                {
                    bestSqDist = sqDist;
                    closestPt = q;
                }
            }

            // Repeat test for face acd
            if (PointOutsideOfPlane(p, a, c, d))
            {
                LVector3 q = ClosestPtPointTriangle(p, a, c, d);
                LFloat sqDist = Dot(q - p, q - p);
                if (sqDist < bestSqDist)
                {
                    bestSqDist = sqDist;
                    closestPt = q;
                }
            }

            // Repeat test for face adb
            if (PointOutsideOfPlane(p, a, d, b))
            {
                LVector3 q = ClosestPtPointTriangle(p, a, d, b);
                LFloat sqDist = Dot(q - p, q - p);
                if (sqDist < bestSqDist)
                {
                    bestSqDist = sqDist;
                    closestPt = q;
                }
            }

            // Repeat test for face bdc
            if (PointOutsideOfPlane(p, b, d, c))
            {
                LVector3 q = ClosestPtPointTriangle(p, b, d, c);
                LFloat sqDist = Dot(q - p, q - p);
                if (sqDist < bestSqDist)
                {
                    bestSqDist = sqDist;
                    closestPt = q;
                }
            }

            return closestPt;
        }

        public static LFloat Signed2DTriArea(LVector3 a, LVector3 b, LVector3 c)
        {
            return (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);
        }

    }
}