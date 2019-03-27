/// Vector2d.cs
/// 
/// The double type version of the Unity struct Vector2.
/// It can solve the problem that the float type may not be accurate enough.
/// 
/// Unity Vector2结构体的double版实现，以解决float型精度可能不够的问题。
/// 
/// Created by D子宇 on 2018.3.17 
/// 
/// Email: darkziyu@126.com
using System;
using UnityEngine;

namespace Mathd
{
    public struct Vector2d
    {
        #region public members

        public double x;
        public double y;

        #endregion

        #region constructor

        public Vector2d(double p_x,double p_y) {
            x = p_x;
            y = p_y;
        }

        #endregion

        #region public properties

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2d index!");
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
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2d index!");
                }
            }
        }

        public static Vector2d down
        {
            get
            {
                return new Vector2d(0, -1);
            }
        }
        public static Vector2d left
        {
            get
            {
                return new Vector2d(-1, 0);
            }
        }
        public static Vector2d one
        {
            get
            {
                return new Vector2d(1, 1);
            }
        }
        public static Vector2d right
        {
            get
            {
                return new Vector2d(1, 0);
            }
        }
        public static Vector2d up
        {
            get
            {
                return new Vector2d(0, 1);
            }
        }
        public static Vector2d zero
        {
            get
            {
                return new Vector2d(0, 0);

            }
        }
        public double magnitude
        {
            get
            {
                return Math.Sqrt(sqrMagnitude);
            }
        }
        public Vector2d normalized
        {
            get
            {
                Vector2d result = new Vector2d(x, y);
                result.Normalize();
                return result;
            }
        }
        public double sqrMagnitude
        {
            get
            {
                return x * x + y * y;
            }
        }

        #endregion

        #region public functions

        /// <summary>
        /// 角度
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Angle(Vector2d from, Vector2d to) {
            double cos = Dot(from.normalized, to.normalized);
            if (cos < -1)
            {
                cos = -1;
            }
            if (cos > 1)
            {
                cos = 1;
            }
            return (float)(Math.Acos(cos) * (180 / Math.PI));
        }
        /// <summary>
        /// 限制距离
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static Vector2d ClampMagnitude(Vector2d vector, double maxLength) {
            if (maxLength * maxLength >= vector.sqrMagnitude)
            {
                return vector;
            }
            return vector.normalized * maxLength;
        }
        /// <summary>
        /// 距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(Vector2d a, Vector2d b) {
            return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }
        /// <summary>
        /// 点乘
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static double Dot(Vector2d lhs, Vector2d rhs) {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2d Lerp(Vector2d a, Vector2d b, double t)
        {
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
        public static Vector2d LerpUnclamped(Vector2d a, Vector2d b, double t)
        {
            return a + (b - a) * t;
        }
        /// <summary>
        /// 最大值(X,Y均取最大)
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector2d Max(Vector2d lhs, Vector2d rhs) {
            Vector2d temp = new Vector2d();
            temp.x = Math.Max(lhs.x, rhs.x);
            temp.y = Math.Max(lhs.y, rhs.y);
            return temp;
        }
        /// <summary>
        /// 最小值(X,Y均取最小)
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector2d Min(Vector2d lhs, Vector2d rhs)
        {
            Vector2d temp = new Vector2d();
            temp.x = Math.Min(lhs.x, rhs.x);
            temp.y = Math.Min(lhs.y, rhs.y);
            return temp;
        }
        /// <summary>
        /// 向目标点移动
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static Vector2d MoveTowards(Vector2d current, Vector2d target, double maxDistanceDelta) {
            Vector2d vector2 = target - current;
            double single = vector2.magnitude;
            if (single <= maxDistanceDelta || single == 0f)
            {
                return target;
            }
            return current + ((vector2 / single) * maxDistanceDelta);
        }
        /// <summary>
        /// 反射
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        /// <returns></returns>
        public static Vector2d Reflect(Vector2d inDirection, Vector2d inNormal) {
            return (-2f * Dot(inNormal, inDirection)) * inNormal + inDirection;
        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2d Scale(Vector2d a, Vector2d b) {
            Vector2d temp = new Vector2d();
            temp.x = a.x * b.x;
            temp.y = a.y * b.y;
            return temp;
        }
        /// <summary>
        /// 平滑阻尼
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="smoothTime"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector2d SmoothDamp(Vector2d current, Vector2d target, ref Vector2d currentVelocity, double smoothTime, double maxSpeed, double deltaTime) {
            smoothTime = Math.Max(0.0001, smoothTime);
            double num = 2 / smoothTime;
            double num2 = num * deltaTime;
            double d = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
            Vector2d vector = current - target;
            Vector2d vector2 = target;
            double maxLength = maxSpeed * smoothTime;
            vector = ClampMagnitude(vector, maxLength);
            target = current - vector;
            Vector2d vector3 = (currentVelocity + num * vector) * deltaTime;
            currentVelocity = (currentVelocity - num * vector3) * d;
            Vector2d vector4 = target + (vector + vector3) * d;
            if (Dot(vector2 - current, vector4 - vector2) > 0f)
            {
                vector4 = vector2;
                currentVelocity = (vector4 - vector2) / deltaTime;
            }
            return vector4;
        }
        /// <summary>
        /// 模长平方
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double SqrMagnitude(Vector2d a)
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
            }
        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(Vector2d scale) {
            x *= scale.x;
            y *= scale.y;
        }
        /// <summary>
        /// 设置向量
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        public void Set(double newX, double newY) {
            x = newX;
            y = newY;
        }
        public double SqrMagnitude()
        {
            return sqrMagnitude;
        }
        public override string ToString() {
            return String.Format("({0}, {1})", x, y);
        }
        public override bool Equals(object other)
        {
            return this == (Vector2d)other;
        }
        public string ToString(string format) {
            return String.Format("({0}, {1})", x.ToString(format), y.ToString(format));
        }
        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
        }
        public Vector2d ToVector2()
        {
            return new Vector2d((float)x, (float)y);
        }

        #endregion

        #region operator

        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x + b.x, a.y + b.y);
        }
        public static Vector2d operator -(Vector2d a)
        {
            return new Vector2d(-a.x, -a.y);
        }
        public static Vector2d operator -(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x - b.x, a.y - b.y);
        }
        public static Vector2d operator *(double d, Vector2d a)
        {
            return new Vector2d(a.x * d, a.y * d);
        }
        public static Vector2d operator *(Vector2d a, double d)
        {
            return new Vector2d(a.x * d, a.y * d);
        }
        public static Vector2d operator /(Vector2d a, double d)
        {
            return new Vector2d(a.x / d, a.y / d);
        }
        public static bool operator ==(Vector2d lhs, Vector2d rhs)
        {
            if (lhs.x == rhs.x && lhs.y == rhs.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(Vector2d lhs, Vector2d rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator Vector2d(Vector3d v)
        {
            return new Vector2d(v.x, v.y);
        }
        public static implicit operator Vector3d(Vector2d v)
        {
            return new Vector3d(v.x, v.y, 0);
        }
        public static implicit operator Vector2d(Vector2 v)
        {
            return new Vector2d(v.x, v.y);
        }
        public static implicit operator Vector2(Vector2d v)
        {
            return new Vector2((float)v.x, (float)v.y);
        }

        #endregion
    }
}
