namespace Lockstep.Collision2D {
    public unsafe class QuadTreeFactory {
        private static NativePool _treePool = NativeFactory.GetPool(sizeof(QuadTree));

        public static Sphere2D** AllocPtrBlock(int size){
            return (Sphere2D**) NativeFactory.GetPool(sizeof(void*) * size).ForceGet();
        }

        public static void FreePtrBlock(void* ptr, int size){
            NativeFactory.GetPool(sizeof(void*) * size).Return(ptr);
        }

        public static QuadTree* AllocQuadTree(){
            return (QuadTree*) _treePool.ForceGet();
        }

        public static void FreeQuadTree(QuadTree* treePtr){
            _treePool.Return(treePtr);
        }
    }
}