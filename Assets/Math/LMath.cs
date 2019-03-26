using System;
using UnityEngine;
using LockStepMath;

namespace LockStepMath
{
    public static partial class LMath
    {
        public static LFloat ATan2(int y, int x)
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

            int dIM = LUTAtan2.DIM;
            long num3 = (long) (dIM - 1);
            long b = (long) ((x >= y) ? x : y);
            int num4 = (int) ((long) x * num3 / b);
            int num5 = (int) ((long) y * num3 / b);
            int num6 = LUTAtan2.table[num5 * dIM + num4];
            return new LFloat((long) ((num6 + num2) * num) / 10);
        }

        public static LFloat ACos(LFloat val)
        {
            int num = (int) (val._val * (long) LUTAcos.HALF_COUNT / LFloat.Precision) +
                      LUTAcos.HALF_COUNT;
            num = Mathf.Clamp(num, 0, LUTAcos.COUNT);
            return new LFloat((long) LUTAcos.table[num] / 10);
        }

        public static LFloat Sin(LFloat radians)
        {
            int index = LUTSinCos.getIndex(radians);
            return new LFloat((long) LUTSinCos.sin_table[index] / 10);
        }

        public static LFloat Cos(LFloat radians)
        {
            int index = LUTSinCos.getIndex(radians);
            return new LFloat((long) LUTSinCos.cos_table[index] / 10);
        }

        public static void SinCos(out LFloat s, out LFloat c, LFloat radians)
        {
            int index = LUTSinCos.getIndex(radians);
            s = new LFloat((long) LUTSinCos.sin_table[index] / 10);
            c = new LFloat((long) LUTSinCos.cos_table[index] / 10);
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

            return (int) LMath.Sqrt32((uint) a);
        }
        public static int Sqrt(long a)
        {
            if (a <= 0L)
            {
                return 0;
            }

            if (a <= (long) (0xffffffffu))
            {
                return (int) LMath.Sqrt32((uint) a);
            }

            return (int) LMath.Sqrt64((ulong) a);
        }

        public static LFloat Sqrt(LFloat a)
        {
            if (a._val <= 0)
            {
                return LFloat.zero;
            }

            return new LFloat(Sqrt((long)a._val * LFloat.Precision));
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
        public static LFloat Clamp(LFloat a, LFloat min, LFloat max)
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


        public static bool SameSign(LFloat a, LFloat b)
        {
            return (long)a._val * b._val > 0L;
        }

        public static int Abs(int val)
        {
            if (val < 0)
            {
                return -val;
            }

            return val;
        }

        public static long Abs(long val)
        {
            if (val < 0L)
            {
                return -val;
            }

            return val;
        }

        public static LFloat Abs(LFloat val)
        {
            if (val._val < 0)
            {
                return new LFloat(-val._val);
            }

            return val;
        }

        public static long Max(long a, long b)
        {
            return (a <= b) ? b : a;
        }

        public static int Max(int a, int b)
        {
            return (a <= b) ? b : a;
        }

        public static long Min(long a, long b)
        {
            return (a > b) ? b : a;
        }

        public static int Min(int a, int b)
        {
            return (a > b) ? b : a;
        }

        public static LFloat Min(LFloat a, LFloat b)
        {
            return new LFloat(Min(a._val, b._val));
        }

        public static LFloat Max(LFloat a, LFloat b)
        {
            return new LFloat(Max(a._val, b._val));
        }

        public static LFloat Lerp(LFloat a, LFloat b, LFloat f)
        {
            return new LFloat((int) (((long) (b._val - a._val) * f._val) / LFloat.Precision) + a._val);
        }

        public static LVector2D Lerp(LVector2D a, LVector2D b, LFloat f)
        {
            return new LVector2D(
                (int) (((long) (b._x - a._x) * f._val) / LFloat.Precision) + a._x,
                (int) (((long) (b._y - a._y) * f._val) / LFloat.Precision) + a._y);
        }

        public static LVector Lerp(LVector a, LVector b, LFloat f)
        {
            return new LVector(
                (int) (((long) (b._x - a._x) * f._val) / LFloat.Precision) + a._x,
                (int) (((long) (b._y - a._y) * f._val) / LFloat.Precision) + a._y,
                (int) (((long) (b._z - a._z) * f._val) / LFloat.Precision) + a._z);
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
    }
}