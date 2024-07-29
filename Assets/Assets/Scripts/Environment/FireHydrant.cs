
using UnityEngine;
using System.Collections;
using Utility;

namespace Environment {
    public class FireHydrant : MonoBehaviour {
        public float hitboxHeight;
        public Sprite waterSprite;
        float lastActive;
        public GameObject fireHydrantHitbox;

        private void Awake() {
            fireHydrantHitbox = new GameObject("FireHydrantHitbox");
            fireHydrantHitbox.transform.position = new Vector3(transform.position.x, transform.position.y + (hitboxHeight / 2), transform.position.z);
            fireHydrantHitbox.transform.localScale = new Vector3(1f, hitboxHeight, 0);
            fireHydrantHitbox.transform.parent = transform;

            BoxCollider2D boxCollider = fireHydrantHitbox.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;

            SpriteRenderer spriteRenderer = fireHydrantHitbox.AddComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0, 1, 1);
            spriteRenderer.sprite = waterSprite;
            spriteRenderer.sortingOrder = -5;

            fireHydrantHitbox.AddComponent<HydrantCollisionHandler>();
        }

        private void Update() {
            if (Time.realtimeSinceStartup - lastActive > 9) {
                lastActive = Time.realtimeSinceStartup;
                StartCoroutine(ActivateHydrant());
            }
        }

        private IEnumerator ActivateHydrant() {
            fireHydrantHitbox.SetActive(true);
            yield return new WaitForSeconds(5);
            fireHydrantHitbox.SetActive(false);
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