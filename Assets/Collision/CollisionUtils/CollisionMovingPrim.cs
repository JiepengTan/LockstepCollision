using Lockstep.Math;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision
{
    public static partial class Utils
    {
        // Intersect sphere s0 moving in direction d over time interval t0 <= t <= t1, against
        // a stationary sphere s1. If found intersecting, return time t of collision
        public static bool TestMovingSphereSphere(Sphere s0, LVector3 d, LFloat t0, LFloat t1, Sphere s1, out LFloat t)
        {
            // Compute sphere bounding motion of s0 during time interval from t0 to t1
            Sphere b = new Sphere();//TODO 移除掉new
            LFloat mid = (t0 + t1) * LFloat.half;
            b.c = s0.c + d * mid;
            b.r = (mid - t0) * d.magnitude + s0.r;
            t = LFloat.zero;
            // If bounding sphere not overlapping s1, then no collision in this interval
            if (!TestSphereSphere(b, s1)) return false;

            // Cannot rule collision out: recurse for more accurate testing. To terminate the
            // recursion, collision is assumed when time interval becomes sufficiently small
            if (t1 - t0 < LFloat.INTERVAL_EPSI_LON)
            {
                t = t0;
                return true;
            }

            // Recursively test first half of interval; return collision if detected
            if (TestMovingSphereSphere(s0, d, t0, mid, s1, out t)) return true;

            // Recursively test second half of interval
            return TestMovingSphereSphere(s0, d, mid, t1, s1,out t);
        }

        // Intersect sphere s with movement vector v with plane p. If intersecting
        // return time t of collision and point q at which sphere hits plane
        public static bool IntersectMovingSpherePlane(Sphere s, LVector3 v, Plane p, out LFloat t, out LVector3 q)
        {
            // Compute distance of sphere center to plane
            LFloat dist = Dot(p.n, s.c) - p.d;
            if (Abs(dist) <= s.r)
            {
                // The sphere is already overlapping the plane. Set time of
                // intersection to zero and q to sphere center
                t = LFloat.zero;
                q = s.c;
                return true;
            }
            else
            {
                LFloat denom = Dot(p.n, v);
                if (denom * dist >= LFloat.zero)
                {
                    // No intersection as sphere moving parallel to or away from plane
                    t = LFloat.zero;
                    q = s.c;
                    return false;
                }
                else
                {
                    // Sphere is moving towards the plane

                    // Use +r in computations if sphere in front of plane, else -r
                    LFloat r = dist > LFloat.zero ? s.r : -s.r;
                    t = (r - dist) / denom;
                    q = s.c + t * v - r * p.n;
                    return true;
                }
            }
        }


        // Test if sphere with radius r moving from a to b intersects with plane p
        public static bool TestMovingSpherePlane(LVector3 a, LVector3 b, LFloat r, Plane p)
        {
            // Get the distance for both a and b from plane p
            LFloat adist = Dot(a, p.n) - p.d;
            LFloat bdist = Dot(b, p.n) - p.d;
            // Intersects if on different sides of plane (distances have different signs)
            if (adist * bdist < LFloat.zero) return true;
            // Intersects if start or end position within radius from plane
            if (Abs(adist) <= r || Abs(bdist) <= r) return true;
            // No intersection
            return false;
        }

        public static bool TestMovingSphereSphere(Sphere s0, Sphere s1, LVector3 v0, LVector3 v1, out LFloat t)
        {
            LVector3 s = s1.c - s0.c; // LVector between sphere centers
            LVector3 v = v1 - v0; // Relative motion of s1 with respect to stationary s0
            LFloat r = s1.r + s0.r; // Sum of sphere radii
            LFloat c = Dot(s, s) - r * r;
            t = LFloat.zero;
            if (c < LFloat.zero)
            {
                // Spheres initially overlapping so exit directly
                t = LFloat.zero;
                return true;
            }

            LFloat a = Dot(v, v);
            if (a < LFloat.EPSILON) return false; // Spheres not moving relative each other
            LFloat b = Dot(v, s);
            if (b >= LFloat.zero) return false; // Spheres not moving towards each other
            LFloat d = b * b - a * c;
            if (d < LFloat.zero) return false; // No real-valued root, spheres do not intersect

            t = (-b - Sqrt(d)) / a;
            return true;
        }

        // Support function that returns the AABB vertex with index n
        private static LVector3 Corner(AABB b, int n)
        {
            LVector3 p;
            p._x = ((n & 1) != 0 ? b.max.x : b.min.x)._val;
            p._y = ((n & 2) != 0 ? b.max.y : b.min.y)._val;
            p._z = ((n & 4) != 0 ? b.max.z : b.min.z)._val;
            return p;
        }

        //TODO IntersectSegmentCapsule
        public static bool IntersectSegmentCapsule(Segment seg, LVector3 b, LVector3 e, LFloat r, out LFloat t)
        {
            t = LFloat.zero;
            return false;
        }        
        //TODO IntersectRayCapsule
        public static bool IntersectRayCapsule(LVector3 o,LVector3 d, LVector3 b, LVector3 e, LFloat r, out LFloat t)
        {
            t = LFloat.zero;
            return false;
        }

        public static bool IntersectMovingSphereAABB(Sphere s, LVector3 d, AABB b, out LFloat t)
        {
            // Compute the AABB resulting from expanding b by sphere radius r
            AABB e = b;
            int _val = s.r._val;
            e.min._x -= _val;
            e.min._y -= _val;
            e.min._z -= _val;
            e.max._x += _val;
            e.max._y += _val;
            e.max._z += _val;

            // Intersect ray against expanded AABB e. Exit with no intersection if ray
            // misses e, else get intersection point p and time t as result
            LVector3 p;
            if (!IntersectRayAABB(s.c, d, e, out t, out p) || t > LFloat.one)
                return false;

            // Compute which min and max faces of b the intersection point p lies
            // outside of. Note, u and v cannot have the same bits set and
            // they must have at least one bit set amongst them
            int u = 0, v = 0;
            if (p.x < b.min.x) u |= 1;
            if (p.x > b.max.x) v |= 1;
            if (p.y < b.min.y) u |= 2;
            if (p.y > b.max.y) v |= 2;
            if (p.z < b.min.z) u |= 4;
            if (p.z > b.max.z) v |= 4;

            // ‘Or’ all set bits together into a bit mask (note: here u + v == u | v)
            int m = u + v;

            // Define line segment [c, c+d] specified by the sphere movement
            Segment seg = new Segment(s.c, s.c + d);

            // If all 3 bits set (m == 7) then p is in a vertex region
            if (m == 7)
            {
                // Must now intersect segment [c, c+d] against the capsules of the three
                // edges meeting at the vertex and return the best time, if one or more hit
                LFloat tmin = LFloat.FLT_MAX;
                if (IntersectSegmentCapsule(seg, Corner(b, v), Corner(b, v ^ 1), s.r, out t))
                    tmin = Min(t, tmin);
                if (IntersectSegmentCapsule(seg, Corner(b, v), Corner(b, v ^ 2), s.r, out t))
                    tmin = Min(t, tmin);
                if (IntersectSegmentCapsule(seg, Corner(b, v), Corner(b, v ^ 4), s.r, out t))
                    tmin = Min(t, tmin);
                if (tmin == LFloat.FLT_MAX) return false; // No intersection
                t = tmin;
                return true; // Intersection at time t == tmin
            }

            // If only one bit set in m, then p is in a face region
            if ((m & (m - 1)) == 0)
            {
                // Do nothing. Time t from intersection with
                // expanded box is correct intersection time
                return true;
            }

            // p is in an edge region. Intersect against the capsule at the edge
            return IntersectSegmentCapsule(seg, Corner(b, u ^ 7), Corner(b, v), s.r, out t);
        }


        // Intersect AABBs ‘a’ and ‘b’ moving with constant velocities va and vb.
        // On intersection, return time of first and last contact in tfirst and tlast
        public static bool IntersectMovingAABBAABB(AABB a, AABB b, LVector3 va, LVector3 vb,
            out LFloat tfirst, out LFloat tlast)
        {
            // Exit early if ‘a’ and ‘b’ initially overlapping
            if (TestAABBAABB(a, b))
            {
                tfirst = tlast = LFloat.zero;
                return true;
            }

            // Use relative velocity; effectively treating 'a' as stationary
            LVector3 v = vb - va;

            // Initialize times of first and last contact
            tfirst = LFloat.zero;
            tlast = LFloat.one;

            // For each axis, determine times of first and last contact, if any
            for (int i = 0; i < 3; i++)
            {
                if (v[i] < LFloat.zero)
                {
                    if (b.max[i] < a.min[i]) return false; // Nonintersecting and moving apart
                    if (a.max[i] < b.min[i]) tfirst = Max((a.max[i] - b.min[i]) / v[i], tfirst);
                    if (b.max[i] > a.min[i]) tlast = Min((a.min[i] - b.max[i]) / v[i], tlast);
                }

                if (v
                        [i] > LFloat.zero)
                {
                    if (b.min[i] > a.max[i]) return false; // Nonintersecting and moving apart
                    if (b.max[i] < a.min[i]) tfirst = Max((a.min[i] - b.max[i]) / v[i], tfirst);
                    if (a.max[i] > b.min[i]) tlast = Min((a.max[i] - b.min[i]) / v[i], tlast);
                }

// No overlap possible if time of first contact occurs after time of last contact
                if (tfirst > tlast) return false;
            }

            return true;
        }
    }
}