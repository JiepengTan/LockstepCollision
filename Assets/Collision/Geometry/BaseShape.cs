using Lockstep.Math;
using System;
using System.Security.Cryptography.X509Certificates;
using LockStepLMath;

namespace Lockstep.Collision
{
    public enum EColType
    {
        Sphere,
        AABB,
        Capsule,
        Cylinder,
        OBB,
        ColMesh,
        Triangle,
        Plane,
        Ray,
        Rect,
        Segment,
        Polygon,
        EnumCount,
    }
    [System.Serializable]
    public partial class BaseShape
    {
        /// <summary>
        /// 碰撞类型
        /// </summary>
        public virtual EColType ColType
        {
            get { return EColType.EnumCount; }
        }

        public virtual AABB GetBound(){ return null;}

        public virtual Sphere GetBoundSphere()
        {
            return null;
        }

        public virtual void UpdatePosition(LVector3 targetPos)
        {
            
        }
        public virtual void UpdateRotation(LVector3 forward, LVector3 up)
        {
            //throw new NotImplementedException(GetType() + "Can not rotate ");
        }
        public static bool TestShapeWithShape(BaseShape a, BaseShape b)
        {
            if (a == null || b == null)
                return false;
            return a.TestWithShape(b);
        }

        #region TestInterfaces

        public virtual bool TestWithShape(BaseShape shape)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithShape");
        }

        public virtual bool TestWith(Sphere sphere)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithSphere");
        }
        public virtual bool TestWith(AABB aabb)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithAABB");
        }
        public virtual bool TestWith(Capsule capsule)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithCapsule");
        }

        public virtual bool TestWith(OBB obb)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithOBB");
        }
        
        
        public virtual bool TestWith(Plane plane)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithPlane");
        }

        public virtual bool TestWith(Ray ray)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithRay");
        }
        public virtual bool TestWith(Ray ray,ref LFloat tmin)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithRay");
        }
        
        
        public virtual bool TestWith(Cylinder cylinder)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithCylinder");
        }


        public virtual bool TestWith(ColMesh mesh)
        {
            throw new System.NotImplementedException(GetType() + " not implement this TestWithMesh");
        }
        #endregion
    }
}