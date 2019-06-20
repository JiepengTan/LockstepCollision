using System.Numerics;
using System.Runtime.CompilerServices;
using Lockstep.Math;
using UnityEditor;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision {
    [System.Serializable]
    public partial class AABB : BaseShape {
        public override EColType ColType {
            get { return EColType.AABB; }
        }

        public LVector3 min;
        public LVector3 max;

        /// <summary>
        /// center point of AABB
        /// </summary>
        public LVector3 c {
            get { return (max + min) * LFloat.half; }
        }

        /// <summary>
        /// radius or halfwidth extents
        /// </summary>
        public LVector3 r {
            get { return (max - min) * LFloat.half; }
        }

        public AABB(LVector3 min, LVector3 max){
            this.min = min;
            this.max = max;
        }

        public AABB(AABB o){
            this.min = o.min;
            this.max = o.max;
        }


        public AABB(){ }

        public static AABB FromOBB(OBB obb){
            var aabb = new AABB();
            var abse = obb.e.abs;
            aabb.min = obb.c - abse;
            aabb.max = obb.c + abse;
            return aabb;
        }

        public OBB ToOBB(){
            var obb = new OBB();
            obb.c = c;
            obb.e = r;
            obb.u = LAxis3D.identity;
            return obb;
        }

        public LFloat SurfaceArea(){
            LVector3 d = min - min;
            return (d.x * d.y + d.x * d.z + d.y * d.z) * 2;
        }

        public int MaximumExtent(){
            var diag = min - min;
            if (diag.x > diag.y && diag.x > diag.z)
                return 0;
            else if (diag.y > diag.z)
                return 1;
            else
                return 2;
        }

        public LVector3 this[int index] {
            get {
                if (index == 0) return min;
                else return max;
            }
            set {
                if (index == 0) min = value;
                else max = value;
            }
        }

        public override void UpdatePosition(LVector3 targetPos){
            var oldR = r;
            min = targetPos - oldR;
            max = targetPos + oldR;
        }
        // Transform AABB a by the matrix m and translation t,
        // find maximum extents, and store result into AABB b.
        public void UpdateAABB(LMatrix33 m, LVector3 t){
            LVector3 _c = c + t;
            LVector3 _r = r;
            min = max = _c;
            // For all three axes
            for (int i = 0; i < 3; i++) {
                // Form extent by summing smaller and larger terms respectively
                for (int j = 0; j < 3; j++) {
                    LFloat e = m[i, j] * _r[j];
                    if (e < LFloat.zero) {
                        min[i] += e;
                        max[i] -= e;
                    }
                    else {
                        min[i] -= e;
                        max[i] += e;
                    }
                }
            }
        }

        public override Sphere GetBoundSphere(){
            return new Sphere(c, (max - min).magnitude * LFloat.half);
        }

        public override bool TestWithShape(BaseShape shape){
            return shape.TestWith(this);
        }


        public override bool TestWith(Sphere sphere){
            return Utils.TestSphereAABB(sphere, this);
        }

        public override bool TestWith(AABB aabb){
            return Utils.TestAABBAABB(aabb, this);
        }

        public override bool TestWith(Capsule capsule){
            return Utils.TestAABBCapsule(this, capsule);
        }

        public override bool TestWith(OBB obb){
            return Utils.TestOBBOBB(obb, this.ToOBB());
        }

        public override bool TestWith(Plane plane){
            return Utils.TestAABBPlane(this, plane);
        }

        public override bool TestWith(Ray ray){
            return Utils.IntersectRayAABB(ray.o, ray.d, this, out LFloat tmin, out LVector3 temp);
        }
    }
}