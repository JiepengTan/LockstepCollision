using System;
using Lockstep.Math;
using UnityEngine;

namespace Lockstep.Collision2D {
    public struct LRect {
        public LFloat m_XMin;
        public LFloat m_YMin;
        public LFloat m_Width;
        public LFloat m_Height;


        /// <summary>
        ///   <para>Creates a new rectangle.</para>
        /// </summary>
        /// <param name="x">The X value the rect is measured from.</param>
        /// <param name="y">The Y value the rect is measured from.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public LRect(LFloat x, LFloat y, LFloat width, LFloat height){
            this.m_XMin = x;
            this.m_YMin = y;
            this.m_Width = width;
            this.m_Height = height;
        }

        /// <summary>
        ///   <para>Creates a rectangle given a size and position.</para>
        /// </summary>
        /// <param name="position">The position of the minimum corner of the rect.</param>
        /// <param name="size">The width and height of the rect.</param>
        public LRect(LVector2 position, LVector2 size){
            this.m_XMin = position.x;
            this.m_YMin = position.y;
            this.m_Width = size.x;
            this.m_Height = size.y;
        }

        /// <summary>
        ///   <para></para>
        /// </summary>
        /// <param name="source"></param>
        public LRect(Rect source){
            this.m_XMin = source.x.ToLFloat();
            this.m_YMin = source.y.ToLFloat();
            this.m_Width = source.width.ToLFloat();
            this.m_Height = source.height.ToLFloat();
        }

        /// <summary>
        ///   <para>Shorthand for writing new LRect(0,0,0,0).</para>
        /// </summary>
        public static LRect zero {
            get { return new LRect(LFloat.zero, LFloat.zero, LFloat.zero, LFloat.zero); }
        }

        public static LRect MinMaxRect(LFloat xmin, LFloat ymin, LFloat xmax, LFloat ymax){
            return new LRect(xmin, ymin, xmax - xmin, ymax - ymin);
        }

        public void Set(LFloat x, LFloat y, LFloat width, LFloat height){
            this.m_XMin = x;
            this.m_YMin = y;
            this.m_Width = width;
            this.m_Height = height;
        }

        public LFloat x {
            get { return this.m_XMin; }
            set { this.m_XMin = value; }
        }

        public LFloat y {
            get { return this.m_YMin; }
            set { this.m_YMin = value; }
        }

        public LVector2 position {
            get { return new LVector2(this.m_XMin, this.m_YMin); }
            set {
                this.m_XMin = value.x;
                this.m_YMin = value.y;
            }
        }

        public LVector2 center {
            get { return new LVector2(this.x + this.m_Width / 2, this.y + this.m_Height / 2); }
            set {
                this.m_XMin = value.x - this.m_Width / 2;
                this.m_YMin = value.y - this.m_Height / 2;
            }
        }

        /// <summary>
        ///   <para>The position of the minimum corner of the rectangle.</para>
        /// </summary>
        public LVector2 min {
            get { return new LVector2(this.xMin, this.yMin); }
            set {
                this.xMin = value.x;
                this.yMin = value.y;
            }
        }

        /// <summary>
        ///   <para>The position of the maximum corner of the rectangle.</para>
        /// </summary>
        public LVector2 max {
            get { return new LVector2(this.xMax, this.yMax); }
            set {
                this.xMax = value.x;
                this.yMax = value.y;
            }
        }

        /// <summary>
        ///   <para>The width of the rectangle, measured from the X position.</para>
        /// </summary>
        public LFloat width {
            get { return this.m_Width; }
            set { this.m_Width = value; }
        }

        /// <summary>
        ///   <para>The height of the rectangle, measured from the Y position.</para>
        /// </summary>
        public LFloat height {
            get { return this.m_Height; }
            set { this.m_Height = value; }
        }

        /// <summary>
        ///   <para>The width and height of the rectangle.</para>
        /// </summary>
        public LVector2 size {
            get { return new LVector2(this.m_Width, this.m_Height); }
            set {
                this.m_Width = value.x;
                this.m_Height = value.y;
            }
        }

        /// <summary>
        ///   <para>The minimum X coordinate of the rectangle.</para>
        /// </summary>
        public LFloat xMin {
            get { return this.m_XMin; }
            set {
                LFloat xMax = this.xMax;
                this.m_XMin = value;
                this.m_Width = xMax - this.m_XMin;
            }
        }

        /// <summary>
        ///   <para>The minimum Y coordinate of the rectangle.</para>
        /// </summary>
        public LFloat yMin {
            get { return this.m_YMin; }
            set {
                LFloat yMax = this.yMax;
                this.m_YMin = value;
                this.m_Height = yMax - this.m_YMin;
            }
        }

        /// <summary>
        ///   <para>The maximum X coordinate of the rectangle.</para>
        /// </summary>
        public LFloat xMax {
            get { return this.m_Width + this.m_XMin; }
            set { this.m_Width = value - this.m_XMin; }
        }

        /// <summary>
        ///   <para>The maximum Y coordinate of the rectangle.</para>
        /// </summary>
        public LFloat yMax {
            get { return this.m_Height + this.m_YMin; }
            set { this.m_Height = value - this.m_YMin; }
        }

        /// <summary>
        ///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the LRect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <param name="allowInverse">Does the test allow the LRect's width and height to be negative?</param>
        /// <returns>
        ///   <para>True if the point lies within the specified rectangle.</para>
        /// </returns>
        public bool Contains(LVector2 point){
            return point.x >= this.xMin && point.x < this.xMax &&
                   point.y >= this.yMin && point.y < this.yMax;
        }

        /// <summary>
        ///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the LRect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <param name="allowInverse">Does the test allow the LRect's width and height to be negative?</param>
        /// <returns>
        ///   <para>True if the point lies within the specified rectangle.</para>
        /// </returns>
        public bool Contains(LVector3 point){
            return point.x >= this.xMin && point.x < this.xMax &&
                   point.y >= this.yMin && point.y < this.yMax;
        }

        /// <summary>
        ///   <para>Returns true if the x and y components of point is a point inside this rectangle. If allowInverse is present and true, the width and height of the LRect are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <param name="allowInverse">Does the test allow the LRect's width and height to be negative?</param>
        /// <returns>
        ///   <para>True if the point lies within the specified rectangle.</para>
        /// </returns>
        public bool Contains(LVector3 point, bool allowInverse){
            if (!allowInverse)
                return this.Contains(point);
            bool flag = false;
            if (this.width < 0 && point.x <= this.xMin &&
                point.x > this.xMax || this.width >= 0 &&
                point.x >= this.xMin && point.x < this.xMax)
                flag = true;
            return flag &&
                   (this.height < 0 && point.y <= this.yMin &&
                    point.y > this.yMax || this.height >= 0 &&
                    point.y >= this.yMin && point.y < this.yMax);
        }

        private static LRect OrderMinMax(LRect rect){
            if ( rect.xMin >  rect.xMax) {
                LFloat xMin = rect.xMin;
                rect.xMin = rect.xMax;
                rect.xMax = xMin;
            }

            if ( rect.yMin >  rect.yMax) {
                LFloat yMin = rect.yMin;
                rect.yMin = rect.yMax;
                rect.yMax = yMin;
            }

            return rect;
        }

        /// <summary>
        ///   <para>Returns true if the other rectangle overlaps this one. If allowInverse is present and true, the widths and heights of the LRects are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
        /// </summary>
        /// <param name="other">Other rectangle to test overlapping with.</param>
        /// <param name="allowInverse">Does the test allow the widths and heights of the LRects to be negative?</param>
        public bool Overlaps(LRect other){
            return
                other.xMax >  this.xMin
                && other.xMin <  this.xMax
                && other.yMax > this.yMin
                && other.yMin < this.yMax;
        }

        /// <summary>
        ///   <para>Returns true if the other rectangle overlaps this one. If allowInverse is present and true, the widths and heights of the LRects are allowed to take negative values (ie, the min value is greater than the max), and the test will still work.</para>
        /// </summary>
        /// <param name="other">Other rectangle to test overlapping with.</param>
        /// <param name="allowInverse">Does the test allow the widths and heights of the LRects to be negative?</param>
        public bool Overlaps(LRect other, bool allowInverse){
            var rect = this;
            if (allowInverse) {
                rect = LRect.OrderMinMax(rect);
                other = LRect.OrderMinMax(other);
            }

            return rect.Overlaps(other);
        }

        /// <summary>
        ///   <para>Returns a point inside a rectangle, given normalized coordinates.</para>
        /// </summary>
        /// <param name="rectangle">Rectangle to get a point inside.</param>
        /// <param name="normalizedRectCoordinates">Normalized coordinates to get a point for.</param>
        public static LVector2 NormalizedToPoint(
            LRect rectangle,
            LVector2 normalizedRectCoordinates){
            return new LVector2(LMath.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x),
                LMath.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
        }


        public static bool operator !=(LRect lhs, LRect rhs){
            return !(lhs == rhs);
        }

        public static bool operator ==(LRect lhs, LRect rhs){
            return lhs.x == rhs.x && lhs.y == rhs.y &&
                   lhs.width == rhs.width && lhs.height == rhs.height;
        }

        public override int GetHashCode(){
            return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^
                   this.height.GetHashCode() >> 1;
        }

        public override bool Equals(object other){
            if (!(other is LRect))
                return false;
            return this.Equals((Rect) other);
        }

        public bool Equals(Rect other){
            return this.x.Equals(other.x) && this.y.Equals(other.y) && this.width.Equals(other.width) &&
                   this.height.Equals(other.height);
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this LRect.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString(){
            return String.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", (object) this.x, (object) this.y,
                (object) this.width, (object) this.height);
        }
    }
}