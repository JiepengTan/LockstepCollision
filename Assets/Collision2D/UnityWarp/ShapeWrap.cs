using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Lockstep.Collision;
using Lockstep.Math;
using UnityEngine;
using UnityEngine.Profiling;

namespace Lockstep.Collision2D {
    public unsafe class ShapeWrap {
        public virtual int TypeId { get; }
    }

    public unsafe class ShapeWrapCircle : ShapeWrap {
        public Circle shape;
        public override int TypeId => (int) EShape2D.Circle;
    }

    public unsafe class ShapeWrapAABB : ShapeWrap {
        public AABB2D shape;
        public override int TypeId => (int) EShape2D.AABB;
    }

    public unsafe class ShapeWrapOBB : ShapeWrap {
        public OBB2D shape;
        public override int TypeId => (int) EShape2D.OBB;
    }

    public unsafe class ShapeWrapPolygon : ShapeWrap {
        public Polygon shape;
        public override int TypeId => (int) EShape2D.Polygon;
        public List<LVector2> points = new List<LVector2>();

        public void AddPoint(LVector2 point){
            points.Add(point);
        }

        public void RemovePoint(int idx){
            points.RemoveAt(idx);
        }
    }
}