using System.Collections;
using System.Timers;
using UnityEngine;
using Random = System.Random;

namespace Lockstep.UnsafeCollision2D {
    public class RandomMove : MonoBehaviour {
        public float moveSpd = 5;
        public static Vector2 border;
        private Vector3 targetPos;
        public float targetChangedInterval = 3;


        private void Start(){
            timer = targetChangedInterval;
        }

        private float timer;

        private void Update(){
            float deltaTime = 0.04f;

            Vector3 pos = transform.position;
            if (targetPos != pos) {
                pos += (targetPos - transform.position).normalized * moveSpd * deltaTime;
            }

            timer += deltaTime;

            if (timer > targetChangedInterval) {
                timer = 0;
                CalcTargetPos();
            }

            transform.position = pos;
        }

        private void CalcTargetPos(){
            Vector3 _poss = transform.position;
            _poss.x = random.Next(0, (int) (100000)) * 0.001f;
            _poss.z = random.Next(0, (int) (100000)) * 0.001f;
            targetPos = _poss;
        }

        public Random random => LDemoScript.random;
    }
}