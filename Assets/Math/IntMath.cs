using System;
using UnityEngine;

public class IntMath
{
    public static VInt atan2(int y, int x)
    {
        int num;
        int num2;
        if (x < 0)
        {
            if (y < 0)
            {
                x = -x;
                y = -y;
                num = 1;
            }
            else
            {
                x = -x;
                num = -1;
            }
            num2 = -31416;
        }
        else
        {
            if (y < 0)
            {
                y = -y;
                num = -1;
            }
            else
            {
                num = 1;
            }
            num2 = 0;
        }
        int dIM = Atan2LookupTable.DIM;
        long num3 = (long)(dIM - 1);
        long b = (long)((x >= y) ? x : y);
        int num4 = (int)IntMath.Divide((long)x * num3, b);
        int num5 = (int)IntMath.Divide((long)y * num3, b);
        int num6 = Atan2LookupTable.table[num5 * dIM + num4];
        return new VInt((long)((num6 + num2) * num) / 10);
    }

    public static VInt acos(VInt val)
    {
        int num = (int)IntMath.Divide(val._val * (long)AcosLookupTable.HALF_COUNT, VInt.Precision) + AcosLookupTable.HALF_COUNT;
        num = Mathf.Clamp(num, 0, AcosLookupTable.COUNT);
        return new VInt((long)AcosLookupTable.table[num] / 10);
    }

    public static VInt sin(VInt radians)
    {
        int index = SinCosLookupTable.getIndex(radians);
        return new VInt((long)SinCosLookupTable.sin_table[index] / 10);
    }

    public static VInt cos(VInt radians)
    {
        int index = SinCosLookupTable.getIndex(radians);
        return new VInt((long)SinCosLookupTable.cos_table[index] / 10);
    }

    public static void sincos(out VInt s, out VInt c, VInt radians)
    {
        int index = SinCosLookupTable.getIndex(radians);
        s = new VInt((long)SinCosLookupTable.sin_table[index] / 10);
        c = new VInt((long)SinCosLookupTable.cos_table[index] / 10);
    }


    public static long Divide(long a, long b)
    {
        long num = (long)((ulong)((a ^ b) & -9223372036854775808L) >> 63);
        long num2 = num * -2L + 1L;
        return (a + b / 2L * num2) / b;
    }
    /// <summary>
    /// 4舍弃五入
    /// Divide(2503,1000) = 3
    /// Divide(2497,1000) = 2
    /// Divide(-2503,1000) = -3
    /// Divide(-2497,1000) = -2
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
	public static int Divide(int a, int b)
    {
        int num = (int)((uint)((a ^ b) & -2147483648) >> 31);
        int num2 = num * -2 + 1;
        return (a + b / 2 * num2) / b;
    }

    public static VInt3 Divide(VInt3 a, long m, long b)
    {
        a._x = (int)IntMath.Divide((long)a._x * m, b);
        a._y = (int)IntMath.Divide((long)a._y * m, b);
        a._z = (int)IntMath.Divide((long)a._z * m, b);
        return a;
    }

    public static VInt2 Divide(VInt2 a, long m, long b)
    {
        a._x = (int)IntMath.Divide((long)a._x * m, b);
        a._y = (int)IntMath.Divide((long)a._y * m, b);
        return a;
    }

    public static VInt3 Divide(VInt3 a, int b)
    {
        a._x = IntMath.Divide(a._x, b);
        a._y = IntMath.Divide(a._y, b);
        a._z = IntMath.Divide(a._z, b);
        return a;
    }

    public static VInt3 Divide(VInt3 a, long b)
    {
        a._x = (int)IntMath.Divide((long)a._x, b);
        a._y = (int)IntMath.Divide((long)a._y, b);
        a._z = (int)IntMath.Divide((long)a._z, b);
        return a;
    }

    public static VInt2 Divide(VInt2 a, long b)
    {
        a._x = (int)IntMath.Divide((long)a._x, b);
        a._y = (int)IntMath.Divide((long)a._y, b);
        return a;
    }

    public static uint Sqrt32(uint a)
    {
        uint num = 0u;
        uint num2 = 0u;
        for (int i = 0; i < 16; i++)
        {
            num2 <<= 1;
            num <<= 2;
            num += a >> 30;
            a <<= 2;
            if (num2 < num)
            {
                num2 += 1u;
                num -= num2;
                num2 += 1u;
            }
        }
        return num2 >> 1 & 65535u;
    }

    public static ulong Sqrt64(ulong a)
    {
        ulong num = 0uL;
        ulong num2 = 0uL;
        for (int i = 0; i < 32; i++)
        {
            num2 <<= 1;
            num <<= 2;
            num += a >> 62;
            a <<= 2;
            if (num2 < num)
            {
                num2 += 1uL;
                num -= num2;
                num2 += 1uL;
            }
        }
        return num2 >> 1 & 0xffffffffu;
    }
    public static int Sqrt(int a)
    {
        if (a <= 0)
        {
            return 0;
        }
        return (int)IntMath.Sqrt32((uint)a);
    }
    public static int Sqrt(long a)
    {
        if (a <= 0L)
        {
            return 0;
        }
        if (a <= (long)(0xffffffffu))
        {
            return (int)IntMath.Sqrt32((uint)a);
        }
        return (int)IntMath.Sqrt64((ulong)a);
    }

    public static long Clamp(long a, long min, long max)
    {
        if (a < min)
        {
            return min;
        }
        if (a > max)
        {
            return max;
        }
        return a;
    }

    public static long Max(long a, long b)
    {
        return (a <= b) ? b : a;
    }

    public static VInt3 Transform(ref VInt3 point, ref VInt3 axis_x, ref VInt3 axis_y, ref VInt3 axis_z, ref VInt3 trans)
    {
        return new VInt3(IntMath.Divide(axis_x._x * point._x + axis_y._x * point._y + axis_z._x * point._z, 1000) + trans._x, IntMath.Divide(axis_x._y * point._x + axis_y._y * point._y + axis_z._y * point._z, 1000) + trans._y, IntMath.Divide(axis_x._z * point._x + axis_y._z * point._y + axis_z._z * point._z, 1000) + trans._z);
    }

    public static VInt3 Transform(VInt3 point, ref VInt3 axis_x, ref VInt3 axis_y, ref VInt3 axis_z, ref VInt3 trans)
    {
        return new VInt3(IntMath.Divide(axis_x._x * point._x + axis_y._x * point._y + axis_z._x * point._z, 1000) + trans._x, IntMath.Divide(axis_x._y * point._x + axis_y._y * point._y + axis_z._y * point._z, 1000) + trans._y, IntMath.Divide(axis_x._z * point._x + axis_y._z * point._y + axis_z._z * point._z, 1000) + trans._z);
    }

    public static VInt3 Transform(ref VInt3 point, ref VInt3 axis_x, ref VInt3 axis_y, ref VInt3 axis_z, ref VInt3 trans, ref VInt3 scale)
    {
        long num = (long)point._x * (long)scale._x;
        long num2 = (long)point._y * (long)scale._x;
        long num3 = (long)point._z * (long)scale._x;
        return new VInt3((int)IntMath.Divide((long)axis_x._x * num + (long)axis_y._x * num2 + (long)axis_z._x * num3, 1000000L) + trans._x, (int)IntMath.Divide((long)axis_x._y * num + (long)axis_y._y * num2 + (long)axis_z._y * num3, 1000000L) + trans._y, (int)IntMath.Divide((long)axis_x._z * num + (long)axis_y._z * num2 + (long)axis_z._z * num3, 1000000L) + trans._z);
    }

    public static VInt3 Transform(ref VInt3 point, ref VInt3 forward, ref VInt3 trans)
    {
        VInt3 up = VInt3.up;
        VInt3 vInt = VInt3.Cross(VInt3.up, forward);
        return IntMath.Transform(ref point, ref vInt, ref up, ref forward, ref trans);
    }

    public static VInt3 Transform(VInt3 point, VInt3 forward, VInt3 trans)
    {
        VInt3 up = VInt3.up;
        VInt3 vInt = VInt3.Cross(VInt3.up, forward);
        return IntMath.Transform(ref point, ref vInt, ref up, ref forward, ref trans);
    }

    public static VInt3 Transform(VInt3 point, VInt3 forward, VInt3 trans, VInt3 scale)
    {
        VInt3 up = VInt3.up;
        VInt3 vInt = VInt3.Cross(VInt3.up, forward);
        return IntMath.Transform(ref point, ref vInt, ref up, ref forward, ref trans, ref scale);
    }

    public static int Lerp(int src, int dest, int nom, int den)
    {
        return IntMath.Divide(src * den + (dest - src) * nom, den);
    }

    public static long Lerp(long src, long dest, long nom, long den)
    {
        return IntMath.Divide(src * den + (dest - src) * nom, den);
    }

    public static bool IsPowerOfTwo(int x)
    {
        return (x & x - 1) == 0;
    }

    public static int CeilPowerOfTwo(int x)
    {
        x--;
        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;
        x++;
        return x;
    }

    /// <summary>
    /// 点是否在线段上  仅仅考虑点在直线上情况
    /// </summary>
    private static bool IsPointOnSegment(ref VInt2 segSrc, ref VInt2 segVec, VInt2 point)
    {
        long num = point._x - (long)segSrc._x;
        long num2 = point._y - (long)segSrc._y;
        return (long)segVec._x * num + (long)segVec._y * num2 >= 0L && num * num + num2 * num2 <= segVec.rawSqrMagnitude;
    }

    /// <summary>
    /// 判定两线段是否相交 并求交点
    /// https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect/565282#
    /// </summary>
    public static bool IntersectSegment(ref VInt2 seg1Src, ref VInt2 seg1Vec, ref VInt2 seg2Src, ref VInt2 seg2Vec, out VInt2 interPoint)
    {
        interPoint = VInt2.zero;
        long denom = (long)seg1Vec._x * seg2Vec._y - (long)seg2Vec._x * seg1Vec._y;//sacle 1000
        if (denom == 0L)
            return false; // Collinear
        bool denomPositive = denom > 0L;
        var s02_x = seg1Src._x - seg2Src._x;
        var s02_y = seg1Src._y - seg2Src._y;
        long s_numer = (long)seg1Vec._x * s02_y - (long)seg1Vec._y * s02_x;//scale 1000
        if ((s_numer < 0L) == denomPositive)
            return false; // No collision

        long t_numer = seg2Vec._x * s02_y - seg2Vec._y * s02_x; //scale 1000
        if ((t_numer < 0L) == denomPositive)
            return false; // No collision

        if (((s_numer > denom) == denomPositive) || ((t_numer > denom) == denomPositive))
            return false; // No collision
        // Collision detected
        var t = t_numer * 1000 / denom;//sacle 1000
        interPoint._x = (int)(seg1Src._x + ((long)((t * seg1Vec._x)) / 1000));
        interPoint._y = (int)(seg1Src._y + ((long)((t * seg1Vec._y)) / 1000));
        return true;
    }

    /// <summary>
    ///  判定点是否在多边形内
    /// https://stackoverflow.com/questions/217578/how-can-i-determine-whether-a-2d-point-is-within-a-polygon
    /// </summary>
    public static bool IsPointInPolygon(VInt2 p, VInt2[] polygon)
    {
        var minX = polygon[0]._x;
        var maxX = polygon[0]._x;
        var minY = polygon[0]._y;
        var maxY = polygon[0]._y;
        for (int i = 1; i < polygon.Length; i++)
        {
            VInt2 q = polygon[i];
            minX = Mathf.Min(q._x, minX);
            maxX = Mathf.Max(q._x, maxX);
            minY = Mathf.Min(q._y, minY);
            maxY = Mathf.Max(q._y, maxY);
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
                 p._x < (polygon[j]._x - polygon[i]._x) * (p._y - polygon[i]._y) / (polygon[j]._y - polygon[i]._y) + polygon[i]._x)
            {
                inside = !inside;
            }
        }

        return inside;
    }
}
