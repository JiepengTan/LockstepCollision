///代码 参考来自PBRT2 BVH
using LockStepMath;
using System.Collections.Generic;
using System.Net.Http.Headers;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;
using static LockStepCollision.Collision;
using Debug = UnityEngine.Debug;
using Shape = LockStepCollision.BaseShape;
using static LockStep.Algorithm;

namespace LockStepCollision {

    // BVHAccel Local Declarations
    public struct BVHShapeInfo {
        public BVHShapeInfo(int pn, AABB b){
            this.ShapeNumber = pn;
            this.bounds = b;
            centroid = b.c;
        }

        public int ShapeNumber;
        public Point centroid;
        public AABB bounds;
    };


    public class BVHBuildNode {
        // BVHBuildNode Public Methods
        public BVHBuildNode(){
            childLeft = null;
            childRight = null;
        }

        public void InitLeaf(int first, int n, AABB b){
            firstPrimOffset = first;
            nShapes = n;
            bounds = b;
        }

        public void InitInterior(int axis, BVHBuildNode c0, BVHBuildNode c1){
            childLeft = c0;
            childRight = c1;
            bounds = Union(c0.bounds, c1.bounds);
            splitAxis = axis;
            nShapes = 0;
        }

        public AABB bounds;
        public BVHBuildNode childLeft;
        public BVHBuildNode childRight;
        public int splitAxis, firstPrimOffset, nShapes;
    };


    public struct CompareToMid {
        public CompareToMid(int d, LFloat m){
            dim = d;
            mid = m;
        }

        int dim;
        LFloat mid;

        public bool Compare(BVHShapeInfo a){
            return a.centroid[dim] < mid;
        }
    };


    public struct ComparePoints {
        public ComparePoints(int d){
            dim = d;
        }

        int dim;

        public int Compare(BVHShapeInfo a, BVHShapeInfo b){
            var ret = (a.centroid[dim] - b.centroid[dim]);
            return ret < 0 ? -1 : (ret == 0) ? 0 : 1;
        }
    };


    public struct CompareToBucket {
        public CompareToBucket(int split, int num, int d, AABB b){
            centroidBounds = b;
            splitBucket = split;
            nBuckets = num;
            dim = d;
        }

        int splitBucket, nBuckets, dim;
        AABB centroidBounds;

        public bool Compare(BVHShapeInfo p){
            int b = (nBuckets * ((p.centroid[dim] - centroidBounds.min[dim]) /
                                 (centroidBounds.max[dim] - centroidBounds.min[dim]))).ToInt;
            if (b == nBuckets) b = nBuckets - 1;
            Debug.Assert(b >= 0 && b < nBuckets);
            return b <= splitBucket;
        }
    };


    public class LinearBVHNode {
        public AABB bounds;

        public int ShapesOffset; // leaf
        public int secondChildOffset; // interior

        public byte nShapes; // 0 . interior node
        public byte axis; // interior node: xyz
        public short pad0; // ensure 32 byte total size
    };

    public class MemoryArena {
        public T[] Alloc<T>(int count = 1) where T : class, new(){
            T[] ret = new T[count];
            for (int i = 0; i < count; ++i)
                ret[i] = new T();
            return ret;
        }

        public T AllocOne<T>() where T : class, new(){
            return new T();
        }
    }

    public class BVH {
        static bool IntersectP(AABB bounds, Ray ray, LVector invDir, int[] dirIsNeg, LFloat mint, LFloat maxt){
            // Check for ray intersection against $x$ and $y$ slabs
            LFloat tmin = (bounds[dirIsNeg[0]].x - ray.o.x) * invDir.x;
            LFloat tmax = (bounds[1 - dirIsNeg[0]].x - ray.o.x) * invDir.x;
            LFloat tymin = (bounds[dirIsNeg[1]].y - ray.o.y) * invDir.y;
            LFloat tymax = (bounds[1 - dirIsNeg[1]].y - ray.o.y) * invDir.y;
            if ((tmin > tymax) || (tymin > tmax))
                return false;
            if (tymin > tmin) tmin = tymin;
            if (tymax < tmax) tmax = tymax;

            // Check for ray intersection against $z$ slab
            LFloat tzmin = (bounds[dirIsNeg[2]].z - ray.o.z) * invDir.z;

            LFloat tzmax = (bounds[1 - dirIsNeg[2]].z - ray.o.z) * invDir.z;
            if ((tmin > tzmax) || (tzmin > tmax))
                return false;
            if (tzmin > tmin)
                tmin = tzmin;
            if (tzmax < tmax)
                tmax = tzmax;
            return (tmin < maxt) && (tmax > mint);
        }

        private List<BaseShape> Shapes;
        private int maxPrimsInNode = 4;

        List<Shape> primitives;
        List<LinearBVHNode> nodes; //改为array?


        // BVHAccel Method Definitions
        public void InitBVHAccel(List<BaseShape> p, int mp, string sm){
            maxPrimsInNode = Max(255, mp);
            Shapes = p;
            var shapeCount = Shapes.Count;
            if (shapeCount == 0) {
                nodes = null;
                return;
            }

            // Initialize _buildData_ array for Shapes
            List<BVHShapeInfo> buildData = new List<BVHShapeInfo>(shapeCount);
            for (int i = 0; i < shapeCount; ++i) {
                AABB bbox = Shapes[i].GetBound();
                buildData.Add(new BVHShapeInfo(i, bbox));
            }

            // Recursively build BVH tree for Shapes
            MemoryArena buildArena = new MemoryArena();
            int totalNodes = 0;
            List<Shape> orderedPrims = new List<BaseShape>(shapeCount);
            BVHBuildNode root = recursiveBuild(buildArena, buildData, 0, Shapes.Count, ref totalNodes, orderedPrims);
            // Compute representation of depth-first traversal of BVH tree
            nodes = new List<LinearBVHNode>(totalNodes);
            int offset = 0;
            //flattenBVHTree(root, &offset);
        }


        bool IntersectP(Ray ray, LFloat tmin, LFloat tmax){
            if (nodes == null || nodes.Count == 0) return false;
            LVector invDir = new LVector(1 / ray.d.x, 1 / ray.d.y, 1 / ray.d.z);
            int[] dirIsNeg = new int[3] {
                invDir.x < 0 ? 1 : 0,
                invDir.y < 0 ? 1 : 0,
                invDir.z < 0 ? 1 : 0
            };
            int[] todo = new int[64]; //最大堆栈数
            int todoOffset = 0, nodeNum = 0;
            while (true) {
                LinearBVHNode node = nodes[nodeNum];
                if (IntersectP(node.bounds, ray, invDir, dirIsNeg, tmin, tmax)) {
                    // Process BVH node _node_ for traversal
                    if (node.nShapes > 0) {
                        for (int i = 0; i < node.nShapes; ++i) {
                            if (Shapes[node.ShapesOffset + i].TestWith(ray)) {
                                return true;
                            }
                        }

                        if (todoOffset == 0) break;
                        nodeNum = todo[--todoOffset];
                    }
                    else {
                        if (dirIsNeg[node.axis] != 0) {
                            /// second child first
                            todo[todoOffset++] = nodeNum + 1;
                            nodeNum = node.secondChildOffset;
                        }
                        else {
                            todo[todoOffset++] = node.secondChildOffset;
                            nodeNum = nodeNum + 1;
                        }
                    }
                }
                else {
                    if (todoOffset == 0) break;
                    nodeNum = todo[--todoOffset];
                }
            }

            return false;
        }

        BVHBuildNode recursiveBuild(MemoryArena buildArena, List<BVHShapeInfo> buildData, int start, int end,
            ref int totalNodes, List<Shape> orderedPrims){
            Debug.Assert(start != end);
            ++totalNodes;
            BVHBuildNode node = buildArena.AllocOne<BVHBuildNode>();
            // Compute bounds of all Shapes in BVH node
            AABB bbox = new AABB(buildData[start].bounds);
            for (int i = start + 1; i < end; ++i)
                bbox = Union(bbox, buildData[i].bounds);
            int nShapes = end - start;
            if (nShapes == 1) {
                // Create leaf _BVHBuildNode_
                int firstPrimOffset = orderedPrims.Count;
                for (int i = start; i < end; ++i) {
                    int primNum = buildData[i].ShapeNumber;
                    orderedPrims.Add(Shapes[primNum]);
                }

                node.InitLeaf(firstPrimOffset, nShapes, bbox);
            }
            else {
                // Compute bound of Shape centroids, choose split dimension _dim_
                AABB centroidBounds = new AABB(buildData[start].bounds);
                for (int i = start; i < end; ++i)
                    centroidBounds = Union(centroidBounds, buildData[i].centroid);
                int dim = centroidBounds.MaximumExtent();

                // Partition Shapes into two sets and build children
                int mid = (start + end) / 2;
                if (centroidBounds.max[dim] == centroidBounds.min[dim]) {
                    // If nShapes is no greater than maxPrimsInNode,
                    // then all the nodes can be stored in a compact bvh node.
                    if (nShapes <= maxPrimsInNode) {
                        // Create leaf _BVHBuildNode_
                        int firstPrimOffset = orderedPrims.Count;
                        for (int i = start; i < end; ++i) {
                            int primNum = buildData[i].ShapeNumber;
                            orderedPrims.Add(Shapes[primNum]);
                        }

                        node.InitLeaf(firstPrimOffset, nShapes, bbox);
                        return node;
                    }
                    else {
                        // else if nShapes is greater than maxPrimsInNode, we
                        // need to split it further to guarantee each node contains
                        // no more than maxPrimsInNode Shapes.
                        node.InitInterior(dim,
                            recursiveBuild(buildArena, buildData, start, mid,
                                ref totalNodes, orderedPrims),
                            recursiveBuild(buildArena, buildData, mid, end,
                                ref totalNodes, orderedPrims));
                        return node;
                    }
                }

                // Partition Shapes based on _splitMethod_
                // Partition Shapes using approximate SAH
                if (nShapes <= 4) {
                    // Partition Shapes into equally-sized subsets
                    mid = (start + end) / 2;
                    NthElement(buildData, start, mid, end, new ComparePoints(dim).Compare);
                }
                else {
                    // Allocate _BucketInfo_ for SAH partition buckets
                    int nBuckets = 12;
                    BucketInfo[] buckets = new BucketInfo[nBuckets];

                    // Initialize _BucketInfo_ for SAH partition buckets
                    for (int i = start; i < end; ++i) {
                        int b = (nBuckets * (
                                     (buildData[i].centroid[dim] - centroidBounds.min[dim]) /
                                     (centroidBounds.max[dim] - centroidBounds.min[dim]))).ToInt;
                        if (b == nBuckets) b = nBuckets - 1;
                        Debug.Assert(b >= 0 && b < nBuckets);
                        buckets[b].count++;
                        buckets[b].bounds = Union(buckets[b].bounds, buildData[i].bounds);
                    }

                    // Compute costs for splitting after each bucket
                    LFloat[] cost = new LFloat[nBuckets - 1];
                    for (int i = 0; i < nBuckets - 1; ++i) {
                        var b0 = new AABB();
                        var b1 = new AABB();
                        int count0 = 0, count1 = 0;
                        for (int j = 0; j <= i; ++j) {
                            b0 = Union(b0, buckets[j].bounds);
                            count0 += buckets[j].count;
                        }

                        for (int j = i + 1; j < nBuckets; ++j) {
                            b1 = Union(b1, buckets[j].bounds);
                            count1 += buckets[j].count;
                        }

                        cost[i] = LFloat.one / 8 + (count0 * b0.SurfaceArea() + count1 * b1.SurfaceArea()) /
                                  bbox.SurfaceArea();
                    }

                    // Find bucket to split at that minimizes SAH metric
                    LFloat minCost = cost[0];
                    int minCostSplit = 0;
                    for (int i = 1; i < nBuckets - 1; ++i) {
                        if (cost[i] < minCost) {
                            minCost = cost[i];
                            minCostSplit = i;
                        }
                    }

                    // Either create leaf or split Shapes at selected SAH bucket
                    if (nShapes > maxPrimsInNode || minCost < nShapes) {
                        //var func = ;
                        int pmid = Partition(buildData, start, end,
                            new CompareToBucket(minCostSplit, nBuckets, dim, centroidBounds).Compare);
                        mid = pmid - 0;
                    }
                    else {
                        // Create leaf _BVHBuildNode_
                        int firstPrimOffset = orderedPrims.Count;
                        for (int i = start; i < end; ++i) {
                            int primNum = buildData[i].ShapeNumber;
                            orderedPrims.Add(Shapes[primNum]);
                        }

                        node.InitLeaf(firstPrimOffset, nShapes, bbox);
                        return node;
                    }
                }

                node.InitInterior(dim,
                    recursiveBuild(buildArena, buildData, start, mid, ref totalNodes, orderedPrims),
                    recursiveBuild(buildArena, buildData, mid, end, ref totalNodes, orderedPrims));
            }

            return node;
        }

        public struct BucketInfo {
            public BucketInfo(int count, AABB aabb){
                this.count = count;
                this.bounds = aabb;
            }

            public int count;
            public AABB bounds;
        };

        int flattenBVHTree(BVHBuildNode node, ref int offset){
            LinearBVHNode linearNode = nodes[offset];
            linearNode.bounds = node.bounds;
            int myOffset = offset++;
            if (node.nShapes > 0) {
                Debug.Assert(node.childLeft != null && node.childRight != null);
                linearNode.ShapesOffset = node.firstPrimOffset;
                linearNode.nShapes = (byte) node.nShapes;
            }
            else {
                // Creater interior flattened BVH node
                linearNode.axis = (byte) node.splitAxis;
                linearNode.nShapes = 0;
                flattenBVHTree(node.childLeft, ref offset);
                linearNode.secondChildOffset = flattenBVHTree(node.childRight, ref offset);
            }

            return myOffset;
        }

        bool Intersect(Ray ray, ref LFloat isect){
            if (nodes == null || nodes.Count == 0) return false;
            bool hit = false;
            LVector invDir = new LVector(1 / ray.d.x, 1 / ray.d.y, 1 / ray.d.z);
            int[] dirIsNeg = new int[3] {
                invDir.x < 0 ? 1 : 0,
                invDir.y < 0 ? 1 : 0,
                invDir.z < 0 ? 1 : 0
            };
            // Follow ray through BVH nodes to find Shape intersections
            int todoOffset = 0, nodeNum = 0;
            int[] todo = new int[64];
            LFloat tmin = LFloat.zero;
            LFloat tmax = LFloat.zero;
            while (true) {
                LinearBVHNode node = nodes[nodeNum];
                // Check ray against BVH node
                if (IntersectP(node.bounds, ray, invDir, dirIsNeg, tmin, tmax)) {
                    if (node.nShapes > 0) {
                        // Intersect ray with Shapes in leaf BVH node
                        for (int i = 0; i < node.nShapes; ++i) {
                            if (Shapes[node.ShapesOffset + i].TestWith(ray, ref isect)) {
                                hit = true;
                            }
                        }

                        if (todoOffset == 0) break;
                        nodeNum = todo[--todoOffset];
                    }
                    else {
                        // Put far BVH node on _todo_ stack, advance to near node
                        if (dirIsNeg[node.axis] != 0) {
                            todo[todoOffset++] = nodeNum + 1;
                            nodeNum = node.secondChildOffset;
                        }
                        else {
                            todo[todoOffset++] = node.secondChildOffset;
                            nodeNum = nodeNum + 1;
                        }
                    }
                }
                else {
                    if (todoOffset == 0) break;
                    nodeNum = todo[--todoOffset];
                }
            }

            return hit;
        }
    }
}