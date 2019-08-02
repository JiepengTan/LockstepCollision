using System;
using System.Collections.Generic;
using Lockstep.Math;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif
namespace Lockstep.Collision2D {
    public class ColliderProxy2D
#if UNITY_5_3_OR_NEWER
        : MonoBehaviour 
#endif
    {
        public LVector2 pos;
        public LFloat deg;

        public List<LVector2> allOffsets = new List<LVector2>();
        public List<BaseShaper2D> allColliders = new List<BaseShaper2D>();

        public BaseShaper2D Collider {
            get {
                if (allColliders.Count == 0) return null;
                return allColliders[0];
            }
        }

        //add
        public void AddCircle(LVector2 offset, LVector2 pos, LFloat radius){
            allColliders.Add(new Circle(pos, radius));
            allOffsets.Add(offset);
        }

        public void AddAABB(LVector2 offset, LVector2 pos, LVector2 size){
            allColliders.Add(new AABB(pos, size));
            allOffsets.Add(offset);
        }

        public void AddOBB(LVector2 offset, LVector2 pos, LVector2 size, LFloat deg){
            allColliders.Add(new OBB2D(pos, size, deg));
            allOffsets.Add(offset);
        }

        //delete
        public void Remove(int idx){
            Debug.Assert(idx < allOffsets.Count, "out of range");
            if (idx >= allOffsets.Count) return;
            allColliders.RemoveAt(idx);
            allOffsets.RemoveAt(idx);
        }

        public void Remove(BaseShaper2D shape){
            var idx = allColliders.IndexOf(shape);
            if (idx != -1) {
                Remove(idx);
            }
        }

        //modify
        public void ModifyOffset(int idx, LVector2 offset){
            //Debug.Assert(idx < allOffsets.Count, "out of range");
            if (idx >= allOffsets.Count) return;
            allOffsets[idx] = offset;
        }


        public void SetPosition(LVector3 val){
            transform.position = val.ToVector3();
            OnSetPosition(new LVector2(val.x, val.y));
        }

        public void SetRotation(LFloat val){
            transform.rotation = Quaternion.Euler(0, 0, val.ToFloat());
            OnSetRotation(val);
        }
        [Obsolete("use SetRotation(LFloat val)")]
        public void SetRotation(Quaternion rotation){
            transform.rotation = rotation;
            OnSetRotation(rotation.eulerAngles.z.ToLFloat());
        }

        public void Update(){
            var pos = transform.position.ToLVector3();
            var deg = transform.rotation.eulerAngles.z.ToLFloat();
            OnSetPosition(new LVector2(pos.x, pos.y));
            OnSetRotation(deg);
        }

        //tranform values
        protected void OnSetPosition(LVector2 val){
            if (pos != val) {
                pos = val;
                var count = allColliders.Count;
                for (int i = 0; i < count; i++) {
                    allColliders[i].UpdatePosition(pos + allOffsets[i]);
                }
            }
        }

        protected void OnSetRotation(LFloat val){
            if (deg != val) {
                deg = val;
                var count = allColliders.Count;
                for (int i = 0; i < count; i++) {
                    allColliders[i].UpdateRotation(deg);
                }
            }
        }
        
    }
}