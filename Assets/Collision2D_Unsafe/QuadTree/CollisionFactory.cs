using System.Collections.Generic;

namespace Lockstep.UnsafeCollision2D {
    public unsafe class CollisionFactory {
        private static NativePool _spherePool = NativeFactory.GetPool(sizeof(Circle));
        private static NativePool _aabbPool = NativeFactory.GetPool(sizeof(AABB2D));

        public static Circle* AllocSphere(){
            return (Circle*) _spherePool.ForceGet();
        }

        public static AABB2D* AllocAABB(){
            return (AABB2D*) _aabbPool.ForceGet();
        }

        public static void Free(Circle* ptr){
            _spherePool.Return(ptr);
        }

        public static void Free(AABB2D* ptr){
            _aabbPool.Return(ptr);
        }
        
    }
}