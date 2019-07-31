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
            IteratePtrs((body1) => {
                var bodies = _quadTree->GetBodies(body1->Pos, body1->Radius);
                for (int i = 0; i < bodies.Count; i++) {
                    var body2 = (Sphere2D*) bodies[i].value;
                    if (body2 == null || body1 == body2) {
                        continue;
                    }
                    Test(body1, body2);
                }
            });
        }
    }
}