
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

namespace Environment {
    public class MovingPlatform : MonoBehaviour {
        public List<PathPoint> path = new List<PathPoint>();
        private int pathIndex = -1;

        private void Start() {
            // Initialize platform
            if (path.Count > 0) {
                transform.position = path[0].transform.position;
                pathIndex = 1;
            }
        }

        private void FixedUpdate() {
            if (pathIndex == -1) return;

            PathPoint pathPoint = path[pathIndex];

            // Update platform pos
            Vector2 desiredPos = pathPoint.transform.position;
            Vector2 smoothedPos = Vector2.Lerp(transform.position, desiredPos, pathPoint.smoothSpeed);
            transform.position = smoothedPos;

            // Check if reached destination
            if ((smoothedPos - desiredPos).sqrMagnitude < 0.1f) {
                // Increment pathIndex
                if (pathIndex + 1 > path.Count - 1) {
                    pathIndex = 0;
                } else {
                    pathIndex++;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            // Force player to move with platform
            if (collision.collider.CompareTag("Player")) {
                collision.collider.transform.SetParent(transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision) {
            if (collision.collider.CompareTag("Player")) {
                collision.collider.transform.SetParent(null);
            }
        }
    }

    [System.Serializable]
    public struct PathPoint {
        public Transform transform;
        public float smoothSpeed;
    }
}
