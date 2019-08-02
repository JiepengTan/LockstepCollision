using UnityEngine.Profiling;

namespace Lockstep.Collision2D {
    /// <summary>
    /// Queries a QuadTree to test for collisions with only nearby bodies
    /// </summary>
    public unsafe class CollisionSystemQuadTree : CollisionSystem {
        ///// Constructor /////

        public CollisionSystemQuadTree(QuadTree* tree){
            _quadTree = tree;
        }

        ///// Fields /////

        private QuadTree* _quadTree;

        ///// Methods /////


        public override void DetectBodyVsBody(){
            countDetectBodyVsBody = 0;
            IteratePtrs(OnShapePtr);
        }

        void OnShapePtr(Circle2D* body1){
            countDetectBodyVsBody++;
            Profiler.BeginSample("GetBodies");
            var bodies = _quadTree->GetBodies(new LRect(body1->pos,((AABB2D*)body1)->size));
            Profiler.EndSample();
            Profiler.BeginSample("Test");
            var count = bodies.Count;
            for (int i = 0; i < count; i++) {
                var body2 = (Circle2D*) bodies[i];
                if (body2 == null || body1 == body2) {
                    continue;
                }

                Test(body1, body2);
            }
            Profiler.EndSample();
        }
    }
}