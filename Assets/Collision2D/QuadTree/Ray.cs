using System.Runtime.InteropServices;
using Lockstep.Math;

namespace Lockstep.Collision2D {
    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct Ray {
        public int TypeId;
        public int Id;
        public LVector2 pos;
        public LVector2 dir;
        public Ray( int id, LVector2 pos, LVector2 dir){
            this.TypeId = (int) EColliderType2D.Ray;
            this.Id = id;
            this.pos = pos;
            this.dir = dir;
        }
    }
}