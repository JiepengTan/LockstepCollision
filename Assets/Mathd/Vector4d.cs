/// Vector4d.cs
/// 
/// The double type version of the Unity struct Vector4.
/// It can solve the problem that the float type may not be accurate enough.
/// 
/// Unity Vector4结构体的double版实现，以解决float型精度可能不够的问题。
/// 
/// Created by D子宇 on 2018.3.17 
/// 
/// Email: darkziyu@126.com
using System;
using UnityEngine;

namespace Mathd
{
    public struct Vector4d
    {
        #region public members

        public double x;
        public double y;
        public double z;
        public double w;

        #endregion

        #region constructor

        public Vector4d(double p_x, double p_y)
        {
            x = p_x;
            y = p_y;
            z = 0;
            w = 0;
        }
        public Vector4d(double p_x, double p_y, double p_z)
        {
            x = p_x;
            y = p_y;
            z = p_z;
            w = 0;
        }
        public Vector4d(double p_x, double p_y, double p_z,double p_w)
        {
            x = p_x;
            y = p_y;
            z = p_z;
            w = p_w;
        }

        #endregion

        #region public properties

        public double this[int index] {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector4d index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector4d index!");
                }
            }
        }
        
        public static Vector4d one
        {
            get
            {
                return new Vector4d(1, 1, 1, 1);
            }
        }
        public static Vector4d zero
        {
            get
            {
                return new Vector4d(0, 0, 0, 0);
            }
        }
        public double magnitude
        {
            get
            {
                return Math.Sqrt(sqrMagnitude);
            }
        }
        public Vector4d normalized
        {
            get
            {
                return Vector4.Normalize(this);
            }
        }
        public double sqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z + w * w;

            }
        }

        #endregion

        #region public functions

        /// <summary>
        /// 距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(Vector4d a, Vector4d b)
        {
            return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z) + (a.w - b.w) * (a.w - b.w));
        }
        /// <summary>
        /// 点乘
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static double Dot(Vector4d lhs, Vector4d rhs) {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z + lhs.w * rhs.w;
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector4d Lerp(Vector4d a, Vector4d b, float t) {
            if (t <= 0)
            {
                return a;
            }
            else if (t >= 1)
            {
                return b;
            }
            return a + (b - a) * t;
        }
        /// <summary>
        /// 线性插值(无限制)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector4d LerpUnclamped(Vector4d a, Vector4d b, double t)
        {
            return a + (b - a) * t;
        }
        /// <summary>
        /// 向量模长
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Magnitude(Vector4d a)
        {
            return a.magnitude;
        }
        /// <summary>
        /// 最大值(X,Y,Z,W均取最大)
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector4d Max(Vector4d lhs, Vector4d rhs)
        {
            Vector4d temp = new Vector4d();
            temp.x = Math.Max(lhs.x, rhs.x);
            temp.y = Math.Max(lhs.y, rhs.y);
            temp.z = Math.Max(lhs.z, rhs.z);
            temp.w = Math.Max(lhs.w, rhs.w);
            return temp;
        }
        /// <summary>
        /// 最小值(X,Y,Z,W均取最小)
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector4d Min(Vector4d lhs, Vector4d rhs) {
            Vector4d temp = new Vector4d();
            temp.x = Math.Min(lhs.x, rhs.x);
            temp.y = Math.Min(lhs.y, rhs.y);
            temp.z = Math.Min(lhs.z, rhs.z);
            temp.w = Math.Min(lhs.w, rhs.w);
            return temp;
        }
        /// <summary>
        /// 向目标点移动
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static Vector4d MoveTowards(Vector4d current, Vector4d target, double maxDistanceDelta)
        {
            Vector4d vector4 = target - current;
            double single = vector4.magnitude;
            if (single <= maxDistanceDelta || single == 0f)
            {
                return target;
            }
            return current + ((vector4 / single) * maxDistanceDelta);
        }
        /// <summary>
        /// 单位化
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Vector4d Normalize(Vector4d value)
        {
            if (value == zero)
            {
                return zero;
            }
            else
            {
                Vector4d tempDVec = new Vector4d();
                tempDVec.x = value.x / value.magnitude;
                tempDVec.y = value.y / value.magnitude;
                tempDVec.z = value.z / value.magnitude;
                tempDVec.w = value.w / value.magnitude;
                return tempDVec;
            }
        }
        /// <summary>
        /// 向量投影
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        /// <returns></returns>
        public static Vector4d Project(Vector4d vector, Vector4d onNormal)
        {
            if (vector == zero || onNormal == zero)
            {
                return zero;
            }
            return Dot(vector, onNormal) / (onNormal.magnitude * onNormal.magnitude) * onNormal;
        }
        /// <summary>
        /// 向量缩放
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector4d Scale(Vector4d a, Vector4d b)
        {
            Vector4d temp = new Vector4d();
            temp.x = a.x * b.x;
            temp.y = a.y * b.y;
            temp.z = a.z * b.z;
            temp.w = a.w * b.w;
            return temp;
        }
        /// <summary>
        /// 模长平方
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double SqrMagnitude(Vector4d a)
        {
            return a.sqrMagnitude;
        }
        /// <summary>
        /// 单位化
        /// </summary>
        public void Normalize() {
            if (this != zero)
            {
                double length = magnitude;
                x /= length;
                y /= length;
                z /= length;
                w /= length;
            }
        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(Vector4d scale) {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
            w *= scale.w;
        }
        /// <summary>
        /// 设置向量
        /// </summary>
        /// <param name="new_x"></param>
        /// <param name="new_y"></param>
        /// <param name="new_z"></param>
        /// <param name="new_w"></param>
        public void Set(double new_x, double new_y, double new_z, double new_w) {
            x = new_x;
            y = new_y;
            z = new_z;
            w = new_w;
        }
        /// <summary>
        /// 模长平方
        /// </summary>
        /// <returns></returns>
        public double SqrMagnitude()
        {
            return sqrMagnitude;
        }
        public override string ToString() {
            return String.Format("({0}, {1}, {2}, {3})", x, y, z, w);
        }
        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2 ^ this.w.GetHashCode() >> 1;
        }
        public override bool Equals(object other)
        {
            return this == (Vector4d)other;
        }
        public string ToString(string format) {
            return String.Format("({0}, {1}, {2}, {3})", x.ToString(format), y.ToString(format), z.ToString(format), w.ToString(format));
        }
        public Vector4 ToVector4()
        {
            return new Vector4((float)x, (float)y, (float)z, (float)w);
        }

        #endregion

        #region operator

        public static Vector4d operator +(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }
        public static Vector4d operator -(Vector4d a)
        {
            return new Vector4d(-a.x, -a.y, -a.z, -a.w);
        }
        public static Vector4d operator -(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }
        public static Vector4d operator *(double d, Vector4d a)
        {
            return new Vector4d(a.x * d, a.y * d, a.z * d, a.w * d);
        }
        public static Vector4d operator *(Vector4d a, double d)
        {
            return new Vector4d(a.x * d, a.y * d, a.z * d, a.w * d);
        }
        public static Vector4d operator /(Vector4d a, double d)
        {
            return new Vector4d(a.x / d, a.y / d, a.z / d, a.w / d);
        }
        public static bool operator ==(Vector4d lhs, Vector4d rhs)
        {
            if (lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(Vector4d lhs, Vector4d rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator Vector4d(Vector2d v) {
            return new Vector4d(v.x, v.y, 0, 0);
        }
        public static implicit operator Vector4d(Vector3d v) {
            return new Vector4d(v.x, v.y, v.z, 0);
        }
        public static implicit operator Vector2d(Vector4d v) {
            return new Vector2d(v.x, v.y);
        }
        public static implicit operator Vector3d(Vector4d v) {
            return new Vector3d(v.x, v.y, v.z);
        }
        public static implicit operator Vector4d(Vector4 v)
        {
            return new Vector4d(v.x, v.y, v.z, v.w);
        }
        public static implicit operator Vector4(Vector4d v)
        {
            return new Vector4((float)v.x, (float)v.y, (float)v.z, (float)v.w);
        }

        #endregion
    }
}
