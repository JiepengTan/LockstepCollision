using System.Runtime.InteropServices;
using Lockstep.Collision2D;
using Lockstep.Math;

namespace Lockstep.UnsafeCollision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct Ray2D {
        public int _TypeId;
        public int TypeId => _TypeId;
        public int Id;
        public LVector2 pos;
        public LVector2 dir;
        public Ray2D( int id, LVector2 pos, LVector2 dir){
            this._TypeId = (int) EShape2D.Ray;
            this.Id = id;
            this.pos = pos;
            this.dir = dir;
        }
        public void UpdatePosition(LVector2 pos){this.pos = pos;}
        public void UpdateRotation(LFloat deg){}
    }
}