
using UnityEngine;
using Utility;

namespace Environment {
    public class FireHydrant : MonoBehaviour {
        public float hitboxHeight;

        private void Awake() {
            GameObject fireHydrantHitbox = new GameObject("FireHydrantHitbox");
            fireHydrantHitbox.transform.position = new Vector3(transform.position.x, transform.position.y + (hitboxHeight / 2), transform.position.z);
            fireHydrantHitbox.transform.localScale = new Vector3(1, hitboxHeight, 0);
            fireHydrantHitbox.transform.parent = transform;

            BoxCollider2D boxCollider = fireHydrantHitbox.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;

            fireHydrantHitbox.AddComponent<HydrantCollisionHandler>();
        }

        private Vector2 GetHitboxDimensions() {
            return new Vector2(transform.position.x, transform.position.y + (hitboxHeight / 2));
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(GetHitboxDimensions(), new Vector3(1, hitboxHeight, 0));
        }
    }
}