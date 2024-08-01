
using UnityEngine;
using Player;
using Managers;

namespace Utility {
    public class CheckpointCollisionHandler : MonoBehaviour {
        public Sprite activeSprite;

        void Start() {
            if (gameObject.name == "Spawn") {
                gameObject.GetComponent<SpriteRenderer>().sprite = activeSprite;
            }
        }

        void OnTriggerEnter2D(Collider2D collider) {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player == null) return;

            player.checkpoint = transform.position;
            gameObject.GetComponent<SpriteRenderer>().sprite = activeSprite;

            if (gameObject.name == "End") {
                player.EndLevel();
            }
        }
    }
}