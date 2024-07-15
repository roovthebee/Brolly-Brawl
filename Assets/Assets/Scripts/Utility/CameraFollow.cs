
using UnityEngine;

namespace Utility {
    public class CameraFollow: MonoBehaviour {
        public Transform target;
        public bool active = true;
        public float smoothSpeed = 0.125f;
        public Vector3 offset;

        private void FixedUpdate() {
            if (target == null || !active) return;

            Vector3 desiredPos = target.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
        }
    }
}
