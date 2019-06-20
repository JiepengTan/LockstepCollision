using Lockstep.Math;
using LockStepLMath;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;

namespace Lockstep.Collision {
    [System.Serializable]
    public partial class Capsule : BaseShape {
        public override EColType ColType => EColType.Capsule;

        public Capsule(LVector3 c, LVector3 hDir, LFloat r) : base(){
            this.c = c;
            this.hDir = hDir;
            this.r = r;
        }

        /// <summary>
        /// Medial line segment start point
        /// </summary>
        public LVector3 a => c - _hDir;

        /// <summary>
        /// Medial line segment end point
        /// </summary>
        public LVector3 b => c + _hDir;


        private LFloat _hLen;

        public LFloat hLen {
            get => _hLen;
            set {
                _hLen = value;
                _hDir = value * _hDir.normalized;
            }
        }

        private LVector3 _hDir;

        public LVector3 hDir {
            get => _hDir;
            set {
                _hDir = value;
                _hLen = value.magnitude;
            }
        }

        /// <summary>
        /// Center
        /// </summary>
        public LVector3 c;

        /// <summary>
        /// Radius
        /// </summary>
        public LFloat r;

        public LFloat GetBoundsSphereRadius(){
            return _hLen + r;
        }

        public LVector3 GetBoundSphereCenter(){
            return c;
        }

        public override Sphere GetBoundSphere(){
            return new Sphere(c, _hLen + r);
        }

        public override void UpdateRotation(LVector3 forward, LVector3 up){
            _hDir = up * _hLen;
        }

        public override void UpdatePosition(LVector3 targetPos){
            c = targetPos;
        }

        public override bool TestWithShape(BaseShape shape){
            return shape.TestWith(this);
        }


        public override bool TestWith(Sphere sphere){
            return Utils.TestSphereCapsule(sphere, this);
        }

        public override bool TestWith(AABB aabb){
            return Utils.TestAABBCapsule(aabb, this);
        }

        public override bool TestWith(Capsule capsule){
            return Utils.TestCapsuleCapsule(this, capsule);
        }

        public override bool TestWith(OBB obb){
            return Utils.TestOBBCapsule(obb, this);
        }

        public override bool TestWith(Plane plane){
            return Utils.TestCapsulePlane(this, plane);
        }

        public override bool TestWith(Ray ray){
            return Utils.IntersectRayCapsule(ray.o, ray.d, a, b, r, out LFloat t);
        }
    };
}