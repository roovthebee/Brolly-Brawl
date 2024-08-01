
using UnityEngine;
using System.Collections.Generic;
using Utility;

namespace Managers {
    public class CheckpointManager : MonoBehaviour {
        public List<Checkpoint> checkpoints = new List<Checkpoint>();
        public Sprite idleSprite;
        public Sprite activeSprite;

        public void Awake() {
            foreach (Checkpoint checkpoint in checkpoints) {
                checkpoint.obj.AddComponent<CheckpointCollisionHandler>();
                checkpoint.obj.GetComponent<CheckpointCollisionHandler>().activeSprite = activeSprite;
            }
        }
    }

    [System.Serializable]
    public struct Checkpoint {
        public GameObject obj;
    }
}
