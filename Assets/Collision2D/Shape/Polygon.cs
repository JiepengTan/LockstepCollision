using System;
using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct Polygon : IShape2D {
        public Circle BoundSphere;

        public int TypeId {
            get => BoundSphere.TypeId;
        }

        public int Id {
            get => BoundSphere.Id;
            set => BoundSphere.Id = value;
        }

        public LVector2 pos {
            get => BoundSphere.pos;
            set => BoundSphere.pos = value;
        }

        public LFloat radius {
            get => BoundSphere.radius;
            set => BoundSphere.radius = value;
        }

        public LVector2* vertexes;
        public int vertexCount;
        public LFloat deg;
        private int vertexCapcity;

        public void UpdatePosition(LVector2 pos){
            this.pos = pos;
        }

        public void UpdateRotation(LFloat deg){
            this.deg = deg; //TODO
        }

        public void AddPoint(LVector2 point){
            const int initSize = 4;
            if (vertexCount == 0) {
                vertexes = (LVector2*) NativeFactory.GetPool(sizeof(LVector2) * initSize).Get();
                vertexCapcity = initSize;
            }

            if (vertexCount == vertexCapcity) {
                var newMem = (LVector2*) NativeFactory.GetPool(sizeof(LVector2) * vertexCapcity * 2).Get();
                NativeHelper.Copy(newMem, vertexes, sizeof(LVector2) * vertexCount);
                NativeFactory.GetPool(sizeof(LVector2) * vertexCapcity).Return(vertexes);
                vertexes = newMem;
                vertexCapcity = vertexCapcity * 2;
            }

            vertexes[vertexCount++] = point;
        }

        public bool RemovePoint(int idx){
            if (idx < 0 || idx >= vertexCount) {
                return false;
            }

            if (idx < vertexCount - 1) {
                var srcOffset = (byte*) (&vertexes[idx + 1]);
                var dstOffset = (byte*) (&vertexes[idx]);
                NativeHelper.Copy(dstOffset, srcOffset, sizeof(LVector2) * (vertexCount - idx - 1));
            }

            vertexCount--;
            return true;
        }

        public void OnRelease(){
            if (vertexes != null) {
                NativeHelper.Free((IntPtr) vertexes);
            }
        }

        public Polygon(int id, LVector2* vertexes, int vertexCount){
            var cpos = LVector2.one;
            var r = LFloat.zero;
            CalcCenterAndRadius(vertexes, vertexCount, ref cpos, ref r);
            BoundSphere = new Circle((int) EShape2D.Polygon, id, cpos, r);
            this.vertexes = vertexes;
            this.vertexCount = vertexCount;
            deg = LFloat.zero;
            vertexCapcity = 0;
        }


        static void CalcCenterAndRadius(LVector2* vertexs, int count, ref LVector2 center, ref LFloat radius){
            var minSqrRadius = LFloat.FLT_MAX;
            for (int i = 0; i < count; i++) {
                for (int j = i + 1; j < count; j++) {
                    var cpos = (vertexs[i] + vertexs[j]) * LFloat.half;
                    var sqrRadius = ((vertexs[i] - vertexs[j]) * LFloat.half).sqrMagnitude;
                    if (sqrRadius < minSqrRadius) {
                        var containAll = CanContainAllPoints(vertexs, count, cpos, sqrRadius);
                        if (containAll) {
                            minSqrRadius = sqrRadius;
                            center = cpos;
                        }
                    }
                }
            }

            for (int i = 0; i < count; i++) {
                for (int j = i + 1; j < count; j++) {
                    for (int k = j + 1; k < count; k++) {
                        var cpos = CalcCentroid(vertexs[i], vertexs[j], vertexs[k]);
                        var sqrRadius = (cpos - vertexs[i]).sqrMagnitude;
                        if (sqrRadius < minSqrRadius) {
                            var containAll = CanContainAllPoints(vertexs, count, cpos, sqrRadius);
                            if (containAll) {
                                minSqrRadius = sqrRadius;
                                center = cpos;
                            }
                        }
                    }
                }
            }

            radius = LMath.Sqrt(minSqrRadius);
        }


        static LVector2 CalcCentroid(LVector2 p1, LVector2 p2, LVector2 p3){
            LVector2 p;
            var x1 = p1.x;
            var y1 = p1.y;
            var x2 = p2.x;
            var y2 = p2.y;
            var x3 = p3.x;
            var y3 = p3.y;
            var a1 = 2 * (x2 - x1);
            var b1 = 2 * (y2 - y1);
            var c1 = x2 * x2 + y2 * y2 - x1 * x1 - y1 * y1;

            var a2 = 2 * (x3 - x2);
            var b2 = 2 * (y3 - y2);
            var c2 = x3 * x3 + y3 * y3 - x2 * x2 - y2 * y2;
            var demo = (b1 * a2 - b2 * a1);
            var x = (b1 * c2 - b2 * c1) / demo;
            var y = (a2 * c1 - a1 * c2) / demo;
            return new LVector2(x, y);
        }

        static bool CanContainAllPoints(LVector2* vertexs, int count, LVector2 center, LFloat sqrRadius){
            for (int i = 0; i < count; i++) {
                var diff = center - vertexs[i];
                if (diff.sqrMagnitude > sqrRadius) {
                    return false;
                }
            }

            return true;
        }
    }
}