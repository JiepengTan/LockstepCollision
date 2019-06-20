using System;
using UnityEngine;

using Lockstep.Math;
namespace Lockstep.Math
{

    [Serializable]
    public struct LVector2
    {
        public LFloat x
        {
            get { return new LFloat(_x); }
        }

        public LFloat y
        {
            get { return new LFloat(_y); }
        }

        public int _x;
        public int _y;
        public static readonly LVector2 zero = new LVector2(0, 0);
        public static readonly LVector2 one = new LVector2(LFloat.Precision, LFloat.Precision);
        public static readonly LVector2 half = new LVector2(LFloat.Precision / 2, LFloat.Precision / 2);
        public static readonly LVector2 up = new LVector2(0, LFloat.Precision);
        public static readonly LVector2 down = new LVector2(0, -LFloat.Precision);
        public static readonly LVector2 right = new LVector2(LFloat.Precision, 0);
        public static readonly LVector2 left = new LVector2(-LFloat.Precision, 0);

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

        public LVector2(LVector o)
        {
            this._x = o._x;
            this._y = o._y;
        }

        public LVector2(LFloat x, LFloat y)
        {
            this._x = x._val;
            this._y = y._val;
        }

        public LVector2(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        public static LFloat Dot(LVector2 a, LVector2 b)
        {
            long num = (long) a._x;
            long num2 = (long) a._y;
            return new LFloat((num * b._x + num2 * b._y) / LFloat.Precision);
        }


        public static LFloat Cross(ref LVector2 a, ref LVector2 b)
        {
            return new LFloat(((long) a._x * (long) b._y - (long) a._y * (long) b._x) / LFloat.Precision);
        }

        /// <summary>
        /// clockwise 顺时针旋转  
        /// 1表示顺时针旋转 90 degree
        /// 2表示顺时针旋转 180 degree
        /// </summary>
        public static LVector2 Rotate(LVector2 v, int r)
        {
            r %= 4;
            return new LVector2(
                v._x * LVector2.Rotations[r * 4] + v._y * LVector2.Rotations[r * 4 + 1],
                v._x * LVector2.Rotations[r * 4 + 2] + v._y * LVector2.Rotations[r * 4 + 3]);
        }

        public static LVector2 Min(LVector2 a, LVector2 b)
        {
            return new LVector2(LMath.Min(a._x, b._x), LMath.Min(a._y, b._y));
        }

        public static LVector2 Max(LVector2 a, LVector2 b)
        {
            return new LVector2(LMath.Max(a._x, b._x), LMath.Max(a._y, b._y));
        }

        public void Min(ref LVector2 r)
        {
            this._x = Mathf.Min(this._x, r._x);
            this._y = Mathf.Min(this._y, r._y);
        }

        public void Max(ref LVector2 r)
        {
            this._x = Mathf.Max(this._x, r._x);
            this._y = Mathf.Max(this._y, r._y);
        }


        public void Normalize()
        {
            long num = (long) (this._x * 100);
            long num2 = (long) (this._y * 100);
            long num3 = num * num + num2 * num2;
            if (num3 == 0L)
            {
                return;
            }

            long b = (long) LMath.Sqrt(num3);
            this._x = (int) (num * 1000L / b);
            this._y = (int) (num2 * 1000L / b);
        }

        public LFloat sqrMagnitude
        {
            get
            {
                long num = (long) this._x;
                long num2 = (long) this._y;
                return new LFloat((num * num + num2 * num2) / LFloat.Precision);
            }
        }

        public long rawSqrMagnitude
        {
            get
            {
                long num = (long) this._x;
                long num2 = (long) this._y;
                return num * num + num2 * num2;
            }
        }

        public LFloat magnitude
        {
            get
            {
                long num = (long) this._x;
                long num2 = (long) this._y;
                return new LFloat(LMath.Sqrt(num * num + num2 * num2));
            }
        }

        public LVector2 normalized
        {
            get
            {
                LVector2 result = new LVector2(this._x, this._y);
                result.Normalize();
                return result;
            }
        }

        public static LVector2 operator +(LVector2 a, LVector2 b)
        {
            return new LVector2(a._x + b._x, a._y + b._y);
        }

        public static LVector2 operator -(LVector2 a, LVector2 b)
        {
            return new LVector2(a._x - b._x, a._y - b._y);
        }

        public static LVector2 operator -(LVector2 lhs)
        {
            lhs._x = -lhs._x;
            lhs._y = -lhs._y;
            return lhs;
        }

        public static LVector2 operator *(LVector2 lhs, LFloat rhs)
        {
            lhs._x = (int) (((long) (lhs._x * rhs._val)) / LFloat.Precision);
            lhs._y = (int) (((long) (lhs._y * rhs._val)) / LFloat.Precision);
            return lhs;
        }

        public static LVector2 operator /(LVector2 lhs, LFloat rhs)
        {
            lhs._x = (int) (((long) lhs._x * LFloat.Precision) / rhs._val);
            lhs._y = (int) (((long) lhs._y * LFloat.Precision) / rhs._val);
            return lhs;
        }

        public static bool operator ==(LVector2 a, LVector2 b)
        {
            return a._x == b._x && a._y == b._y;
        }

        public static bool operator !=(LVector2 a, LVector2 b)
        {
            return a._x != b._x || a._y != b._y;
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return false;
            }

            LVector2 vInt = (LVector2) o;
            return this._x == vInt._x && this._y == vInt._y;
        }

        public override int GetHashCode()
        {
            return this._x * 49157 + this._y * 98317;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", _x * LFloat.PrecisionFactor, _y * LFloat.PrecisionFactor);
        }

        public Vector2Int ToVector2Int
        {
            get { return new Vector2Int(x.ToInt, y.ToInt); }
        }

        public Vector2 ToVector2
        {
            get { return new Vector2(x.ToFloat, y.ToFloat); }
        }

        public LVector ToInt3
        {
            get { return new LVector(_x, 0, _y); }
        }

        public static LVector2 FromInt3XZ(LVector o)
        {
            return new LVector2(o._x, o._z);
        }
        
        public LFloat this[int index]

        {

            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default: throw new IndexOutOfRangeException("vector idx invalid" + index);
                }
            }

            set
            {
                switch (index)
                {
                    case 0: _x = value._val; break;
                    case 1: _y = value._val;break;
                    default: throw new IndexOutOfRangeException("vector idx invalid" + index);
                }
            }

        }
    }
}