using Lockstep.Collision2D;

namespace TQuadTree1 {


    public partial class CollisionHelper {
        public enum ECollisionType {
            Seg_Seg         = 0x00,
            Seg_Ray         = 0x01,
            Seg_Circle      = 0x02,
            Seg_AABB        = 0x03,
            Seg_OBB         = 0x04,
            Seg_Polygon     = 0x05,
               
            Ray_Ray         = 0x11,
            Ray_Circle      = 0x12,
            Ray_AABB        = 0x13,
            Ray_OBB         = 0x14,
            Ray_Polygon     = 0x15,
            
            Circle_Circle   = 0x22,
            Circle_AABB     = 0x23 ,
            Circle_OBB      = 0x24,
            Circle_Polygon  = 0x25,
            
            AABB_AABB       = 0x33  ,
            AABB_OBB        = 0x34 ,
            AABB_Polygon    = 0x35 ,
            
            OBB_OBB          = 0x44  ,
            OBB_Polygon      = 0x45      ,
            
            Polygon_Polygon  = 0x55
        }

        public delegate bool FuncCollisionDetected(CBaseShape col1, Transform2D trans1, CBaseShape col2,Transform2D trans2);
        static FuncCollisionDetected[] _dealFuncs = new FuncCollisionDetected[(int)ECollisionType.Polygon_Polygon +1];
        static CollisionHelper(){
            _dealFuncs[(int)ECollisionType.Seg_Seg        ] = (col1, trans1, col2,trans2)=>CheckSeg_Seg        ((CSegment)col1,(CSegment )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Seg_Ray        ] = (col1, trans1, col2,trans2)=>CheckSeg_Ray        ((CSegment)col1,(CRay     )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Seg_Circle     ] = (col1, trans1, col2,trans2)=>CheckSeg_Circle     ((CSegment)col1,(CCircle  )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Seg_AABB       ] = (col1, trans1, col2,trans2)=>CheckSeg_AABB       ((CSegment)col1,(CAABB    )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Seg_OBB        ] = (col1, trans1, col2,trans2)=>CheckSeg_OBB        ((CSegment)col1,(COBB     )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Seg_Polygon    ] = (col1, trans1, col2,trans2)=>CheckSeg_Polygon    ((CSegment)col1,(CPolygon )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Ray_Ray        ] = (col1, trans1, col2,trans2)=>CheckRay_Ray        ((CRay    )col1,(CRay     )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Ray_Circle     ] = (col1, trans1, col2,trans2)=>CheckRay_Circle     ((CRay    )col1,(CCircle  )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Ray_AABB       ] = (col1, trans1, col2,trans2)=>CheckRay_AABB       ((CRay    )col1,(CAABB    )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Ray_OBB        ] = (col1, trans1, col2,trans2)=>CheckRay_OBB        ((CRay    )col1,(COBB     )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Ray_Polygon    ] = (col1, trans1, col2,trans2)=>CheckRay_Polygon    ((CRay    )col1,(CPolygon )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Circle_Circle  ] = (col1, trans1, col2,trans2)=>CheckCircle_Circle  ((CCircle )col1,(CCircle  )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Circle_AABB    ] = (col1, trans1, col2,trans2)=>CheckCircle_AABB    ((CCircle )col1,(CAABB    )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Circle_OBB     ] = (col1, trans1, col2,trans2)=>CheckCircle_OBB     ((CCircle )col1,(COBB     )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Circle_Polygon ] = (col1, trans1, col2,trans2)=>CheckCircle_Polygon ((CCircle )col1,(CPolygon )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.AABB_AABB      ] = (col1, trans1, col2,trans2)=>CheckAABB_AABB      ((CAABB   )col1,(CAABB    )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.AABB_OBB       ] = (col1, trans1, col2,trans2)=>CheckAABB_OBB       ((CAABB   )col1,(COBB     )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.AABB_Polygon   ] = (col1, trans1, col2,trans2)=>CheckAABB_Polygon   ((CAABB   )col1,(CPolygon )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.OBB_OBB        ] = (col1, trans1, col2,trans2)=>CheckOBB_OBB        ((COBB    )col1,(COBB     )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.OBB_Polygon    ] = (col1, trans1, col2,trans2)=>CheckOBB_Polygon    ((COBB    )col1,(CPolygon )col2,trans1,trans2);
            _dealFuncs[(int)ECollisionType.Polygon_Polygon] = (col1, trans1, col2,trans2)=>CheckPolygon_Polygon((CPolygon)col1,(CPolygon )col2,trans1,trans2);
        }


        private static bool CheckCollision(CBaseShape col1, Transform2D trans1, CBaseShape col2,
            Transform2D trans2){
            if (col1.TypeId > col2.TypeId) {
                return CheckCollision(col2,trans2,col1,trans1);
            }
            var id = col1.TypeId | col2.TypeId;
            if (id > _dealFuncs.Length)
                return false;
            var func = _dealFuncs[id];
            if(func!= null){
                return   func(col1, trans1, col2,trans2);
            }
            return false;
        }
        public static bool CheckCollision(ColliderPrefab col1, CTransform2D trans1, ColliderPrefab col2,
            CTransform2D trans2){
            foreach (var part1 in col1.parts) {
                foreach (var part2 in col2.parts) {
                    if (CollisionHelper.CheckCollision(part1.collider, trans1 + part1.transform, part2.collider, trans2 + part2.transform)) {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}