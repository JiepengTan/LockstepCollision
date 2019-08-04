using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct Circle :IShape2D{
        public int _TypeId;
        public int TypeId => _TypeId;
        public int Id;
        public LVector2 pos;
        public LFloat radius;
        public QuadTree* ParentNode;
        public int _debugId;

        public LFloat SqrRadius => radius * radius;
        public Circle(int typeid, int id, LVector2 pos, LFloat radius){
            this._TypeId = typeid;
            this.Id = id;
            this.pos = pos;
            this.radius = radius;
            ParentNode = null;
            _debugId = 0;
        }
        public Circle( int id, LVector2 pos, LFloat radius):this((int) EShape2D.Circle,id,pos,radius){}

        public void UpdatePosition(LVector2 pos){this.pos = pos;}
        public void UpdateRotation(LFloat deg){}
    }
}