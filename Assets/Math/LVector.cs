using System;
using UnityEngine;

namespace LockStepMath
{
    [Serializable]
    public struct LVector : IEquatable<LVector>
    {
        public LFloat x
        {
            get { return new LFloat(_x); }
        }

        public LFloat y
        {
            get { return new LFloat(_y); }
        }

        public LFloat z
        {
            get { return new LFloat(_z); }
        }

        public int _x;
        public int _y;
        public int _z;


        public static readonly LVector zero = new LVector(0, 0, 0);
        public static readonly LVector one = new LVector(LFloat.Precision, LFloat.Precision, LFloat.Precision);

        public static readonly LVector half = new LVector(LFloat.Precision / 2, LFloat.Precision / 2,
            LFloat.Precision / 2);

        public static readonly LVector forward = new LVector(0, 0, LFloat.Precision);
        public static readonly LVector up = new LVector(0, LFloat.Precision, 0);
        public static readonly LVector right = new LVector(LFloat.Precision, 0, 0);

        public LVector(int _x, int _y, int _z)
        {
            this._x = _x;
            this._y = _y;
            this._z = _z;
        }

        public LVector(long _x, long _y, long _z)
        {
            this._x = (int) _x;
            this._y = (int) _y;
            this._z = (int) _z;
        }

        public LVector(LFloat x, LFloat y, LFloat z)
        {
            this._x = x._val;
            this._y = y._val;
            this._z = z._val;
        }

        public LFloat magnitude
        {
            get
            {
                long num = (long) this._x;
                long num2 = (long) this._y;
                long num3 = (long) this._z;
                return new LFloat(LMath.Sqrt(num * num + num2 * num2 + num3 * num3));
            }
        }


        public LFloat sqrMagnitude
        {
            get
            {
                long num = (long) this._x;
                long num2 = (long) this._y;
                long num3 = (long) this._z;
                return new LFloat((num * num + num2 * num2 + num3 * num3) / LFloat.Precision);
            }
        }

        public LVector abs
        {
            get { return new LVector(LMath.Abs(this._x), LMath.Abs(this._y), LMath.Abs(this._z)); }
        }

        public LVector Normalize()
        {
            return Normalize((LFloat) 1);
        }

        public LVector Normalize(LFloat newMagn)
        {
            long num = (long) (this._x * 100);
            long num2 = (long) (this._y * 100);
            long num3 = (long) (this._z * 100);
            long num4 = num * num + num2 * num2 + num3 * num3;
            if (num4 == 0L)
            {
                return this;
            }

            long b = (long) LMath.Sqrt(num4);
            long num5 = newMagn._val;
            this._x = (int) (num * num5 / b);
            this._y = (int) (num2 * num5 / b);
            this._z = (int) (num3 * num5 / b);
            return this;
        }

        public LVector normalized
        {
            get
            {
                long num = (long) ((long) this._x << 7);
                long num2 = (long) ((long) this._y << 7);
                long num3 = (long) ((long) this._z << 7);
                long num4 = num * num + num2 * num2 + num3 * num3;
                if (num4 == 0L)
                {
                    return LVector.zero;
                }

                var ret = new LVector();
                long b = (long) LMath.Sqrt(num4);
                long num5 = LFloat.Precision;
                ret._x = (int) (num * num5 / b);
                ret._y = (int) (num2 * num5 / b);
                ret._z = (int) (num3 * num5 / b);
                return ret;
            }
        }

        public LVector RotateY(LFloat degree)
        {
            LFloat s;
            LFloat c;
            LMath.SinCos(out s, out c, new LFloat(degree._val * 31416L / 1800000L));
            LVector vInt;
            vInt._x = (int) (((long) this._x * s._val + (long) this._z * c._val) / LFloat.Precision);
            vInt._z = (int) (((long) this._x * -c._val + (long) this._z * s._val) / LFloat.Precision);
            vInt._y = 0;
            return vInt.normalized;
        }


        public static bool operator ==(LVector lhs, LVector rhs)
        {
            return lhs._x == rhs._x && lhs._y == rhs._y && lhs._z == rhs._z;
        }

        public static bool operator !=(LVector lhs, LVector rhs)
        {
            return lhs._x != rhs._x || lhs._y != rhs._y || lhs._z != rhs._z;
        }

        public static LVector operator -(LVector lhs, LVector rhs)
        {
            lhs._x -= rhs._x;
            lhs._y -= rhs._y;
            lhs._z -= rhs._z;
            return lhs;
        }

        public static LVector operator -(LVector lhs)
        {
            lhs._x = -lhs._x;
            lhs._y = -lhs._y;
            lhs._z = -lhs._z;
            return lhs;
        }

        public static LVector operator +(LVector lhs, LVector rhs)
        {
            lhs._x += rhs._x;
            lhs._y += rhs._y;
            lhs._z += rhs._z;
            return lhs;
        }

        public static LVector operator *(LVector lhs, LVector rhs)
        {
            lhs._x = (int) (((long) (lhs._x * rhs._x)) / LFloat.Precision);
            lhs._y = (int) (((long) (lhs._y * rhs._y)) / LFloat.Precision);
            lhs._z = (int) (((long) (lhs._z * rhs._z)) / LFloat.Precision);
            return lhs;
        }

        public static LVector operator *(LVector lhs, LFloat rhs)
        {
            lhs._x = (int) (((long) (lhs._x * rhs._val)) / LFloat.Precision);
            lhs._y = (int) (((long) (lhs._y * rhs._val)) / LFloat.Precision);
            lhs._z = (int) (((long) (lhs._z * rhs._val)) / LFloat.Precision);
            return lhs;
        }

        public static LVector operator /(LVector lhs, LFloat rhs)
        {
            lhs._x = (int) (((long) lhs._x * LFloat.Precision) / rhs._val);
            lhs._y = (int) (((long) lhs._y * LFloat.Precision) / rhs._val);
            lhs._z = (int) (((long) lhs._z * LFloat.Precision) / rhs._val);
            return lhs;
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", _x * LFloat.PrecisionFactor, _y * LFloat.PrecisionFactor,
                _z * LFloat.PrecisionFactor);
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return false;
            }

            LVector other = (LVector) o;
            return this._x == other._x && this._y == other._y && this._z == other._z;
        }


        public bool Equals(LVector other)
        {
            return this._x == other._x && this._y == other._y && this._z == other._z;
        }


        public override int GetHashCode()
        {
            return this._x * 73856093 ^ this._y * 19349663 ^ this._z * 83492791;
        }


        public Vector3Int ToVector3Int
        {
            get { return new Vector3Int(x.ToInt, y.ToInt, z.ToInt); }
        }

        public Vector3 ToVector3
        {
            get
            {
                return new Vector3(_x * LFloat.PrecisionFactor, _y * LFloat.PrecisionFactor,
                    _z * LFloat.PrecisionFactor);
            }
        }

        #region 2D

        public LVector2D xz
        {
            get { return new LVector2D(this._x, this._z); }
        }

        public LFloat magnitude2D
        {
            get
            {
                long num = (long) this._x;
                long num2 = (long) this._z;
                return new LFloat(LMath.Sqrt(num * num + num2 * num2));
            }
        }

        public LFloat sqrMagnitudeLong2D
        {
            get
            {
                long num = (long) this._x;
                long num2 = (long) this._z;
                return new LFloat((num * num + num2 * num2) / LFloat.Precision);
            }
        }

        public LFloat XZSqrMagnitude(LVector rhs)
        {
            long num = (long) (this._x - rhs._x);
            long num2 = (long) (this._z - rhs._z);
            return new LFloat((num * num + num2 * num2) / LFloat.Precision);
        }

        public LFloat XZSqrMagnitude(ref LVector rhs)
        {
            long num = (long) (this._x - rhs._x);
            long num2 = (long) (this._z - rhs._z);
            return new LFloat((num * num + num2 * num2) / LFloat.Precision);
        }

        public bool IsEqualXZ(LVector rhs)
        {
            return this._x == rhs._x && this._z == rhs._z;
        }

        public bool IsEqualXZ(ref LVector rhs)
        {
            return this._x == rhs._x && this._z == rhs._z;
        }

        public LVector Normal2D()
        {
            return new LVector(this._z, this._y, -this._x);
        }

        #endregion
    }
}