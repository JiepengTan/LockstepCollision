using Lockstep.Math;

namespace Lockstep.Collision2D {
    public interface IShape2D {
        int TypeId { get; }
        void UpdatePosition(LVector2 npos);
        void UpdateRotation(LFloat deg);
    }
}