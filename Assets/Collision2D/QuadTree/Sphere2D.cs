using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct Sphere2D {
        public int TypeId;
        public int Id;
        public LVector2 Pos;
        public LFloat Radius;
        public QuadTree* ParentNode;

        public Sphere2D(int typeid, int id, LVector2 pos, LFloat radius){
            this.TypeId = typeid;
            this.Id = id;
            this.Pos = pos;
            this.Radius = radius;
            ParentNode = null;
        }

        public bool TestCollision(Sphere2D* shapePtr){
            return true;
        }
    }
}