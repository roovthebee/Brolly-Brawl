
using UnityEngine;
using System.Collections.Generic;
using Utility;

namespace Managers {
    public class CheckpointManager : MonoBehaviour {
        public List<Checkpoint> checkpoints = new List<Checkpoint>();

        public void Awake() {
            foreach (Checkpoint checkpoint in checkpoints) {
                GameObject checkpointObject = new GameObject(checkpoint.name);
                checkpointObject.transform.position = checkpoint.position;
                checkpointObject.transform.localScale = checkpoint.size;
                checkpointObject.transform.parent = transform;

                BoxCollider2D boxCollider = checkpointObject.AddComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;

                checkpointObject.AddComponent<CheckpointCollisionHandler>();
            }
        }

        public void OnDrawGizmosSelected() {
            Gizmos.color = Color.white;

            foreach (Checkpoint checkpoint in checkpoints) {
                Gizmos.DrawWireCube(checkpoint.position, checkpoint.size);
            }
        }
    }

    [System.Serializable]
    public struct Checkpoint {
        public string name;
        public Vector2 position;
        public Vector2 size;
    }
}
