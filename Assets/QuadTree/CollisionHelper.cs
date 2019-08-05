using Lockstep.Collision2D;

namespace TQuadTree1 {
    public partial class CollisionHelper {
        
        private static bool CheckSeg_Seg        (CSegment col1,    CSegment  col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckSeg_Ray        (CSegment col1,    CRay      col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckSeg_Circle     (CSegment col1,    CCircle   col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckSeg_AABB       (CSegment col1,    CAABB     col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckSeg_OBB        (CSegment col1,    COBB      col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckSeg_Polygon    (CSegment col1,    CPolygon  col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckRay_Ray        (CRay     col1,    CRay      col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckRay_Circle     (CRay     col1,    CCircle   col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckRay_AABB       (CRay     col1,    CAABB     col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckRay_OBB        (CRay     col1,    COBB      col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckRay_Polygon    (CRay     col1,    CPolygon  col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckCircle_Circle  (CCircle  col1,    CCircle   col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckCircle_AABB    (CCircle  col1,    CAABB     col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckCircle_OBB     (CCircle  col1,    COBB      col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckCircle_Polygon (CCircle  col1,    CPolygon  col2,Transform2D trans1,Transform2D trans2){return false;}

        private static bool CheckAABB_AABB(CAABB col1, CAABB col2, Transform2D trans1, Transform2D trans2){
            return Utils.TestAABBAABB(
                trans1.pos, col1.radius, col1.size,
                trans2.pos, col2.radius, col2.size);;
        }   
        private static bool CheckAABB_OBB       (CAABB    col1,    COBB      col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckAABB_Polygon   (CAABB    col1,    CPolygon  col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckOBB_OBB        (COBB     col1,    COBB      col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckOBB_Polygon    (COBB     col1,    CPolygon  col2,Transform2D trans1,Transform2D trans2){return false;}   
        private static bool CheckPolygon_Polygon(CPolygon col1,    CPolygon  col2,Transform2D trans1,Transform2D trans2){return false;}  
    }
}