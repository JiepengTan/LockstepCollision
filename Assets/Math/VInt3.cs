using System;
using UnityEngine;

[Serializable]
public struct VInt3
{
    public VInt x { get { return new VInt(_x); } }
    public VInt y { get { return new VInt(_y); } }
    public VInt z { get { return new VInt(_z); } }

    public int _x;
    public int _y;
    public int _z;
    

    public static readonly VInt3 zero = new VInt3(0, 0, 0);
    public static readonly VInt3 one = new VInt3(VInt.Precision, VInt.Precision, VInt.Precision);
    public static readonly VInt3 half = new VInt3(VInt.Precision / 2, VInt.Precision / 2, VInt.Precision / 2);
    public static readonly VInt3 forward = new VInt3(0, 0, VInt.Precision);
    public static readonly VInt3 up = new VInt3(0, VInt.Precision, 0);
    public static readonly VInt3 right = new VInt3(VInt.Precision, 0, 0);
    public VInt3(int _x, int _y, int _z)
    {
        this._x = _x;
        this._y = _y;
        this._z = _z;
    }
    public VInt3(long _x, long _y, long _z)
    {
        this._x = (int)_x;
        this._y = (int)_y;
        this._z = (int)_z;
    }
    public VInt3(VInt x, VInt y, VInt z)
    {
        this._x = x._val;
        this._y = y._val;
        this._z = z._val;
    }

    public VInt magnitude
    {
        get
        {
            long num = (long)this._x;
            long num2 = (long)this._y;
            long num3 = (long)this._z;
            return new VInt(IntMath.Sqrt(num * num + num2 * num2 + num3 * num3));
        }
    }


    public VInt sqrMagnitude
    {
        get
        {
            long num = (long)this._x;
            long num2 = (long)this._y;
            long num3 = (long)this._z;
            return new VInt((num * num + num2 * num2 + num3 * num3) / VInt.Precision);
        }
    }

    public VInt3 abs { get { return new VInt3(Math.Abs(this._x), Math.Abs(this._y), Math.Abs(this._z)); } }
    
    public static VInt AngleInt(VInt3 lhs, VInt3 rhs)
    {
        var den = lhs.magnitude * rhs.magnitude;
        return IntMath.acos(VInt3.Dot(ref lhs, ref rhs));
    }

    public static VInt Dot(ref VInt3 lhs, ref VInt3 rhs)
    {
        var val = ((long)lhs._x) * rhs._x + ((long)lhs._y) * rhs._y + ((long)lhs._z) * rhs._z;
        return new VInt(val / VInt.Precision);
    }

    public static VInt Dot(VInt3 lhs, VInt3 rhs)
    {
        var val = ((long)lhs._x) * rhs._x + ((long)lhs._y) * rhs._y + ((long)lhs._z) * rhs._z;
        return new VInt(val / VInt.Precision); ;
    }

    public static VInt3 Cross(ref VInt3 lhs, ref VInt3 rhs)
    {
        return new VInt3(
            ((long)lhs._y * rhs._z - (long)lhs._z * rhs._y) / VInt.Precision,
            ((long)lhs._z * rhs._x - (long)lhs._x * rhs._z) / VInt.Precision,
            ((long)lhs._x * rhs._y - (long)lhs._y * rhs._x) / VInt.Precision
        );
    }

    public static VInt3 Cross(VInt3 lhs, VInt3 rhs)
    {
        return new VInt3(
            ((long)lhs._y * rhs._z - (long)lhs._z * rhs._y) / VInt.Precision,
            ((long)lhs._z * rhs._x - (long)lhs._x * rhs._z) / VInt.Precision,
            ((long)lhs._x * rhs._y - (long)lhs._y * rhs._x) / VInt.Precision
        );
    }

    public static VInt3 MoveTowards(VInt3 from, VInt3 to, VInt dt)
    {
        if ((to - from).sqrMagnitude <= (dt * dt))
        {
            return to;
        }
        return from + (to - from).NormalizeTo(dt);
    }

    public VInt3 NormalizeTo(VInt newMagn)
    {
        long num = (long)(this._x * 100);
        long num2 = (long)(this._y * 100);
        long num3 = (long)(this._z * 100);
        long num4 = num * num + num2 * num2 + num3 * num3;
        if (num4 == 0L)
        {
            return this;
        }
        long b = (long)IntMath.Sqrt(num4);
        long num5 = newMagn._val;
        this._x = (int)IntMath.Divide(num * num5, b);
        this._y = (int)IntMath.Divide(num2 * num5, b);
        this._z = (int)IntMath.Divide(num3 * num5, b);
        return this;
    }

    public VInt3 normalized
    {
        get
        {
            long num = (long)((long)this._x << 7);
            long num2 = (long)((long)this._y << 7);
            long num3 = (long)((long)this._z << 7);
            long num4 = num * num + num2 * num2 + num3 * num3;
            if (num4 == 0L)
            {
                return VInt3.zero;
            }
            var ret = new VInt3();
            long b = (long)IntMath.Sqrt(num4);
            long num5 = VInt.Precision;
            ret._x = (int)IntMath.Divide(num * num5, b);
            ret._y = (int)IntMath.Divide(num2 * num5, b);
            ret._z = (int)IntMath.Divide(num3 * num5, b);
            return ret;
        }
    }

    public VInt3 RotateY(VInt degree)
    {
        VInt s;
        VInt c;
        IntMath.sincos(out s, out c, new VInt(degree._val * 31416L / 1800000L));
        VInt3 vInt;
        vInt._x = (int)(((long)this._x * s._val + (long)this._z * c._val) / VInt.Precision);
        vInt._z = (int)(((long)this._x * -c._val + (long)this._z * s._val) / VInt.Precision);
        vInt._y = 0;
        return vInt.normalized;
    }

    
    public static VInt3 Lerp(VInt3 a, VInt3 b, VInt f)
    {
        return new VInt3(
            (int)IntMath.Divide((long)(b._x - a._x) * f._val, VInt.Precision) + a._x,
            (int)IntMath.Divide((long)(b._y - a._y) * f._val, VInt.Precision) + a._y,
            (int)IntMath.Divide((long)(b._z - a._z) * f._val, VInt.Precision) + a._z);
    }
    
    public static bool operator ==(VInt3 lhs, VInt3 rhs)
    {
        return lhs._x == rhs._x && lhs._y == rhs._y && lhs._z == rhs._z;
    }

    public static bool operator !=(VInt3 lhs, VInt3 rhs)
    {
        return lhs._x != rhs._x || lhs._y != rhs._y || lhs._z != rhs._z;
    }

    public static VInt3 operator -(VInt3 lhs, VInt3 rhs)
    {
        lhs._x -= rhs._x;
        lhs._y -= rhs._y;
        lhs._z -= rhs._z;
        return lhs;
    }

    public static VInt3 operator -(VInt3 lhs)
    {
        lhs._x = -lhs._x;
        lhs._y = -lhs._y;
        lhs._z = -lhs._z;
        return lhs;
    }

    public static VInt3 operator +(VInt3 lhs, VInt3 rhs)
    {
        lhs._x += rhs._x;
        lhs._y += rhs._y;
        lhs._z += rhs._z;
        return lhs;
    }

    public static VInt3 operator *(VInt3 lhs, VInt3 rhs)
    {
        lhs._x = (int)(((long)(lhs._x * rhs._x)) / VInt.Precision);
        lhs._y = (int)(((long)(lhs._y * rhs._y)) / VInt.Precision);
        lhs._z = (int)(((long)(lhs._z * rhs._z)) / VInt.Precision);
        return lhs;
    }

    public static VInt3 operator *(VInt3 lhs, VInt rhs)
    {
        lhs._x = (int)(((long)(lhs._x * rhs._val)) / VInt.Precision);
        lhs._y = (int)(((long)(lhs._y * rhs._val)) / VInt.Precision);
        lhs._z = (int)(((long)(lhs._z * rhs._val)) / VInt.Precision);
        return lhs;
    }

    public static VInt3 operator /(VInt3 lhs, VInt rhs)
    {
        lhs._x = (int)(((long)lhs._x * VInt.Precision) / rhs._val);
        lhs._y = (int)(((long)lhs._y * VInt.Precision) / rhs._val);
        lhs._z = (int)(((long)lhs._z * VInt.Precision) / rhs._val);
        return lhs;
    }
    
    public override string ToString()
    {
        return string.Format("({0},{1},{2})", _x * VInt.PrecisionFactor, _y * VInt.PrecisionFactor, _z * VInt.PrecisionFactor);
    }

    public override bool Equals(object o)
    {
        if (o == null)
        {
            return false;
        }
        VInt3 vInt = (VInt3)o;
        return this._x == vInt._x && this._y == vInt._y && this._z == vInt._z;
    }

    public override int GetHashCode()
    {
        return this._x * 73856093 ^ this._y * 19349663 ^ this._z * 83492791;
    }


    public Vector3Int ToVector3Int { get { return new Vector3Int(x.ToInt, y.ToInt, z.ToInt); } }
    public Vector3 ToVector3 { get { return new Vector3(_x * VInt.PrecisionFactor, _y * VInt.PrecisionFactor, _z * VInt.PrecisionFactor); } }

    #region 2D
    public VInt2 xz { get { return new VInt2(this._x, this._z); } }
    public VInt magnitude2D
    {
        get
        {
            long num = (long)this._x;
            long num2 = (long)this._z;
            return new VInt(IntMath.Sqrt(num * num + num2 * num2));
        }
    }
    public VInt sqrMagnitudeLong2D
    {
        get
        {
            long num = (long)this._x;
            long num2 = (long)this._z;
            return new VInt((num * num + num2 * num2) / VInt.Precision);
        }
    }

    public VInt XZSqrMagnitude(VInt3 rhs)
    {
        long num = (long)(this._x - rhs._x);
        long num2 = (long)(this._z - rhs._z);
        return new VInt((num * num + num2 * num2) / VInt.Precision);
    }

    public VInt XZSqrMagnitude(ref VInt3 rhs)
    {
        long num = (long)(this._x - rhs._x);
        long num2 = (long)(this._z - rhs._z);
        return new VInt((num * num + num2 * num2) / VInt.Precision);
    }

    public bool IsEqualXZ(VInt3 rhs)
    {
        return this._x == rhs._x && this._z == rhs._z;
    }
    public bool IsEqualXZ(ref VInt3 rhs)
    {
        return this._x == rhs._x && this._z == rhs._z;
    }
    public VInt3 Normal2D()
    {
        return new VInt3(this._z, this._y, -this._x);
    }

    #endregion
}
