using Lockstep.Math;
using Lockstep.UnsafeCollision2D;

namespace Lockstep.Collision2D {
    public class CRay : CBaseShape {
        public override int TypeId => (int) EShape2D.Ray;
        public LVector2 pos;
        public LVector2 dir;
    }
}