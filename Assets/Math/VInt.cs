using System;

[Serializable]
public struct VInt
{
    public const int Precision = 1000;
    public const float PrecisionFactor = 0.001f;

    public int _val;

    public static readonly VInt zero = new VInt( 0);
    public static readonly VInt one = new VInt( VInt.Precision);

    /// <summary>
    /// 传入的是正常数放大1000 的数值
    /// </summary>
    /// <param name="i"></param>
    public VInt(int i)
    {
        this._val = i;
    }
    /// <summary>
    /// 传入的是正常数放大1000 的数值
    /// </summary>
    /// <param name="i"></param>
    public VInt(long i)
    {
        this._val = (int)i;
    }
    public VInt(VInt rhs)
    {
        this._val = rhs._val;
    }

    public static VInt Min(VInt a, VInt b)
    {
        return new VInt(Math.Min(a._val, b._val));
    }

    public static VInt Max(VInt a, VInt b)
    {
        return new VInt(Math.Max(a._val, b._val));
    }

    public static bool operator <(VInt a, VInt b) { return a._val < b._val; }
    public static bool operator >(VInt a, VInt b) { return a._val > b._val; }
    public static bool operator <=(VInt a, VInt b) { return a._val <= b._val; }
    public static bool operator >=(VInt a, VInt b) { return a._val >= b._val; }
    public static bool operator ==(VInt a, VInt b) { return a._val == b._val; }
    public static bool operator !=(VInt a, VInt b) { return a._val != b._val; }
    public static VInt operator +(VInt a, VInt b) { return new VInt(a._val + b._val); }
    public static VInt operator -(VInt a, VInt b) { return new VInt(a._val - b._val); }
    public static VInt operator *(VInt a, VInt b)
    {
        long val = (long)(a._val) * b._val;
        return new VInt((int)(val / 1000));
    }

    public static VInt operator /(VInt a, VInt b)
    {
        long val = (long)(a._val * 1000) / b._val;
        return new VInt((int)(val));
    }


    public override bool Equals(object o)
    {
        if (o == null)
        {
            return false;
        }
        VInt vInt = (VInt)o;
        return this._val == vInt._val;
    }

    public override int GetHashCode() { return _val; }
    public override string ToString() { return (_val * VInt.PrecisionFactor).ToString(); }

    public int ToInt { get { return _val / 1000; } }
    public long ToLong { get { return _val / 1000; } }
    public float ToFloat { get { return _val / 0.001f; } }
    public double ToDouble { get { return _val / 0.001; } }


    public VInt Floor
    {
        get
        {
            int x = this._val;
            if (x > 0) { x /= VInt.Precision; }
            else
            {
                if (x % VInt.Precision == 0) { x /= VInt.Precision; }
                else { x = x / VInt.Precision - 1; }
            }
            return new VInt(x * VInt.Precision);
        }
    }

    public VInt Ceil
    {
        get
        {
            int x = this._val;
            if (x < 0) { x /= VInt.Precision; }
            else
            {
                if (x % VInt.Precision == 0) { x /= VInt.Precision; }
                else { x = x / VInt.Precision + 1; }
            }
            return new VInt(x * VInt.Precision);
        }
    }


}
