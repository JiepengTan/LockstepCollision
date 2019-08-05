using UnityEngine;
using Random = UnityEngine.Random;

namespace TQuadTree1 {
    public class RandomMove : MonoBehaviour {
        public Vector3 targetPos = new Vector3();
        public float halfworldSize;
        public float spd = 3;
        public float updateInterval = 50;
        private float timer = 0;

        void Start(){
            UpdateTargetPos();
        }

        void UpdateTargetPos(){
            targetPos = new Vector3(Random.Range(-halfworldSize, halfworldSize), 0,
                Random.Range(-halfworldSize, halfworldSize));
        }

        private void Update(){
            timer += Time.deltaTime;
            if (timer > updateInterval) {
                timer = 0;
                UpdateTargetPos();
            }

            if ((transform.position - targetPos).sqrMagnitude < 1) {
                UpdateTargetPos();
            }

            transform.position += (targetPos - transform.position).normalized * Time.deltaTime * spd;
        }
    }
}