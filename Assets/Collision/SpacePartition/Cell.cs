using Lockstep.Math;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using static Lockstep.Math.LMath;
using Point = Lockstep.Math.LVector;
using Point2D = Lockstep.Math.LVector2;
using static Lockstep.Collision.Utils;
using Debug = UnityEngine.Debug;
using Shape = Lockstep.Collision.BaseShape;
using static LockStep.Algorithm;


namespace Lockstep.Collision {
    
   

    public struct Cell {
        public int x, y, z;

        public Cell(int px, int py, int pz){
            x = px;
            y = py;
            z = pz;
        }

        public static bool operator ==(Cell lhs, Cell rhs){
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(Cell lhs, Cell rhs){
            return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
        }

        public override bool Equals(object o){
            if (o == null) {
                return false;
            }

            Cell other = (Cell) o;
            return this.x == other.x && this.y == other.y && this.z == other.z;
        }


        public bool Equals(Cell other){
            return this.x == other.x && this.y == other.y && this.z == other.z;
        }

        const uint __h1 = 0x8da6b343; // Large multiplicative constants;
        const uint __h2 = 0xd8163841; // here arbitrarily chosen primes
        const uint __h3 = 0xcb1ab31f;

        public override int GetHashCode(){
            return (int) (__h1 * x + __h2 * y + __h3 * z);
        }
    };
}