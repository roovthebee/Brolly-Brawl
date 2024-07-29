
using UnityEngine;
using Player;

namespace Utility {
    public class CheckpointCollisionHandler : MonoBehaviour {
        void OnTriggerEnter2D(Collider2D collider) {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player == null) return;

            player.checkpoint = transform.position;
        }
    }
}