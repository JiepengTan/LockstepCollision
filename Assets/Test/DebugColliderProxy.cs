using System.Collections.Generic;
using UnityEngine;

using LockStepCollision;
using LockStepMath;
using Collision = LockStepCollision.Collision;

namespace Test
{
    public class DebugColliderProxy : MonoBehaviour
    {
        public static List<DebugColliderProxy> allProxys = new List<DebugColliderProxy>();
        public List<BaseShape> allColliders = new List<BaseShape>();
        public Sphere boundSphere;
        public Material mat;
        private void Start()
        {
            allProxys.Add(this);
            boundSphere = new Sphere(this.transform.position.ToLVector(),1.0f.ToLFloat());
            mat =new Material(GetComponent<Renderer>().material);
            GetComponent<Renderer>().material = mat;
        }

        private void OnDrawGizmos()
        {
            foreach (var col in allColliders)
            {
                col.OnDrawGizmos(false);
            }
        }

        private void Update()
        {
       
            bool hasCollidedOthers = false;
            //TODO 改为更加 性能友好的判定方式
            foreach (var col in allProxys)
            {
                if(col != this)
                {
                    var isCollided = Collision.TestSphereSphere(this.boundSphere, col.boundSphere);
                    if (isCollided)
                    {
                        hasCollidedOthers = true;
                        break;
                    }
                }
            }
            mat.color = hasCollidedOthers ? Color.red : Color.white;
        }
        public void UpdatePosition(Vector3 position)
        {
        }

        public void UpdateRotation(Quaternion rotation)
        {
        }
        

        public void AddCollider(GameObject obj,PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.Cube:
                {
                    var _col = new AABB();
                    _col.min = (obj.transform.position - Vector3.one * 0.5f).ToLVector();
                    _col.max = (obj.transform.position + Vector3.one * 0.5f).ToLVector();
                    AddCollider(_col); break;
                }
                case PrimitiveType.Sphere:
                {
                    var _col = new Sphere(obj.transform.position.ToLVector(),1.0f.ToLFloat());
                    AddCollider(_col); break;
                }
                case PrimitiveType.Capsule:
                {
                    var _col = new Capsule();
                    _col.a = (obj.transform.position - Vector3.one * 0.5f).ToLVector();
                    _col.b = (obj.transform.position + Vector3.one * 0.5f).ToLVector();
                    _col.r = 0.5f.ToLFloat();
                    AddCollider(_col); break;
                }
            }
        }

        public void AddCollider(BaseShape shape)
        {
            Debug.Assert(shape!= null);
            allColliders.Add(shape);
        }
    }
}