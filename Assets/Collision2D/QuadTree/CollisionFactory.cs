using System.Collections.Generic;

namespace Lockstep.Collision2D {
    public unsafe class CollisionFactory {
        private static NativePool _spherePool = NativeFactory.GetPool(sizeof(Circle2D));
        private static NativePool _aabbPool = NativeFactory.GetPool(sizeof(AABB2D));

        public static Circle2D* AllocSphere(){
            return (Circle2D*) _spherePool.ForceGet();
        }

        public static AABB2D* AllocAABB(){
            return (AABB2D*) _aabbPool.ForceGet();
        }

        public static void Free(Circle2D* ptr){
            _spherePool.Return(ptr);
        }

        public static void Free(AABB2D* ptr){
            _aabbPool.Return(ptr);
        }
        
    }
}