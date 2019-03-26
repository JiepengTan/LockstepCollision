using LockStepMath;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;

namespace LockStepCollision
{
    /// <summary>
    /// Collision Query 
    /// </summary>
    public static partial class Collision
    {
        // Test if point p is contained in triangle (a, b, c)
        public static bool TestPointTriangle(Point p, Point a, Point b, Point c)
        {
            LFloat u, v, w;
            Barycentric(a, b, c, p, out u, out v, out w);
            return v >= LFloat.zero && w >= LFloat.zero && (v + w) <= LFloat.one;
        }

        public static bool TestAABBAABB(AABB a, AABB b)
        {
            if (Abs(a.c[0] - b.c[0]) > (a.r[0] + b.r[0])) return false;
            if (Abs(a.c[1] - b.c[1]) > (a.r[1] + b.r[1])) return false;
            if (Abs(a.c[2] - b.c[2]) > (a.r[2] + b.r[2])) return false;
            return true;
        }

        public static bool TestSphereSphere(Sphere a, Sphere b)
        {
            // Calculate squared distance between centers
            LVector d = a.c - b.c;
            LFloat dist2 = Dot(d, d);
            // Spheres intersect if squared distance is less than squared sum of radii
            LFloat radiusSum = a.r + b.r;
            return dist2 <= radiusSum * radiusSum;
        }

        public static int TestOBBOBB(ref OBB a, ref OBB b)
        {
            LFloat ra, rb;
            Matrix33 R = new Matrix33(), AbsR = new Matrix33();

            // Compute rotation matrix expressing b in a's coordinate frame
            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                R[i,j] = Dot(a.u[i], b.u[j]);

            // Compute translation LVector t
            LVector t = b.c - a.c;
            // Bring translation into a's coordinate frame
            t = new LVector(Dot(t, a.u[0]), Dot(t, a.u[1]), Dot(t, a.u[2]));

            // Compute common subexpressions. Add in an epsilon term to
            // counteract arithmetic errors when two edges are parallel and
            // their cross product is (near) null (see text for details)
            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                AbsR[i,j] = Abs(R[i,j]) + LFloat.EPSILON;

            // Test axes L = A0, L = A1, L = A2
            for (int i = 0; i < 3; i++)
            {
                ra = a.e[i];
                rb = b.e[0] * AbsR[i,0] + b.e[1] * AbsR[i,1] + b.e[2] * AbsR[i,2];
                if (Abs(t[i]) > ra + rb) return 0;
            }

            // Test axes L = B0, L = B1, L = B2
            for (int i = 0; i < 3; i++)
            {
                ra = a.e[0] * AbsR[0,i] + a.e[1] * AbsR[1,i] + a.e[2] * AbsR[2,i];
                rb = b.e[i];
                if (Abs(t[0] * R[0,i] + t[1] * R[1,i] + t[2] * R[2,i]) > ra + rb) return 0;
            }

            // Test axis L = A0 x B0
            ra = a.e[1] * AbsR[2,0] + a.e[2] * AbsR[1,0];
            rb = b.e[1] * AbsR[0,2] + b.e[2] * AbsR[0,1];
            if (Abs(t[2] * R[1,0] - t[1] * R[2,0]) > ra + rb) return 0;

            // Test axis L = A0 x B1
            ra = a.e[1] * AbsR[2,1] + a.e[2] * AbsR[1,1];
            rb = b.e[0] * AbsR[0,2] + b.e[2] * AbsR[0,0];
            if (Abs(t[2] * R[1,1] - t[1] * R[2,1]) > ra + rb) return 0;

            // Test axis L = A0 x B2
            ra = a.e[1] * AbsR[2,2] + a.e[2] * AbsR[1,2];
            rb = b.e[0] * AbsR[0,1] + b.e[1] * AbsR[0,0];
            if (Abs(t[2] * R[1,2] - t[1] * R[2,2]) > ra + rb) return 0;

            // Test axis L = A1 x B0
            ra = a.e[0] * AbsR[2,0] + a.e[2] * AbsR[0,0];
            rb = b.e[1] * AbsR[1,2] + b.e[2] * AbsR[1,1];
            if (Abs(t[0] * R[2,0] - t[2] * R[0,0]) > ra + rb) return 0;

            // Test axis L = A1 x B1
            ra = a.e[0] * AbsR[2,1] + a.e[2] * AbsR[0,1];
            rb = b.e[0] * AbsR[1,2] + b.e[2] * AbsR[1,0];
            if (Abs(t[0] * R[2,1] - t[2] * R[0,1]) > ra + rb) return 0;

            // Test axis L = A1 x B2
            ra = a.e[0] * AbsR[2,2] + a.e[2] * AbsR[0,2];
            rb = b.e[0] * AbsR[1,1] + b.e[1] * AbsR[1,0];
            if (Abs(t[0] * R[2,2] - t[2] * R[0,2]) > ra + rb) return 0;

            // Test axis L = A2 x B0
            ra = a.e[0] * AbsR[1,0] + a.e[1] * AbsR[0,0];
            rb = b.e[1] * AbsR[2,2] + b.e[2] * AbsR[2,1];
            if (Abs(t[1] * R[0,0] - t[0] * R[1,0]) > ra + rb) return 0;

            // Test axis L = A2 x B1
            ra = a.e[0] * AbsR[1,1] + a.e[1] * AbsR[0,1];
            rb = b.e[0] * AbsR[2,2] + b.e[2] * AbsR[2,0];
            if (Abs(t[1] * R[0,1] - t[0] * R[1,1]) > ra + rb) return 0;

            // Test axis L = A2 x B2
            ra = a.e[0] * AbsR[1,2] + a.e[1] * AbsR[0,2];
            rb = b.e[0] * AbsR[2,1] + b.e[1] * AbsR[2,0];
            if (Abs(t[1] * R[0,2] - t[0] * R[1,2]) > ra + rb) return 0;

            // Since no separating axis found, the OBBs must be intersecting
            return 1;
        }
    }
}