
using UnityEngine;
using System.Collections;
using Utility;
using UnityEditor;

namespace Environment {
    public class FireHydrant : MonoBehaviour {
        public float hitboxHeight;
        public Sprite waterSprite;
        float lastActive;
        private GameObject fireHydrantHitbox;
        public HydrantAudio hydrantAudio;
        public AudioSource defaultSource;
        public AudioSource flowSource;

        private void Awake() {
            fireHydrantHitbox = new GameObject("FireHydrantHitbox");
            fireHydrantHitbox.transform.position = new Vector3(transform.position.x, transform.position.y + (hitboxHeight / 2), transform.position.z);
            fireHydrantHitbox.transform.localScale = new Vector3(1f, hitboxHeight, 0);
            fireHydrantHitbox.transform.parent = transform;

            fireHydrantHitbox.layer = LayerMask.NameToLayer("Ignore Raycast");

            BoxCollider2D boxCollider = fireHydrantHitbox.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;

            SpriteRenderer spriteRenderer = fireHydrantHitbox.AddComponent<SpriteRenderer>();
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            spriteRenderer.sprite = waterSprite;
            spriteRenderer.sortingOrder = -5;

            fireHydrantHitbox.AddComponent<HydrantCollisionHandler>();

            // Initializes sound sources
            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length >= 2) {
                defaultSource = sources[0];
                flowSource = sources[1];
            }

            flowSource.clip = hydrantAudio.flow;
            flowSource.loop = true;
            flowSource.volume = 0;
            flowSource.Play();
        }

        private void Update() {
            fireHydrantHitbox.GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
            fireHydrantHitbox.GetComponent<SpriteRenderer>().sprite = waterSprite;

            if (Time.realtimeSinceStartup - lastActive > 9) {
                lastActive = Time.realtimeSinceStartup;
                StartCoroutine(ActivateHydrant());
            }

            if (fireHydrantHitbox.activeSelf) {
                float desiredVolume = 1;
                float smoothedVolume = flowSource.volume + (desiredVolume - flowSource.volume) * Time.deltaTime;
                flowSource.volume = smoothedVolume;
            } else {
                float smoothedVolume = flowSource.volume - flowSource.volume * Time.deltaTime * 4;
                flowSource.volume = smoothedVolume;
            }
        }

        private IEnumerator ActivateHydrant() {
            defaultSource.PlayOneShot(hydrantAudio.burst);
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

    [System.Serializable]
    public struct HydrantAudio {
        public AudioClip burst;
        public AudioClip flow;
    }
}