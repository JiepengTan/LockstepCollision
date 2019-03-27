using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    public static partial class Collision
    {
        public static bool IntersectSegmentPlane(Point a, Point b, Plane p, out LFloat t, out Point q)
        {
            // Compute the t value for the directed line ab intersecting the plane
            LVector ab = b - a;
            t = (p.d - Dot(p.n, a)) / Dot(p.n, ab);

            // If t in [0..1] compute and return intersection point
            if (t >= LFloat.zero && t <= LFloat.one)
            {
                q = a + t * ab;
                return true;
            }

            q = a;
            // Else no intersection
            return false;
        }


        // Intersect segment ab against plane of triangle def. If intersecting,
        // return t value and position q of intersection
        public static bool IntersectSegmentPlane(Point a, Point b, Point d, Point e, Point f,
            out LFloat t, out Point q)
        {
            Plane p = new Plane();//TODO
            p.n = Cross(e - d, f - d);
            p.d = Dot(p.n, d);
            return IntersectSegmentPlane(a, b, p, out t, out q);
        }


        // Intersects ray r = p + td, |d| = 1, with sphere s and, if intersecting,
        // returns t value of intersection and intersection point q
        public static bool IntersectRaySphere(Point p, LVector d, Sphere s, out LFloat t, out Point q)
        {
            LVector m = p - s.c;
            LFloat b = Dot(m, d);
            LFloat c = Dot(m, m) - s.r * s.r;
            t = LFloat.zero;
            q = p;
            // Exit if r’s origin outside s (c > 0)and r pointing away from s (b > 0)
            if (c > LFloat.zero && b > LFloat.zero) return false;
            LFloat discr = b * b - c;
            // A negative discriminant corresponds to ray missing sphere
            if (discr < LFloat.zero) return false;
            // Ray now found to intersect sphere, compute smallest t value of intersection
            t = -b - Sqrt(discr);
            // If t is negative, ray started inside sphere so clamp t to zero
            if (t < LFloat.zero) t = LFloat.zero;
            q = p + t * d;
            return true;
        }


        // Intersect ray R(t) = p + t*d against AABB a. When intersecting,
        // return intersection distance tmin and point q of intersection
        public static bool IntersectRayAABB(Point p, LVector d, AABB a, out LFloat tmin, out Point q)
        {
            q = p;
            tmin = LFloat.zero; // set to -FLT_MAX to get first hit on line
            LFloat tmax = LFloat.FLT_MAX; // set to max distance ray can travel (for segment)

            // For all three slabs
            for (int i = 0; i < 3; i++)
            {
                if (Abs(d[i]) < LFloat.EPSILON)
                {
                    // Ray is parallel to slab. No hit if origin not within slab
                    if (p[i] < a.min[i] || p[i] > a.max[i]) return false;
                }
                else
                {
                    // Compute intersection t value of ray with near and far plane of slab
                    LFloat ood = LFloat.one / d[i];
                    LFloat t1 = (a.min[i] - p[i]) * ood;
                    LFloat t2 = (a.max[i] - p[i]) * ood;
                    // Make t1 be intersection with near plane, t2 with far plane
                    if (t1 > t2)
                    {
                        var temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }

                    ;
                    // Compute the intersection of slab intersections intervals
                    tmin = Max(tmin, t1);
                    tmax = Min(tmax, t2);
                    // Exit with no collision as soon as slab intersection becomes empty
                    if (tmin > tmax) return false;
                }
            }

            // Ray intersects all 3 slabs. Return point (q) and intersection t value (tmin) 
            q = p + d * tmin;
            return true;
        }

        // Given line pq and ccw triangle abc, return whether line pierces triangle. If
        // so, also return the barycentric coordinates (u,v,w) of the intersection point
        public static bool IntersectLineTriangle(Point p, Point q, Point a, Point b, Point c,
            out LFloat u, out LFloat v, out LFloat w)
        {
            LVector pq = q - p;
            LVector pa = a - p;
            LVector pb = b - p;
            LVector pc = c - p;

            u = LFloat.zero;
            v = LFloat.zero;
            w = LFloat.zero;

            // Test if pq is inside the edges bc, ca and ab. Done by testing
            // that the signed tetrahedral volumes, computed using scalar triple
            // products, are all positive
            u = ScalarTriple(pq, pc, pb);
            if (u < LFloat.zero) return false;
            v = ScalarTriple(pq, pa, pc);
            if (v < LFloat.zero) return false;
            w = ScalarTriple(pq, pb, pa);
            if (w < LFloat.zero) return false;

            // Compute the barycentric coordinates (u, v, w) determining the
            // intersection point r, r = u*a + v*b + w*c
            LFloat denom = LFloat.one / (u + v + w);
            u *= denom;
            v *= denom;
            w *= denom; // w = LFloat.one - u - v;
            return true;
        }

        // Given line pq and ccw quadrilateral abcd, return whether the line
        // pierces the triangle. If so, also return the point r of intersection
        public static bool IntersectLineQuad(Point p, Point q, Point a, Point b, Point c, Point d, out Point r)
        {
            LVector pq = q - p;
            LVector pa = a - p;
            LVector pb = b - p;
            LVector pc = c - p;

            r = p;
            // Determine which triangle to test against by testing against diagonal first
            LVector m = Cross(pc, pq);
            LFloat v = Dot(pa, m); // ScalarTriple(pq, pa, pc);
            if (v >= LFloat.zero)
            {
                // Test intersection against triangle abc
                LFloat u = -Dot(pb, m); // ScalarTriple(pq, pc, pb);
                if (u < LFloat.zero) return false;
                LFloat w = ScalarTriple(pq, pb, pa);
                if (w < LFloat.zero) return false;
                // Compute r, r = u*a + v*b + w*c, from barycentric coordinates (u, v, w)
                LFloat denom = LFloat.one / (u + v + w);
                u *= denom;
                v *= denom;
                w *= denom; // w = LFloat.one - u - v;
                r = u * a + v * b + w * c;
            }
            else
            {
                // Test intersection against triangle dac
                LVector pd = d - p;
                LFloat u = Dot(pd, m); // ScalarTriple(pq, pd, pc);
                if (u < LFloat.zero) return false;
                LFloat w = ScalarTriple(pq, pa, pd);
                if (w < LFloat.zero) return false;
                v = -v;
                // Compute r, r = u*a + v*d + w*c, from barycentric coordinates (u, v, w)
                LFloat denom = LFloat.one / (u + v + w);
                u *= denom;
                v *= denom;
                w *= denom; // w = LFloat.one - u - v;
                r = u * a + v * d + w * c;
            }

            return true;
        }

        // Given segment pq and triangle abc, returns whether segment intersects
        // triangle and if so, also returns the barycentric coordinates (u,v,w)
        // of the intersection point
        public static bool IntersectSegmentTriangle(Point p, Point q, Point a, Point b, Point c,
            out LFloat u, out LFloat v, out LFloat w, out LFloat t)
        {
            LVector ab = b - a;
            LVector ac = c - a;
            LVector qp = p - q;

            t = LFloat.zero;
            u = LFloat.zero;
            v = LFloat.zero;
            w = LFloat.zero;

            // Compute triangle normal. Can be precalculated or cached if
            // intersecting multiple segments against the same triangle
            LVector n = Cross(ab, ac);

            // Compute denominator d. If d <= 0, segment is parallel to or points
            // away from triangle, so exit early
            LFloat d = Dot(qp, n);
            if (d <= LFloat.zero) return false;

            // Compute intersection t value of pq with plane of triangle. A ray
            // intersects iff 0 <= t. Segment intersects iff 0 <= t <= 1. Delay
            // dividing by d until intersection has been found to pierce triangle
            LVector ap = p - a;
            t = Dot(ap, n);
            if (t < LFloat.zero) return false;
            if (t > d) return false; // For segment; exclude this code line for a ray test

            // Compute barycentric coordinate components and test if within bounds
            LVector e = Cross(qp, ap);
            v = Dot(ac, e);
            if (v < LFloat.zero || v > d) return false;
            w = -Dot(ab, e);
            if (w < LFloat.zero || v + w > d) return false;

            // Segment/ray intersects triangle. Perform delayed division and
            // compute the last barycentric coordinate component
            LFloat ood = LFloat.one / d;
            t *= ood;
            v *= ood;
            w *= ood;
            u = LFloat.one - v - w;
            return true;
        }


        // Intersect segment S(t)=sa+t(sb-sa), 0<=t<=1 against cylinder specified by p, q and r
        public static bool IntersectSegmentCylinder(Point sa, Point sb, Point p, Point q, LFloat r, out LFloat t)
        {
            LVector d = q - p, m = sa - p, n = sb - sa;
            LFloat md = Dot(m, d);
            LFloat nd = Dot(n, d);
            LFloat dd = Dot(d, d);

            t = LFloat.zero;
            // Test if segment fully outside either endcap of cylinder
            if (md < LFloat.zero && md + nd < LFloat.zero) return false; // Segment outside ‘p’ side of cylinder
            if (md > dd && md + nd > dd) return false; // Segment outside ‘q’ side of cylinder
            LFloat nn = Dot(n, n);
            LFloat mn = Dot(m, n);
            LFloat a = dd * nn - nd * nd;
            LFloat k = Dot(m, m) - r * r;
            LFloat c = dd * k - md * md;
            if (Abs(a) < LFloat.EPSILON)
            {
                // Segment runs parallel to cylinder axis
                if (c > LFloat.zero) return false; // ‘a’ and thus the segment lie outside cylinder
                // Now known that segment intersects cylinder; figure out how it intersects
                if (md < LFloat.zero) t = -mn / nn; // Intersect segment against ‘p’ endcap
                else if (md > dd) t = (nd - mn) / nn; // Intersect segment against ‘q’ endcap
                else t = LFloat.zero; // ‘a’ lies inside cylinder
                return true;
            }

            LFloat b = dd * mn - nd * md;
            LFloat discr = b * b - a * c;
            if (discr < LFloat.zero) return false; // No real roots; no intersection
            t = (-b - Sqrt(discr)) / a;
            if (t < LFloat.zero || t > LFloat.one) return false; // Intersection lies outside segment
            if (md + t * nd < LFloat.zero)
            {
                // Intersection outside cylinder on ‘p’ side
                if (nd <= LFloat.zero) return false; // Segment pointing away from endcap
                t = -md / nd;
                // Keep intersection if Dot(S(t) - p, S(t) - p) <= r^2
                return k + 2 * t * (mn + t * nn) <= LFloat.zero;
            }
            else if (md + t * nd > dd)
            {
                // Intersection outside cylinder on ‘q’ side
                if (nd >= LFloat.zero) return false; // Segment pointing away from endcap
                t = (dd - md) / nd;
                // Keep intersection if Dot(S(t) - q, S(t) - q) <= r^2
                return k + dd - 2 * md + t * (2 * (mn - nd) + t * nn) <= LFloat.zero;
            }

            // Segment intersects cylinder between the end-caps; t is correct
            return true;
        }

        // Intersect segment S(t)=A+t(B-A), 0<=t<=1 against convex polyhedron specified
        // by the n halfspaces defined by the planes p[]. On exit tfirst and tlast
        // define the intersection, if any
        public static bool IntersectSegmentPolyhedron(Point a, Point b, Plane[] p, int n, out LFloat tfirst,
            out LFloat tlast)
        {
            // Compute direction vector for the segment
            LVector d = b - a;
            // Set initial interval to being the whole segment. For a ray, tlast should be
            // set to +FLT_MAX. For a line, additionally tfirst should be set to -FLT_MAX
            tfirst = LFloat.zero;
            tlast = LFloat.one;
            // Intersect segment against each plane 
            for (int i = 0; i < n; i++)
            {
                LFloat denom = Dot(p[i].n, d);
                LFloat dist = p[i].d - Dot(p[i].n, a);
                // Test if segment runs parallel to the plane
                if (denom == LFloat.zero)
                {
                    // If so, return “no intersection” if segment lies outside plane
                    if (dist > LFloat.zero) return false;
                }
                else
                {
                    // Compute parameterized t value for intersection with current plane
                    LFloat t = dist / denom;
                    if (denom < LFloat.zero)
                    {
                        // When entering halfspace, update tfirst if t is larger
                        if (t > tfirst) tfirst = t;
                    }
                    else
                    {
                        // When exiting halfspace, update tlast if t is smaller
                        if (t < tlast) tlast = t;
                    }

                    // Exit with “no intersection” if intersection becomes empty
                    if (tfirst > tlast) return false;
                }
            }

            // A nonzero logical intersection, so the segment intersects the polyhedron
            return true;
        }
    }
}