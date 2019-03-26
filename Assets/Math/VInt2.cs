using System;
using UnityEngine;

[Serializable]
public struct VInt2
{
    public VInt x { get { return new VInt(_x); } }
    public VInt y { get { return new VInt(_y); } }

    public int _x;
    public int _y;
    public static readonly VInt2 zero = new VInt2(0, 0);
    public static readonly VInt2 one = new VInt2(VInt.Precision, VInt.Precision);
    public static readonly VInt2 half = new VInt2(VInt.Precision / 2, VInt.Precision / 2);
    public static readonly VInt2 up = new VInt2(0, VInt.Precision);
    public static readonly VInt2 down = new VInt2(0, -VInt.Precision);
    public static readonly VInt2 right = new VInt2(VInt.Precision, 0);
    public static readonly VInt2 left = new VInt2(-VInt.Precision, 0);

    private static readonly int[] Rotations = new int[]
    {
        1,
        0,
        0,
        1,
        0,
        1,
        -1,
        0,
        -1,
        0,
        0,
        -1,
        0,
        -1,
        1,
        0
    };
    /// <summary>
    /// 顺时针旋转90Deg 参数
    /// </summary>
    public const int ROTATE_CW_90 = 1;
    public const int ROTATE_CW_180 = 2;
    public const int ROTATE_CW_270 = 3;
    public const int ROTATE_CW_360 = 4;

    public VInt2(VInt3 o)
    {
        this._x = o._x;
        this._y = o._y;
    }

    public VInt2(VInt x, VInt y)
    {
        this._x = x._val;
        this._y = y._val;
    }

    public VInt2(int x, int y)
    {
        this._x = x;
        this._y = y;
    }

    public static VInt Dot(VInt2 a, VInt2 b)
    {
        long num = (long)a._x;
        long num2 = (long)a._y;
        return new VInt((num * b._x + num2 * b._y) / VInt.Precision);
    }


    public static VInt Cross(ref VInt2 a, ref VInt2 b)
    {
        return new VInt(((long)a._x * (long)b._y - (long)a._y * (long)b._x) / VInt.Precision);
    }
    /// <summary>
    /// clockwise 顺时针旋转  
    /// 1表示顺时针旋转 90 degree
    /// 2表示顺时针旋转 180 degree
    /// </summary>
    public static VInt2 Rotate(VInt2 v, int r)
    {
        r %= 4;
        return new VInt2(
            v._x * VInt2.Rotations[r * 4] + v._y * VInt2.Rotations[r * 4 + 1],
            v._x * VInt2.Rotations[r * 4 + 2] + v._y * VInt2.Rotations[r * 4 + 3]);
    }

    public static VInt2 Min(VInt2 a, VInt2 b)
    {
        return new VInt2(Math.Min(a._x, b._x), Math.Min(a._y, b._y));
    }

    public static VInt2 Max(VInt2 a, VInt2 b)
    {
        return new VInt2(Math.Max(a._x, b._x), Math.Max(a._y, b._y));
    }

    public void Min(ref VInt2 r)
    {
        this._x = Mathf.Min(this._x, r._x);
        this._y = Mathf.Min(this._y, r._y);
    }

    public void Max(ref VInt2 r)
    {
        this._x = Mathf.Max(this._x, r._x);
        this._y = Mathf.Max(this._y, r._y);
    }


    public void Normalize()
    {
        long num = (long)(this._x * 100);
        long num2 = (long)(this._y * 100);
        long num3 = num * num + num2 * num2;
        if (num3 == 0L)
        {
            return;
        }
        long b = (long)IntMath.Sqrt(num3);
        this._x = (int)(num * 1000L / b);
        this._y = (int)(num2 * 1000L / b);
    }
    public VInt sqrMagnitude
    {
        get
        {
            long num = (long)this._x;
            long num2 = (long)this._y;
            return new VInt((num * num + num2 * num2) / VInt.Precision);
        }
    }
    public long rawSqrMagnitude
    {
        get
        {
            long num = (long)this._x;
            long num2 = (long)this._y;
            return num * num + num2 * num2;
        }
    }
    public VInt magnitude
    {
        get
        {
            long num = (long)this._x;
            long num2 = (long)this._y;
            return new VInt(IntMath.Sqrt(num * num + num2 * num2));
        }
    }

    public VInt2 normalized
    {
        get
        {
            VInt2 result = new VInt2(this._x, this._y);
            result.Normalize();
            return result;
        }
    }

    public static VInt2 operator +(VInt2 a, VInt2 b)
    {
        return new VInt2(a._x + b._x, a._y + b._y);
    }

    public static VInt2 operator -(VInt2 a, VInt2 b)
    {
        return new VInt2(a._x - b._x, a._y - b._y);
    }
    public static VInt2 operator -(VInt2 lhs)
    {
        lhs._x = -lhs._x;
        lhs._y = -lhs._y;
        return lhs;
    }

    public static VInt2 operator *(VInt2 lhs, VInt rhs)
    {
        lhs._x = (int)(((long)(lhs._x * rhs._val)) / VInt.Precision);
        lhs._y = (int)(((long)(lhs._y * rhs._val)) / VInt.Precision);
        return lhs;
    }
    public static VInt2 operator /(VInt2 lhs, VInt rhs)
    {
        lhs._x = (int)(((long)lhs._x * VInt.Precision) / rhs._val);
        lhs._y = (int)(((long)lhs._y * VInt.Precision) / rhs._val);
        return lhs;
    }

    public static bool operator ==(VInt2 a, VInt2 b)
    {
        return a._x == b._x && a._y == b._y;
    }

    public static bool operator !=(VInt2 a, VInt2 b)
    {
        return a._x != b._x || a._y != b._y;
    }
    public override bool Equals(object o)
    {
        if (o == null)
        {
            return false;
        }
        VInt2 vInt = (VInt2)o;
        return this._x == vInt._x && this._y == vInt._y;
    }

    public override int GetHashCode()
    {
        return this._x * 49157 + this._y * 98317;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", _x * VInt.PrecisionFactor, _y * VInt.PrecisionFactor);
    }

    public Vector2Int ToVector2Int { get { return new Vector2Int(x.ToInt, y.ToInt); } }
    public Vector2 ToVector2 { get { return new Vector2(x.ToFloat, y.ToFloat); } }
    public VInt3 ToInt3 { get { return new VInt3(_x, 0, _y); } }
    public static VInt2 FromInt3XZ(VInt3 o) { return new VInt2(o._x, o._z); }
}
