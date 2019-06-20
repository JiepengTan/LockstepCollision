using System.Collections.Generic;
using Lockstep.Math;
#if UNITY_5_3_OR_NEWER
using UnityEngine;

#endif

namespace Lockstep.Collision {
    [System.Serializable]
    public struct ColliderLocalInfo {
        public LVector3 offset;
        public LVector3 rotation;
        public static readonly ColliderLocalInfo identity = new ColliderLocalInfo(LVector3.zero, LVector3.zero);

        public ColliderLocalInfo(LVector3 offset, LVector3 rotation){
            this.offset = offset;
            this.rotation = rotation;
        }
    }

    public class ColliderProxy
#if UNITY_5_3_OR_NEWER
        : MonoBehaviour {
#else
    {
#endif
        public List<BaseShape> allCollider = new List<BaseShape>();
        public List<ColliderLocalInfo> allColliderOffset = new List<ColliderLocalInfo>();
        public Sphere boundSphere;

        public BaseShape Collider => allCollider?[0];
        
        /// <summary>
        /// 获取所有的Shape 的总boundSphere
        /// </summary>
        /// <returns></returns>
        public virtual Sphere GetBoundSphere(){
            Sphere retS = null;
            foreach (var col in allCollider) {
                var tempS = col.GetBoundSphere();
                if (retS == null) {
                    retS = tempS;
                }
                else {
                    var rt = (tempS.c - retS.c);
                    var sqrCenterDist = rt.sqrMagnitude;
                    var rDist = retS.r + tempS.r;
                    if (rDist * rDist <= sqrCenterDist) //separatie
                    {
                        var centerDist = LMath.Sqrt(sqrCenterDist);
                        var r = (centerDist + rDist) * LFloat.half;
                        var c = retS.c + rt.normalized * (r - retS.r);
                        retS.c = c;
                        retS.r = r;
                    }
                    else {
                        var rdiff = LMath.Abs(retS.r - tempS.r);
                        if (rdiff <= sqrCenterDist) //one contains another
                        {
                            if (retS.r < tempS.r) {
                                retS.c = tempS.c;
                                retS.r = tempS.r;
                            }
                        }
                        else //intersect
                        {
                            var centerDist = LMath.Sqrt(sqrCenterDist);
                            var r = (centerDist + rDist) * LFloat.half;
                            var c = retS.c + rt.normalized * (r - retS.r);
                            retS.c = c;
                            retS.r = r;
                        }
                    }
                }
            }

            return retS;
        }

        public void UpdatePosition(LVector3 pos){
            boundSphere.c = pos;
            foreach (var col in allCollider) {
                col.UpdatePosition(pos);
            }
        }

        public void UpdateRotation(LVector3 forward, LVector3 up){
            foreach (var col in allCollider) {
                col.UpdateRotation(forward, up);
            }
        }

        public void AddCollider(BaseShape shape, ColliderLocalInfo localInfo){
            allCollider.Add(shape);
            allColliderOffset.Add(localInfo);
            boundSphere = GetBoundSphere();
        }

        public static bool TestColliderProxy(ColliderProxy a, ColliderProxy b){
            //TODO use better ways
            var isCollided = Utils.TestSphereSphere(a.boundSphere, b.boundSphere);
            if (isCollided) {
                foreach (var cCola in a.allCollider) {
                    foreach (var cColb in b.allCollider) {
                        if (BaseShape.TestShapeWithShape(cCola, cColb)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}