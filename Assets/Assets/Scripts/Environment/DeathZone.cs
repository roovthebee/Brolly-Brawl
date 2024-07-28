
using UnityEngine;
using Player;

namespace Environment {
    public class DeathZone : MonoBehaviour {
        public string deathType;

        public void OnTriggerEnter2D(Collider2D collider) {
            PlayerController player = collider.GetComponent<PlayerController>();
            player?.Die(deathType);
        }
    }
}
