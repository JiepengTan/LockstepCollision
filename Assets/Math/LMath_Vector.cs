using System;
using LockStepMath;

namespace LockStepMath
{
    public static partial class LMath
    {
        public static LFloat Dot(LVector2D u, LVector2D v)
        {
            return new LFloat(((long) u._x * v._x + (long) u._y * v._y) / LFloat.Precision);
        }

        public static LFloat Dot(ref LVector lhs, ref LVector rhs)
        {
            var val = ((long) lhs._x) * rhs._x + ((long) lhs._y) * rhs._y + ((long) lhs._z) * rhs._z;
            return new LFloat(val / LFloat.Precision);
        }

        public static LFloat Dot(LVector lhs, LVector rhs)
        {
            var val = ((long) lhs._x) * rhs._x + ((long) lhs._y) * rhs._y + ((long) lhs._z) * rhs._z;
            return new LFloat(val / LFloat.Precision);
            ;
        }

        public static LVector Cross(ref LVector lhs, ref LVector rhs)
        {
            return new LVector(
                ((long) lhs._y * rhs._z - (long) lhs._z * rhs._y) / LFloat.Precision,
                ((long) lhs._z * rhs._x - (long) lhs._x * rhs._z) / LFloat.Precision,
                ((long) lhs._x * rhs._y - (long) lhs._y * rhs._x) / LFloat.Precision
            );
        }

        public static LVector Cross(LVector lhs, LVector rhs)
        {
            return new LVector(
                ((long) lhs._y * rhs._z - (long) lhs._z * rhs._y) / LFloat.Precision,
                ((long) lhs._z * rhs._x - (long) lhs._x * rhs._z) / LFloat.Precision,
                ((long) lhs._x * rhs._y - (long) lhs._y * rhs._x) / LFloat.Precision
            );
        }


        public static LVector Transform(ref LVector point, ref LVector axis_x, ref LVector axis_y, ref LVector axis_z,
            ref LVector trans)
        {
            return new LVector(
                ((axis_x._x * point._x + axis_y._x * point._y + axis_z._x * point._z) / LFloat.Precision) + trans._x,
                ((axis_x._y * point._x + axis_y._y * point._y + axis_z._y * point._z) / LFloat.Precision) + trans._y,
                ((axis_x._z * point._x + axis_y._z * point._y + axis_z._z * point._z) / LFloat.Precision) + trans._z);
        }

        public static LVector Transform(LVector point, ref LVector axis_x, ref LVector axis_y, ref LVector axis_z,
            ref LVector trans)
        {
            return new LVector(
                ((axis_x._x * point._x + axis_y._x * point._y + axis_z._x * point._z) / LFloat.Precision) + trans._x,
                ((axis_x._y * point._x + axis_y._y * point._y + axis_z._y * point._z) / LFloat.Precision) + trans._y,
                ((axis_x._z * point._x + axis_y._z * point._y + axis_z._z * point._z) / LFloat.Precision) + trans._z);
        }

        public static LVector Transform(ref LVector point, ref LVector axis_x, ref LVector axis_y, ref LVector axis_z,
            ref LVector trans, ref LVector scale)
        {
            long num = (long) point._x * (long) scale._x;
            long num2 = (long) point._y * (long) scale._x;
            long num3 = (long) point._z * (long) scale._x;
            return new LVector(
                (int) (((long) axis_x._x * num + (long) axis_y._x * num2 + (long) axis_z._x * num3) / 1000000L) +
                trans._x,
                (int) (((long) axis_x._y * num + (long) axis_y._y * num2 + (long) axis_z._y * num3) / 1000000L) +
                trans._y,
                (int) (((long) axis_x._z * num + (long) axis_y._z * num2 + (long) axis_z._z * num3) / 1000000L) +
                trans._z);
        }

        public static LVector Transform(ref LVector point, ref LVector forward, ref LVector trans)
        {
            LVector up = LVector.up;
            LVector vInt = Cross(LVector.up, forward);
            return LMath.Transform(ref point, ref vInt, ref up, ref forward, ref trans);
        }

        public static LVector Transform(LVector point, LVector forward, LVector trans)
        {
            LVector up = LVector.up;
            LVector vInt = Cross(LVector.up, forward);
            return LMath.Transform(ref point, ref vInt, ref up, ref forward, ref trans);
        }

        public static LVector Transform(LVector point, LVector forward, LVector trans, LVector scale)
        {
            LVector up = LVector.up;
            LVector vInt = Cross(LVector.up, forward);
            return LMath.Transform(ref point, ref vInt, ref up, ref forward, ref trans, ref scale);
        }



        public static LVector MoveTowards(LVector from, LVector to, LFloat dt)
        {
            if ((to - from).sqrMagnitude <= (dt * dt))
            {
                return to;
            }

            return from + (to - from).Normalize(dt);
        }


        public static LFloat AngleInt(LVector lhs, LVector rhs)
        {
            return LMath.ACos(Dot(ref lhs, ref rhs));
        }

    }
}