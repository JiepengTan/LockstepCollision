using System.Numerics;
using Lockstep.Math;
using UnityEditor.Experimental.UIElements.GraphView;
using static Lockstep.Math.LMath;
using Point = Lockstep.Math.LVector;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision {
    [System.Serializable]
    public partial class Triangle : BaseShape {
        public Point a;
        public Point b;
        public Point c;

        //所在平面 
        public LVector n;
        public LFloat d;

        /// <summary>
        /// Collision Type
        /// </summary>
        public override EColType ColType {
            get { return EColType.Triangle; }
        }
    }
    
    
    public class SAABB {
        public LVector min;
        public LVector max;
    }

    /// <summary>
    /// 结构体 方便 内存紧凑
    /// </summary>
    public struct STriangle {
        public Point a;
        public Point b;
        public Point c;

        public STriangle(Point a, Point b, Point c){
            this.a = a;
            this.b = b;
            this.c = c;
            this.n = Cross(b - a, c - a).normalized;
            this.d = Dot(n, a);
            this.min = a;
            this.max = a;
            for (int i = 0; i < 3; i++) {
                LFloat minv = a[i];
                LFloat maxv = a[i];
                var _b = b[i];
                var _c = c[i];
                if (_b < minv) minv = _b;
                if (_c < minv) minv = _c;
                if (_b > maxv) maxv = _b;
                if (_c > maxv) maxv = _c;
                min[i] = minv;
                max[i] = maxv;
            }
        }

        //所在平面 
        public LVector n;
        public LFloat d;

        //bound box
        public Point min;
        public Point max;
    }
}