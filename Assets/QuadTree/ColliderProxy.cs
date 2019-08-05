using Lockstep.Collision2D;
using Lockstep.Math;
using UnityEngine;

namespace TQuadTree1 {
    public delegate void FuncOnTriggerEvent(ColliderProxy other, ECollisionEvent type);

    public class ColliderProxy {
#if UNITY_EDITOR
        public Transform UnityTransform;
#endif
        public uint Id;
        public int LayerType { get; set; }
        public ColliderPrefab Prefab;
        public CTransform2D Transform2D;
        public LFloat Height;
        public bool IsTrigger = true;
        public bool IsStatic = false;


        private Rect _bound;

        public FuncOnTriggerEvent OnTriggerEvent;

        private BoundsQuadTree _quadTree;

        private static uint autoIncId = 0;

        public void Init(ColliderPrefab prefab, LVector2 pos, LFloat y){
            Init(prefab, pos, y, LFloat.zero);
        }

        public void Init(ColliderPrefab prefab, LVector2 pos){
            Init(prefab, pos, LFloat.zero, LFloat.zero);
        }

        public void Init(ColliderPrefab prefab, LVector2 pos, LFloat y, LFloat deg){
            this.Prefab = prefab;
            _bound = prefab.GetBounds();
            Transform2D = new CTransform2D(pos, y, deg);
            unchecked {
                Id = autoIncId++;
            }
        }

        public bool _isMoved = true;

        public LVector2 pos {
            get => Transform2D.pos;
            set {
                _isMoved = true;
                Transform2D.pos = value;
            }
        }

        public LFloat y {
            get => Transform2D.y;
            set {
                _isMoved = true;
                Transform2D.y = value;
            }
        }

        public LFloat deg {
            get => Transform2D.deg;
            set {
                _isMoved = true;
                Transform2D.deg = value;
            }
        }


        public Rect GetBounds(){
            return new Rect(_bound.position + pos.ToVector2(), _bound.size);
        }

        public virtual void OnTriggerEnter(ColliderProxy other){ }
        public virtual void OnTriggerStay(ColliderProxy other){ }
        public virtual void OnTriggerExit(ColliderProxy other){ }
        public virtual void OnCollisionEnter(ColliderProxy other){ }
        public virtual void OnCollisionStay(ColliderProxy other){ }
        public virtual void OnCollisionExit(ColliderProxy other){ }
    }
}