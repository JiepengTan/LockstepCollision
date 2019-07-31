using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct AABB2D {
        public Sphere2D BoundSphere;

        public int TypeId {
            get => BoundSphere.TypeId;
            set => BoundSphere.TypeId = value;
        }

        public int Id {
            get => BoundSphere.Id;
            set => BoundSphere.Id = value;
        }

        public LVector2 Pos {
            get => BoundSphere.Pos;
            set => BoundSphere.Pos = value;
        }

        public LFloat Radius {
            get => BoundSphere.Radius;
            set => BoundSphere.Radius = value;
        }

        public LVector2 Extents;

        public AABB2D( int id, LVector2 pos, LVector2 extents){
            BoundSphere = new Sphere2D((int) EColliderType2D.AABB2D, id, pos, extents.magnitude);
            this.Extents = extents;
        }
    }
}