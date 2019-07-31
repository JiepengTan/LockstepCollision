using System.Collections.Generic;
using System.Runtime.InteropServices;
using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Collision2D {
    public enum CollisionType {
        Enter,
        Stay,
        Exit
    }

    public enum EColliderType2D {
        Shpere2D,
        AABB2D,
    }


    [StructLayout(LayoutKind.Sequential, Pack = NativeHelper.STRUCT_PACK)]
    public unsafe struct PointerFilter {
        public void* value;

        public PointerFilter(void* ptr){
            value = ptr;
        }
    }


    public struct CollisionResult {
        public bool Collides;
        public LVector3 Normal;
        public LFloat Penetration;
        public CollisionType Type;
        public bool First;
    }

    public interface ICollisionBody {
        int RefId { get; set; }
        bool Sleeping { get; }
        LFloat Y { get; set; }
        LVector2 Center { get; set; }
        LVector2 Extents { get; }
        void OnCollision(CollisionResult result, ICollisionBody other);
    }

    public unsafe struct QuadTree {
        public void DrawGizmos(){
            //draw children
            if (_childA != null) _childA->DrawGizmos();
            if (_childB != null) _childB->DrawGizmos();
            if (_childC != null) _childC->DrawGizmos();
            if (_childD != null) _childD->DrawGizmos();

            //draw rect
            Gizmos.color = Color.cyan;
            var p1 = new LVector3(_bounds.position.x, 0.1f.ToLFloat(), _bounds.position.y);
            var p2 = new LVector3(p1.x + _bounds.width, 0.1f.ToLFloat(), p1.z);
            var p3 = new LVector3(p1.x + _bounds.width, 0.1f.ToLFloat(), p1.z + _bounds.height);
            var p4 = new LVector3(p1.x, 0.1f.ToLFloat(), p1.z + _bounds.height);
            DrawLine(p1, p2);
            DrawLine(p2, p3);
            DrawLine(p3, p4);
            DrawLine(p4, p1);
        }

        public void DrawLine(LVector3 a, LVector3 b){
            Gizmos.DrawLine(a.ToVector3(), b.ToVector3());
        }
        ///// Constructors /////

        private int BodySize => sizeof(void*) * _maxBodiesPerNode;

        public QuadTree(LRect bounds, int maxBodiesPerNode = 6, int maxLevel = 6){
            _bounds = bounds;
            _maxBodiesPerNode = maxBodiesPerNode;
            _maxLevel = maxLevel;
            _childA = null;
            _childB = null;
            _childC = null;
            _childD = null;
            _curLevel = 0;
            _parent = null;
            _bodyCount = 0;
            _bodies = QuadTreeFactory.AllocPtrBlock(_maxBodiesPerNode);  
        }

        private QuadTree(LRect bounds, QuadTree* parent)
            : this(bounds, parent->_maxBodiesPerNode, parent->_maxLevel){
            _parent = parent;
            _curLevel = parent->_curLevel + 1;
        }
        private LRect _bounds;
        private int _bodyCount;
        private int _maxBodiesPerNode;
        private int _maxLevel;
        private int _curLevel;
        private QuadTree* _parent;
        private Sphere2D** _bodies;
        private QuadTree* _childA;
        private QuadTree* _childB;
        private QuadTree* _childC;
        private QuadTree* _childD;
        private static List<long> _tempPtrList = new List<long>(64);

        public void AddBody(AABB2D* body){
            AddBody((Sphere2D*) body);
        }

        public void AddBody(Sphere2D* body){
            if (_childA != null) {
                var child = GetQuadrant(body->Pos);
                child->AddBody(body);
            }
            else {
                if (_bodies == null) {
                    _bodies = QuadTreeFactory.AllocPtrBlock(_maxBodiesPerNode);   
                }

                _bodies[_bodyCount++] = body;
                if (_bodyCount >= _maxBodiesPerNode && _curLevel < _maxLevel) {
                    Split();
                }
            }
        }

        public List<long> GetBodies(LVector2 point, LFloat radius){
            _tempPtrList.Clear();
            _GetBodies(point, radius, _tempPtrList);
            return _tempPtrList;
        }

        public List<long> GetBodies(LRect rect){
            _tempPtrList.Clear();
            _GetBodies(rect, _tempPtrList);
            return _tempPtrList;
        }

        private void _GetBodies(LVector2 point, LFloat radius, List<long> bods){
            //no children
            if (_childA == null) {
                for (int i = 0; i < _bodyCount; i++)
                    bods.Add((long)(_bodies[i]));
            }
            else {
                if (_childA->ContainsCircle(point, radius))
                    _childA->_GetBodies(point, radius, bods);
                if (_childB->ContainsCircle(point, radius))
                    _childB->_GetBodies(point, radius, bods);
                if (_childC->ContainsCircle(point, radius))
                    _childC->_GetBodies(point, radius, bods);
                if (_childD->ContainsCircle(point, radius))
                    _childD->_GetBodies(point, radius, bods);
            }
        }

        private void _GetBodies(LRect rect, List<long> bods){
            //no children
            if (_childA == null) {
                for (int i = 0; i < _bodyCount; i++)
                    bods.Add((long)(_bodies[i]));
            }
            else {
                if (_childA->ContainsRect(rect))
                    _childA->_GetBodies(rect, bods);
                if (_childB->ContainsRect(rect))
                    _childB->_GetBodies(rect, bods);
                if (_childC->ContainsRect(rect))
                    _childC->_GetBodies(rect, bods);
                if (_childD->ContainsRect(rect))
                    _childD->_GetBodies(rect, bods);
            }
        }

        public bool ContainsCircle(LVector2 circleCenter, LFloat radius){
            var center = _bounds.center;
            var dx = LMath.Abs(circleCenter.x - center.x);
            var dy = LMath.Abs(circleCenter.y - center.y);
            if (dx > (_bounds.width / 2 + radius)) {
                return false;
            }

            if (dy > (_bounds.height / 2 + radius)) {
                return false;
            }

            if (dx <= (_bounds.width / 2)) {
                return true;
            }

            if (dy <= (_bounds.height / 2)) {
                return true;
            }

            var dsx = (dx - _bounds.width / 2);
            var dsy = (dy - _bounds.height / 2);
            var cornerDist = dsx * dsx + dsy * dsy;
            return cornerDist <= (radius * radius);
        }

        public bool ContainsRect(LRect rect){
            return _bounds.Overlaps(rect);
        }

        //TODO Free all child
        public void Clear(){
            _parent = null;
            _bodyCount = 0;
            FreeTree(ref _childA);
            FreeTree(ref _childB);
            FreeTree(ref _childC);
            FreeTree(ref _childD);
            if (_bodies != null) {
                QuadTreeFactory.FreePtrBlock(_bodies,_maxBodiesPerNode);
                _bodies = null;
            }
        }

        QuadTree* AllocTree(LRect rect, QuadTree* tree){
            var ptr = QuadTreeFactory.AllocQuadTree();
            *ptr = new QuadTree(rect, tree);
            return ptr;
        }

        void FreeTree(ref QuadTree* ptr){
            if (ptr != null) {
                ptr->Clear();
                QuadTreeFactory.FreeQuadTree(ptr);
                ptr = null;
            }
        }
  
        private void Split(){
            var hx = _bounds.width / 2;
            var hz = _bounds.height / 2;
            var sz = new LVector2(hx, hz);

            //split a
            var aLoc = _bounds.position;
            var aRect = new LRect(aLoc, sz);
            //split b
            var bLoc = new LVector2(_bounds.position.x + hx, _bounds.position.y);
            var bRect = new LRect(bLoc, sz);
            //split c
            var cLoc = new LVector2(_bounds.position.x + hx, _bounds.position.y + hz);
            var cRect = new LRect(cLoc, sz);
            //split d
            var dLoc = new LVector2(_bounds.position.x, _bounds.position.y + hz);
            var dRect = new LRect(dLoc, sz);
            fixed (QuadTree* thisPtr = &this) {
                _childA = AllocTree(aRect, thisPtr);
                _childB = AllocTree(bRect, thisPtr);
                _childC = AllocTree(cRect, thisPtr);
                _childD = AllocTree(dRect, thisPtr);
            }

            //assign QuadTrees
            for (int i = _bodyCount - 1; i >= 0; i--) {
                var child = GetQuadrant(_bodies[i]->Pos);
                child->AddBody(_bodies[i]);
                _bodies[i] = null;
            }
        }

        private QuadTree* GetQuadrant(LVector2 point){
            if (_childA == null) return null;
            if (point.x > _bounds.x + _bounds.width / 2) {
                if (point.y > _bounds.y + _bounds.height / 2) return _childC;
                else return _childB;
            }
            else {
                if (point.y > _bounds.y + _bounds.height / 2) return _childD;
                return _childA;
            }
        }
    }
}