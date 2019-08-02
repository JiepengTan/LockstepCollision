using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct AABB2D {
        public Circle2D BoundSphere;

        public int TypeId {
            get => BoundSphere.TypeId;
            set => BoundSphere.TypeId = value;
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
            BoundSphere = new Circle2D((int) EColliderType2D.AABB, id, pos, size.magnitude);
            this.size = size;
        }
    }
}