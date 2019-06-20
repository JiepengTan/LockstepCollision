using Lockstep.Math;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static Lockstep.Collision.Utils;
using static Lockstep.Math.LMath;
using Point2D = Lockstep.Math.LVector2;
using Debug = UnityEngine.Debug;
using Shape = Lockstep.Collision.BaseShape;
using static LockStep.Algorithm;

namespace Lockstep.Collision {
 
    public class HierachyGrid  {
        /// <summary>
        /// the size of cell
        /// </summary>
        public LFloat MIN_CELL_SIZE = 4.0f.ToLFloat();

        private const int NUM_BUCKETS = 1024;
        private const int HGRID_MAX_LEVELS = 5;

        private uint occupiedLevelsMask; // Initially zero (Implies max 32 hgrid levels)
        private int[] objectsAtLevel = new int[HGRID_MAX_LEVELS]; // Initially all zero
        private CObject[] objectBucket = new CObject[NUM_BUCKETS]; // Initially all NULL
        private int[] timeStamp = new int[NUM_BUCKETS]; // Initially all zero
        private int tick;

        private static LFloat SPHERE_TO_CELL_RATIO = LFloat.one / 4; // Largest sphere in cell is 1/4*cell size
        private static LFloat CELL_TO_CELL_RATIO = 2.0f.ToLFloat(); // Cells at next level are 2*side of current cell

        private HashSet<long> allCheckedCollitionPairs = new HashSet<long>(); //all has checked collision pairs in this frame

        private HashSet<long> lastFrameCollidedPairs = new HashSet<long>(); //all pairs that collided last frame

        private LFloat[] levelSize = new LFloat[HGRID_MAX_LEVELS];

        public Dictionary<int, CObject> allCObjs = new Dictionary<int, CObject>();

        public HierachyGrid(){
            //init level to size map
            int level = 0;
            LFloat size = MIN_CELL_SIZE;
            levelSize[0] = size;
            for (int i = 1; i < HGRID_MAX_LEVELS; i++) {
                size *= CELL_TO_CELL_RATIO;
                levelSize[i] = size;
            }
            //should read from config
            for (int i = 0; i < targetMasks.Length; i++) {
                targetMasks[i] = 0xffffffff;
            }
        }
        private uint[] targetMasks = new uint[32];
        private uint GetTargetMask(CObject obj){
            return targetMasks[obj.layerMaskIdx];
        }

        public LFloat LevelToSize(int level){
            return levelSize[level];
        }

        public void AddObject(CObject obj){
            // Find lowest level where object fully fits inside cell, taking RATIO into account
            var cellPos = ComputeCellPos(obj);
            var bucket = ComputeHashBucketIndex(cellPos);
            AddObject(obj, bucket, cellPos);
        }

        private void AddObject(CObject obj, int bucket, Cell cell){
            var level = cell.z;
            obj.cellPos = cell;
            obj.bucket = bucket;
            obj.level = level;
            obj.pPreObject = null;
            var _obj = this.objectBucket[bucket];
            if (_obj != null) {
                _obj.pPreObject = obj;
            }

            obj.pNextObject = _obj;
            this.objectBucket[bucket] = obj;

            // Mark this level as having one more object. Also indicate level is in use
            this.objectsAtLevel[level]++;
            this.occupiedLevelsMask |= (uint) (1 << level);
        }

        public void RemoveObject(CObject obj){
            // One less object on this grid level. Mark level as unused if no objects left
            if (--this.objectsAtLevel[obj.level] == 0)
                this.occupiedLevelsMask &= (uint) (~(1 << obj.level));

            // Now scan through list and unlink object 'obj'
            int bucket = obj.bucket;
            CObject p = this.objectBucket[bucket];
            // Special-case updating list header when object is first in list
            if (p == obj) {
                this.objectBucket[bucket] = obj.pNextObject;
            }

            if (obj.pPreObject != null) {
                obj.pPreObject.pNextObject = obj.pNextObject;
            }

            if (obj.pNextObject != null) {
                obj.pNextObject.pPreObject = obj.pPreObject;
            }
        }

        public void ChangeObjectCell(CObject obj, Cell newCell, int newBucket){
            RemoveObject(obj);
            AddObject(obj, newBucket, newCell);
        }

        public void OnObjectMoved(CObject obj, LVector3 newPos){
            var size = LevelToSize(obj.level);
            var x = (int) (obj.pos.x / size);
            var y = (int) (obj.pos.y / size);
            //try to quit fast 
            if (x == obj.cellPos.x && y == obj.cellPos.y) {
                return;
            }

            obj.cellPos = new Cell(x, y, obj.level);
            var bucket = ComputeHashBucketIndex(obj.cellPos);
            if (obj.bucket != bucket) {
                ChangeObjectCell(obj, obj.cellPos, bucket);
            }
        }

        public void OnObjectMovedAndSizeChanged(CObject obj){
            var cellPos = ComputeCellPos(obj);
            if (cellPos != obj.cellPos) {
                obj.cellPos = cellPos;
                var bucket = ComputeHashBucketIndex(cellPos);
                if (obj.bucket != bucket) {
                    ChangeObjectCell(obj, obj.cellPos, bucket);
                }
            }
        }

        private long ComputeColisionPairKey(CObject a, CObject b){
            if (a.ID > b.ID) {
                return (((long) a.ID) << 32) | b.ID;
            }
            else {
                return (((long) b.ID) << 32) | a.ID;
            }
        }

        public void CheckAllCollision(System.Func<CObject, CObject, bool> pCallbackFunc,
            System.Action<CObject, CObject, int> collitionResultCallBack){
            allCheckedCollitionPairs.Clear();
            foreach (var item in allCObjs) {
                CheckCollision(item.Value, pCallbackFunc, collitionResultCallBack, true);
            }

            allCheckedCollitionPairs.Clear();
        }

        public const int ETriggerEnter = 0;
        public const int ETriggerExit = 1;
        public const int ETriggerStay = 2;

        // Test collisions between object and all objects in hgrid
        private void CheckCollision(CObject obj, System.Func<CObject, CObject, bool> pCallbackFunc,
            System.Action<CObject, CObject, int> collitionResultCallBack,
            bool isNeedRecoderCheck = false){
            LFloat size = MIN_CELL_SIZE;
            int startLevel = 0;
            uint occupiedLevelsMask = this.occupiedLevelsMask;
            LVector3 pos = obj.pos;

            // For each new query, increase time stamp counter
            var targetLayerMask = GetTargetMask(obj);
            this.tick++;
            for (int level = startLevel;
                level < HGRID_MAX_LEVELS;
                size *= CELL_TO_CELL_RATIO, occupiedLevelsMask >>= 1, level++) {
                // If no objects in rest of grid, stop now
                if (occupiedLevelsMask == 0) break;
                // If no objects at this level, go on to the next level
                if ((occupiedLevelsMask & 1) == 0) continue;

                // Compute ranges [x1..x2, y1..y2] of cells overlapped on this level. To
                // make sure objects in neighboring cells are tested, by increasing range by
                // the maximum object overlap: size * SPHERE_TO_CELL_RATIO
                LFloat delta = obj.radius + size * SPHERE_TO_CELL_RATIO + LFloat.EPSILON;
                LFloat ooSize = 1 / size;
                int x1 = ((pos.x - delta) * ooSize).Floor();
                int y1 = ((pos.y - delta) * ooSize).Floor();
                int x2 = ((pos.x + delta) * ooSize).Ceil();
                int y2 = ((pos.y + delta) * ooSize).Ceil();
        
                // Check all the grid cells overlapped on current level
                for (int x = x1; x <= x2; x++) {
                    for (int y = y1; y <= y2; y++) {
                        // Treat level as a third dimension coordinate
                        Cell cellPos = new Cell(x, y, level);
                        int bucket = ComputeHashBucketIndex(cellPos);

                        // Has this hash bucket already been checked for this object?
                        if (this.timeStamp[bucket] == this.tick) continue;
                        this.timeStamp[bucket] = this.tick;

                        // Loop through all objects in the bucket to find nearby objects
                        CObject p = this.objectBucket[bucket];
                        while (p != null) {
                            var layermask = p.LayerMask;
                            var isTargetLayer = ((targetLayerMask & layermask)!= 0);
                            if (isTargetLayer && p != obj && p.cellPos == cellPos ) {
                                long pairKey = -1L; // pairkey must >0L
                                if (isNeedRecoderCheck) {
                                    pairKey = ComputeColisionPairKey(p, obj);
                                    //check whether has checked in this frame
                                    if (allCheckedCollitionPairs.Add(pairKey)) {
                                        LFloat dist2 = Sqr(pos.x - p.pos.x) + Sqr(pos.y - p.pos.y);
                                        if (dist2 <= Sqr(obj.radius + p.radius + LFloat.EPSILON)) {
                                            var hasCollided = pCallbackFunc(obj, p); // Close, call callback function
                                            var hasCollidedLastFrame = lastFrameCollidedPairs.Contains(pairKey);
                                            if (!hasCollided) {
                                                if (hasCollidedLastFrame) {
                                                    collitionResultCallBack(p, obj, HierachyGrid.ETriggerExit);
                                                }
                                            }
                                            else if (hasCollidedLastFrame) {
                                                collitionResultCallBack(p, obj, HierachyGrid.ETriggerStay);
                                            }
                                            else {
                                                collitionResultCallBack(p, obj, HierachyGrid.ETriggerEnter);
                                            }
                                        }
                                    }
                                }
                                else {
                                    LFloat dist2 = Sqr(pos.x - p.pos.x) + Sqr(pos.y - p.pos.y);
                                    if (dist2 <= Sqr(obj.radius + p.radius + LFloat.EPSILON)) {
                                        var hasCollided = pCallbackFunc(obj, p); // Close, call callback function
                                        if (hasCollided) {
                                            collitionResultCallBack(p, obj, HierachyGrid.ETriggerEnter);
                                        }
                                    }
                                }
                            }

                            p = p.pNextObject;
                        }
                    }
                }
            } // end for level
        }


        // Computes hash bucket index in range [0, NUM_BUCKETS-1]
        static int ComputeHashBucketIndex(Cell cellPos){
            int n = cellPos.GetHashCode();
            n = n % NUM_BUCKETS;
            if (n < 0) n += NUM_BUCKETS;
            return n;
        }


        private Cell ComputeCellPos(CObject obj){
            int level = 0;
            LFloat size = MIN_CELL_SIZE;
            LFloat diameter = obj.radius * 2;
            for (level = 0; size * SPHERE_TO_CELL_RATIO < diameter; level++)
                size *= CELL_TO_CELL_RATIO;

            // Assert if object is larger than largest grid cell
            Debug.Assert(level < HGRID_MAX_LEVELS);

            // Add object to grid square, and remember cell and level numbers,
            // treating level as a third dimension coordinate
            return new Cell((int) (obj.pos.x / size), (int) (obj.pos.y / size), level);
        }
    }
}