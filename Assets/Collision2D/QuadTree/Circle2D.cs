using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct Circle2D {
        public int TypeId;
        public int Id;
        public LVector2 pos;
        public LFloat radius;
        public QuadTree* ParentNode;
        public int _debugId;

        public LFloat SqrRadius => radius * radius;
        public Circle2D(int typeid, int id, LVector2 pos, LFloat radius){
            this.TypeId = typeid;
            this.Id = id;
            this.pos = pos;
            this.radius = radius;
            ParentNode = null;
            _debugId = 0;
        }
        public Circle2D( int id, LVector2 pos, LFloat radius):this((int) EColliderType2D.Circle,id,pos,radius){}
    }
}