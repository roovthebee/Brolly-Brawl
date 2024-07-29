
using UnityEngine;

namespace Utility {
    public class HydrantCollisionHandler : MonoBehaviour {
        public void OnTriggerEnter2D(Collider2D collider) {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            rb.gravityScale *= -1;
        }

        public void OnTriggerExit2D(Collider2D collider) {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            rb.gravityScale *= -1;
        }
    }
}