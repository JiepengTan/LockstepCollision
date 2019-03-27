using LockStepMath;
using UnityEngine;

namespace LockStepCollision
{
    public static class VectorExtension
    {
        public static LFloat ToLFloat(this float self)
        {
            return new LFloat((int)(self * LFloat.Precision));
        }

        public static LVector ToLVector(this Vector3 self)
        {
            return new LVector(self.x.ToLFloat(),self.y.ToLFloat(),self.z.ToLFloat());
        }
    }
}