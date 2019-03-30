using LockStepMath;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static LockStepMath.LMath;
using Point = LockStepMath.LVector;
using Point2D = LockStepMath.LVector2D;
using static LockStepCollision.Collision;
using Debug = UnityEngine.Debug;
using Shape = LockStepCollision.BaseShape;
using static LockStep.Algorithm;

namespace LockStepCollision {
    public struct Cell {
        public Cell(int px, int py, int pz){
            x = px;
            y = py;
            z = pz;
        }

        public int x, y, z;
    };


    public class HierachyGrid {
        private const int NUM_BUCKETS = 1024;
        const uint __h1 = 0x8da6b343; // Large multiplicative constants;
        const uint __h2 = 0xd8163841; // here arbitrarily chosen primes
        const uint __h3 = 0xcb1ab31f;

        // Computes hash bucket index in range [0, NUM_BUCKETS-1]
        static int ComputeHashBucketIndex(Cell cellPos){
            int n = (int) (__h1 * cellPos.x + __h2 * cellPos.y + __h3 * cellPos.z);
            n = n % NUM_BUCKETS;
            if (n < 0) n += NUM_BUCKETS;
            return n;
        }

        private const int HGRID_MAX_LEVELS = 5;

        uint occupiedLevelsMask; // Initially zero (Implies max 32 hgrid levels)
        int[] objectsAtLevel = new int[HGRID_MAX_LEVELS]; // Initially all zero
        CObject[] objectBucket = new CObject[NUM_BUCKETS]; // Initially all NULL
        int[] timeStamp = new int[NUM_BUCKETS]; // Initially all zero
        int tick;


        public class CObject {
            public CObject pNextObject; // Embedded link to next hgrid object
            public CObject pPreObject;
            public Point pos; // x, y (and z) position for sphere (or top left AABB corner)
            public LFloat radius; // Radius for bounding sphere (or width of AABB)
            public int bucket; // Index of hash bucket object is in
            public int level; // Grid level for the object
        };

        static LFloat SPHERE_TO_CELL_RATIO = LFloat.one / 4; // Largest sphere in cell is 1/4*cell size
        static LFloat CELL_TO_CELL_RATIO = 2.0f.ToLFloat(); // Cells at next level are 2*side of current cell
        private static LFloat MIN_CELL_SIZE = 4.0f.ToLFloat();

        public void AddObject(CObject obj){
            // Find lowest level where object fully fits inside cell, taking RATIO into account
            int level = 0;
            LFloat size = MIN_CELL_SIZE;
            LFloat diameter = obj.radius * 2;
            for (level = 0; size * SPHERE_TO_CELL_RATIO < diameter; level++)
                size *= CELL_TO_CELL_RATIO;

            // Assert if object is larger than largest grid cell
            Debug.Assert(level < HGRID_MAX_LEVELS);

            // Add object to grid square, and remember cell and level numbers,
            // treating level as a third dimension coordinate
            Cell cellPos = new Cell((int) (obj.pos.x / size), (int) (obj.pos.y / size), level);
            int bucket = ComputeHashBucketIndex(cellPos);
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

        // Test collisions between object and all objects in hgrid
        public void CheckCollision(CObject obj, System.Action<CObject, CObject> pCallbackFunc){
            LFloat size = MIN_CELL_SIZE;
            int startLevel = 0;
            uint occupiedLevelsMask = this.occupiedLevelsMask;
            Point pos = obj.pos;

            // If all objects are tested at the same time, the appropriate starting
            // grid level can be computed as:
            // LFloat diameter = 2.0f * obj.radius;
            // for ( ; size * SPHERE_TO_CELL_RATIO < diameter; startLevel++) {
            //     size *= CELL_TO_CELL_RATIO;
            //     occupiedLevelsMask >>= 1;
            // }

            // For each new query, increase time stamp counter
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
                int x1 = ((pos.x - delta) * ooSize).Floor;
                int y1 = ((pos.y - delta) * ooSize).Floor;
                int x2 = ((pos.x + delta) * ooSize).Ceil;
                int y2 = ((pos.y + delta) * ooSize).Ceil;

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
                            if (p != obj) {
                                LFloat dist2 = Sqr(pos.x - p.pos.x) + Sqr(pos.y - p.pos.y);
                                if (dist2 <= Sqr(obj.radius + p.radius + LFloat.EPSILON))
                                    pCallbackFunc(obj, p); // Close, call callback function
                            }

                            p = p.pNextObject;
                        }
                    }
                }
            } // end for level
        }
    }
}