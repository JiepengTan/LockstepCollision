using Lockstep.Math;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
{
    /// <summary>
    /// Collision Query 
    /// </summary>
    public static partial class Utils
    {
        //TODO 实现碰撞检测 派发
        public static bool TestShapeShape(BaseShape a, BaseShape b)
        {
            return false;
        }
        // Test if point p is contained in triangle (a, b, c)
        public static bool PointInTriangle(LVector3 p, LVector3 a, LVector3 b, LVector3 c)
        {
            // Translate point and triangle so that point lies at origin
            a -= p; b -= p; c -= p;
            LFloat ab = Dot(a, b);
            LFloat ac = Dot(a, c);
            LFloat bc = Dot(b, c);
            LFloat cc = Dot(c, c);
            // Make sure plane normals for pab and pbc point in the same direction
            if (bc * ac - cc * ab < LFloat.zero) return false;
            // Make sure plane normals for pab and pca point in the same direction
            LFloat bb = Dot(b, b);
            if (ab * bc - ac * bb < LFloat.zero) return false;
            // Otherwise P must be in (or on) the triangle
            return true;
        }

        // Test if 2D point P lies inside 2D triangle ABC
        public static bool  TestPointTriangle2D(Point2D p, Point2D a, Point2D b, Point2D c)
        {
            LFloat pab = Cross2D(p - a, b - a);
            LFloat pbc = Cross2D(p - b, c - b);
            // If P left of one of AB and BC and right of the other, not inside triangle
            if (!SameSign(pab, pbc)) return false;
            LFloat pca = Cross2D(p - c, a - c);
            // If P left of one of AB and CA and right of the other, not inside triangle
            if (!SameSign(pab, pca)) return false;
            // P left or right of all edges, so must be in (or on) the triangle
            return true;
        }
        public static bool TestAABBAABB(AABB a, AABB b)
        {
            // Exit with no intersection if separated along an axis
            if (a.max[0] < b.min[0] || a.min[0] > b.max[0]) return false;
            if (a.max[1] < b.min[1] || a.min[1] > b.max[1]) return false;
            if (a.max[2] < b.min[2] || a.min[2] > b.max[2]) return false;
            // Overlapping on all axes means AABBs are intersecting
            return true;
        }

        public static bool TestSphereSphere(Sphere a, Sphere b)
        {
            // Calculate squared distance between centers
            LVector3 d = a.c - b.c;
            LFloat dist2 = Dot(d, d);
            // Spheres intersect if squared distance is less than squared sum of radii
            LFloat radiusSum = a.r + b.r;
            return dist2 <= radiusSum * radiusSum;
        }
        
        
        public static  bool TestOBBCapsule(OBB obb, Capsule capsule)
        {
            //-1.确认两个包围球是否 碰撞 如果未曾碰撞 加速返回
            var cc = capsule.GetBoundSphereCenter();
            var rc = capsule.GetBoundsSphereRadius();
            var diffr = obb.e.magnitude + rc;
            if ((obb.c - cc).sqrMagnitude > diffr * diffr)
            {
                return false;
            }

            //0.检测端点距离
            var maxDist = capsule.r * capsule.r;
            var sqDistA = Utils.SqDistPointOBB(capsule.a, obb);
            if (sqDistA <= maxDist) {
                return true;
            }
            var sqDistB = Utils.SqDistPointOBB(capsule.b, obb);
            if (sqDistB <= maxDist) {
                return true;
            }
            
            //0.Capsule 转换到OBB坐标 空间转换为Capsule VS AABB 
            //1 将capsule 变成一个射线 
            var lrayd = obb.u.WorldToLocal(capsule.b - capsule.a);
            var lraydn = lrayd.normalized;
            var lcapa = obb.u.WorldToLocal( capsule.a);
            var cap2obb = (/*obb.lc == LVector3.zero*/- lcapa);
            var aabb = obb.ToAABB();
            //2.将射线沿着AABB 中心靠近capsule.r 距离 
            var orthDir = (cap2obb -  Dot(lraydn, cap2obb) * lraydn ).normalized;
            var finalRayC = lcapa + (orthDir * capsule.r);
            
            //3.碰撞转换为ray AABB碰撞 
            if (Utils.IntersectRayAABB(finalRayC, lrayd, aabb, out LFloat tmin, out LVector3 q))
            {
                //4.检测碰撞点时间 确认碰撞是否发生
                return tmin <= LFloat.one;
            }

            return false;
        }

        public static bool TestAABBCapsule(AABB aabb, Capsule capsule)
        {
            //-1.确认两个包围球是否 碰撞 如果未曾碰撞 加速返回
            var cc = capsule.GetBoundSphereCenter();
            var rc = capsule.GetBoundsSphereRadius();
            var diffr = aabb.r.magnitude + rc;
            var aabbc = aabb.c;
            if ((aabbc - cc).sqrMagnitude > diffr * diffr)
            {
                return false;
            }

            //0.检测端点距离
            var maxDist = capsule.r * capsule.r;
            var sqDistA = Utils.SqDistPointAABB(capsule.a, aabb);
            if (sqDistA <= maxDist) {
                return true;
            }
            var sqDistB = Utils.SqDistPointAABB(capsule.b, aabb);
            if (sqDistB <= maxDist) {
                return true;
            }
            
            //1.Capsule 转换到OBB坐标 空间转换为Capsule VS AABB 
            var lrayd = capsule.b - capsule.a;
            //2 将capsule 变成一个射线 
            var lraydn = lrayd.normalized;
            var cap2obb = (aabb.c - capsule.a);
            //3.将射线沿着AABB 中心靠近capsule.r 距离
            var proj = Dot(lraydn, cap2obb) * lraydn;
            var orthDir = (cap2obb -  proj ).normalized;
            var finalRayC = capsule.a + (orthDir * capsule.r);
            //4.碰撞转换为ray AABB碰撞 
            if (Utils.IntersectRayAABB(finalRayC, lrayd, aabb, out LFloat tmin, out LVector3 q))
            {
                //5.检测碰撞点时间 确认碰撞是否发生
                return tmin <= LFloat.one && tmin >=LFloat.zero;
            }

            return false;
        }


        public static bool TestOBBOBB(OBB a, OBB b)
        {
            LFloat ra, rb;
            LMatrix33 R = new LMatrix33(), AbsR = new LMatrix33();

            // Compute rotation matrix expressing b in a's coordinate frame
            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                R[i, j] = Dot(a.u[i], b.u[j]);

            // Compute translation LVector t
            LVector3 t = b.c - a.c;
            // Bring translation into a's coordinate frame
            t = new LVector3(Dot(t, a.u[0]), Dot(t, a.u[1]), Dot(t, a.u[2]));

            // Compute common subexpressions. Add in an epsilon term to
            // counteract arithmetic errors when two edges are parallel and
            // their cross product is (near) null (see text for details)
            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                AbsR[i, j] = Abs(R[i, j]) + LFloat.EPSILON;

            // Test axes L = A0, L = A1, L = A2
            for (int i = 0; i < 3; i++)
            {
                ra = a.e[i];
                rb = b.e[0] * AbsR[i, 0] + b.e[1] * AbsR[i, 1] + b.e[2] * AbsR[i, 2];
                if (Abs(t[i]) > ra + rb) return false;
            }

            // Test axes L = B0, L = B1, L = B2
            for (int i = 0; i < 3; i++)
            {
                ra = a.e[0] * AbsR[0, i] + a.e[1] * AbsR[1, i] + a.e[2] * AbsR[2, i];
                rb = b.e[i];
                if (Abs(t[0] * R[0, i] + t[1] * R[1, i] + t[2] * R[2, i]) > ra + rb) return false;
            }

            // Test axis L = A0 x B0
            ra = a.e[1] * AbsR[2, 0] + a.e[2] * AbsR[1, 0];
            rb = b.e[1] * AbsR[0, 2] + b.e[2] * AbsR[0, 1];
            if (Abs(t[2] * R[1, 0] - t[1] * R[2, 0]) > ra + rb) return false;

            // Test axis L = A0 x B1
            ra = a.e[1] * AbsR[2, 1] + a.e[2] * AbsR[1, 1];
            rb = b.e[0] * AbsR[0, 2] + b.e[2] * AbsR[0, 0];
            if (Abs(t[2] * R[1, 1] - t[1] * R[2, 1]) > ra + rb) return false;

            // Test axis L = A0 x B2
            ra = a.e[1] * AbsR[2, 2] + a.e[2] * AbsR[1, 2];
            rb = b.e[0] * AbsR[0, 1] + b.e[1] * AbsR[0, 0];
            if (Abs(t[2] * R[1, 2] - t[1] * R[2, 2]) > ra + rb) return false;

            // Test axis L = A1 x B0
            ra = a.e[0] * AbsR[2, 0] + a.e[2] * AbsR[0, 0];
            rb = b.e[1] * AbsR[1, 2] + b.e[2] * AbsR[1, 1];
            if (Abs(t[0] * R[2, 0] - t[2] * R[0, 0]) > ra + rb) return false;

            // Test axis L = A1 x B1
            ra = a.e[0] * AbsR[2, 1] + a.e[2] * AbsR[0, 1];
            rb = b.e[0] * AbsR[1, 2] + b.e[2] * AbsR[1, 0];
            if (Abs(t[0] * R[2, 1] - t[2] * R[0, 1]) > ra + rb) return false;

            // Test axis L = A1 x B2
            ra = a.e[0] * AbsR[2, 2] + a.e[2] * AbsR[0, 2];
            rb = b.e[0] * AbsR[1, 1] + b.e[1] * AbsR[1, 0];
            if (Abs(t[0] * R[2, 2] - t[2] * R[0, 2]) > ra + rb) return false;

            // Test axis L = A2 x B0
            ra = a.e[0] * AbsR[1, 0] + a.e[1] * AbsR[0, 0];
            rb = b.e[1] * AbsR[2, 2] + b.e[2] * AbsR[2, 1];
            if (Abs(t[1] * R[0, 0] - t[0] * R[1, 0]) > ra + rb) return false;

            // Test axis L = A2 x B1
            ra = a.e[0] * AbsR[1, 1] + a.e[1] * AbsR[0, 1];
            rb = b.e[0] * AbsR[2, 2] + b.e[2] * AbsR[2, 0];
            if (Abs(t[1] * R[0, 1] - t[0] * R[1, 1]) > ra + rb) return false;

            // Test axis L = A2 x B2
            ra = a.e[0] * AbsR[1, 2] + a.e[1] * AbsR[0, 2];
            rb = b.e[0] * AbsR[2, 1] + b.e[1] * AbsR[2, 0];
            if (Abs(t[1] * R[0, 2] - t[0] * R[1, 2]) > ra + rb) return false;

            // Since no separating axis found, the OBBs must be intersecting
            return true;
        }

        public static bool TestSphereCapsule(Sphere s, Capsule capsule)
        {
            // Compute (squared) distance between sphere center and capsule line segment
            LFloat dist2 = SqDistPointSegment(capsule.a, capsule.b, s.c);

            // If (squared) distance smaller than (squared) sum of radii, they collide
            LFloat radius = s.r + capsule.r;
            return dist2 <= radius * radius;
        }

        public static bool TestCapsuleCapsule(Capsule capsule1, Capsule capsule2)
        {
            // Compute (squared) distance between the inner structures of the capsules
            LFloat s, t;
            LVector3 c1, c2;
            LFloat dist2 = ClosestPtSegmentSegment(capsule1.a, capsule1.b,
                capsule2.a, capsule2.b, out s, out t, out c1, out c2);

            // If (squared) distance smaller than (squared) sum of radii, they collide
            LFloat radius = capsule1.r + capsule2.r;
            return dist2 <= radius * radius;
        }


        // Test if segments ab and cd overlap. If they do, compute and return
        // intersection t value along ab and intersection position p
        public static bool Test2DSegmentSegment(LVector3 a, LVector3 b, LVector3 c, LVector3 d, ref LFloat t, ref LVector3 p)
        {
            // Sign of areas correspond to which side of ab points c and d are
            LFloat a1 = Signed2DTriArea(a, b, d); // Compute winding of abd (+ or -)
            LFloat a2 = Signed2DTriArea(a, b, c); // To intersect, must have sign opposite of a1

            // If c and d are on different sides of ab, areas have different signs
            if (a1 * a2 < LFloat.zero)
            {
                // Compute signs for a and b with respect to segment cd
                LFloat a3 = Signed2DTriArea(c, d, a); // Compute winding of cda (+ or -)
                // Since area is constant a1-a2 = a3-a4, or a4=a3+a2-a1
                //LFloat a4 = Signed2DTriArea(c, d, b); // Must have opposite sign of a3
                LFloat a4 = a3 + a2 - a1;
                // Points a and b on different sides of cd if areas have different signs
                if (a3 * a4 < LFloat.zero)
                {
                    // Segments intersect. Find intersection point along L(t)=a+t*(b-a).
                    // Given height h1 of a over cd and height h2 of b over cd,
                    // t = h1 / (h1 - h2) = (b*h1/2) / (b*h1/2 - b*h2/2) = a3 / (a3 - a4),
                    // where b (the base of the triangles cda and cdb, i.e., the length
                    // of cd) cancels out.
                    t = a3 / (a3 - a4);
                    p = a + t * (b - a);
                    return true;
                }
            }

            // Segments not intersecting (or collinear)
            return false;
        }

        // Determine whether plane p intersects sphere s
        public static bool TestSpherePlane(Sphere s, Plane p)
        {
            // For a normalized plane (|p.n| = 1), evaluating the plane equation
            // for a point gives the signed distance of the point to the plane
            LFloat dist = Dot(s.c, p.n) - p.d;
            // If sphere center within +/-radius from plane, plane intersects sphere
            return Abs(dist) <= s.r;
        }

        // Test if OBB b intersects plane p
        public static bool TestOBBPlane(OBB b, Plane p)
        {
            // Compute the projection interval radius of b onto L(t) = b.c + t * p.n
            LFloat r = b.e[0] * Abs(Dot(p.n, b.u[0])) +
                       b.e[1] * Abs(Dot(p.n, b.u[1])) +
                       b.e[2] * Abs(Dot(p.n, b.u[2]));
            // Compute distance of box center from plane
            LFloat s = Dot(p.n, b.c) - p.d;
            // Intersection occurs when distance s falls within [-r,+r] interval
            return Abs(s) <= r;
        }
        //TODO TestCapsulePlane
        // Test if OBB b intersects plane p
        public static bool TestCapsulePlane(Capsule b, Plane p)
        {
            return false;
        }

        // Test if AABB b intersects plane p
        public static bool TestAABBPlane(AABB b, Plane p)
        {
            // These two lines not necessary with a (center, extents) AABB representation
            LVector3 c = (b.max + b.min) * LFloat.half; // Compute AABB center
            LVector3 e = b.max - c; // Compute positive extents
            // Compute the projection interval radius of b onto L(t) = b.c + t * p.n
            LFloat r = e[0] * Abs(p.n[0]) + e[1] * Abs(p.n[1]) + e[2] * Abs(p.n[2]);
            // Compute distance of box center from plane
            LFloat s = Dot(p.n, c) - p.d;
            // Intersection occurs when distance s falls within [-r,+r] interval
            return Abs(s) <= r;
        }

        // Returns true if sphere s intersects AABB b, false otherwise
        public static bool TestSphereAABB(Sphere s, AABB b)
        {
            // Compute squared distance between sphere center and AABB
            LFloat sqDist = SqDistPointAABB(s.c, b);

            // Sphere and AABB intersect if the (squared) distance
            // between them is less than the (squared) sphere radius
            return sqDist <= s.r * s.r;
        }


        // Returns true if sphere s intersects AABB b, false otherwise.
        // The point p on the AABB closest to the sphere center is also returned
        public static bool TestSphereAABB(Sphere s, AABB b, out LVector3 p)
        {
            // Find point p on AABB closest to sphere center
            ClosestPtPointAABB(s.c, b, out p);

            // Sphere and AABB intersect if the (squared) distance from sphere
            // center to point p is less than the (squared) sphere radius
            LVector3 v = p - s.c;
            return Dot(v, v) <= s.r * s.r;
        }

        // Returns true if sphere s intersects OBB b, false otherwise.
        // The point p on the OBB closest to the sphere center is also returned
        public static bool TestSphereOBB(Sphere s, OBB b, out LVector3 p)
        {
            // Find point p on OBB closest to sphere center
            ClosestPtPointOBB(s.c, b, out p);

            // Sphere and OBB intersect if the (squared) distance from sphere
            // center to point p is less than the (squared) sphere radius
            LVector3 v = p - s.c;
            return Dot(v, v) <= s.r * s.r;
        }


        // Returns true if sphere s intersects triangle ABC, false otherwise.
        // The point p on abc closest to the sphere center is also returned
        public static bool TestSphereTriangle(Sphere s, LVector3 a, LVector3 b, LVector3 c, out LVector3 p)
        {
            // Find point P on triangle ABC closest to sphere center
            p = ClosestPtPointTriangle(s.c, a, b, c);

            // Sphere and triangle intersect if the (squared) distance from sphere
            // center to point p is less than the (squared) sphere radius
            LVector3 v = p - s.c;
            return Dot(v, v) <= s.r * s.r;
        }
        // TODO
        /*
        // Test whether sphere s intersects polygon p
        public static bool TestSpherePolygon(Sphere s, Polygon p)
        {
            // Compute normal for the plane of the polygon
            LVector n = (Cross(p.v[1] - p.v[0], p.v[2] - p.v[0])).normalized;
            // Compute the plane equation for p
            Plane m;
            m.n = n;
            m.d = -Dot(n, p.v[0]);
            // No intersection if sphere not intersecting plane of polygon
            if (!TestSpherePlane(s, m)) return false;
            // Test to see if any one of the polygon edges pierces the sphere   
            for (int k = p.numVerts, i = 0, j = k - 1; i < k; j = i, i++)
            {
                LFloat t;
                Point q;
                // Test if edge (p.v[j], p.v[i]) intersects s
                if (IntersectRaySphere(p.v[j], p.v[i] - p.v[j], s, t, q) && t <= LFloat.one)
                    return 1;
            }

            // Test if the orthogonal projection q of the sphere center onto m is inside p
            Point q = ClosestPtPointPlane(s.c, m);
            return PointInPolygon(q, p);
        }
        */
        //TODO
        /*
        public static bool  TestTriangleAABB(Point v0, Point v1, Point v2, AABB b)
        {
            LFloat p0, p1, p2, r;

            // Compute box center and extents (if not already given in that format)
            LVector c = (b.min + b.max) * LFloat.half;
            LFloat e0 = (b.max.x - b.min.x) * LFloat.half;
            LFloat e1 = (b.max.y - b.min.y) * LFloat.half;
            LFloat e2 = (b.max.z - b.min.z) * LFloat.half;

            // Translate triangle as conceptually moving AABB to origin
            v0 = v0 - c;
            v1 = v1 - c;
            v2 = v2 - c;

            // Compute edge vectors for triangle
            LVector f0 = v1 - v0, f1 = v2 - v1, f2 = v0 - v2;

            // Test axes a00..a22 (category 3)
            // Test axis a00
            p0 = v0.z * v1.y - v0.y * v1.z;
            p2 = v2.z * (v1.y - v0.y) - v2.y * (v1.z - v0.z);
            r = e1 * Abs(f0.z) + e2 * Abs(f0.y);
            if (Max(-Max(p0, p2), Min(p0, p2)) > r)
                return false; // Axis is a separating axis

                    // Repeat similar tests for remaining axes a01..a22
                    ...

            // Test the three axes corresponding to the face normals of AABB b (category 1).
            // Exit if...
            // ... [-e0, e0] and [min(v0.x,v1.x,v2.x), max(v0.x,v1.x,v2.x)] do not overlap
            if (Max(v0.x, v1.x, v2.x) < -e0 || Min(v0.x, v1.x, v2.x) > e0) return false;
            // ... [-e1, e1] and [min(v0.y,v1.y,v2.y), max(v0.y,v1.y,v2.y)] do not overlap
            if (Max(v0.y, v1.y, v2.y) < -e1 || Min(v0.y, v1.y, v2.y) > e1) return false;
            // ... [-e2, e2] and [min(v0.z,v1.z,v2.z), max(v0.z,v1.z,v2.z)] do not overlap
            if (Max(v0.z, v1.z, v2.z) < -e2 || Min(v0.z, v1.z, v2.z) > e2) return false;

            // Test separating axis corresponding to triangle face normal (category 2)
            Plane p;
            p.n = Cross(f0, f1);
            p.d = Dot(p.n, v0);
            return TestAABBPlane(b, p);
        }
        */
    }
}