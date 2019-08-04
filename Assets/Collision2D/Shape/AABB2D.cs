using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct AABB2D :IShape2D{
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

        public LVector2 size;

        public AABB2D(int id, LVector2 pos, LVector2 size){
            BoundSphere = new Circle((int) EShape2D.AABB, id, pos, size.magnitude);
            this.size = size;
        }
        public void UpdatePosition(LVector2 pos){this.pos = pos;}
        public void UpdateRotation(LFloat deg){}
    }
}