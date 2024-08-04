
using UnityEngine;
using Utility;
using UI;
using UnityEngine.SceneManagement;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {
        public bool dashEnabled;
        public bool glideEnabled;
        public bool clingEnabled;
        public float moveSpeed = 5f;
        public float jumpForce = 7f;
        public float dashForce = 5f;
        public float dashCooldown = 1;
        public float lastDash = 0;
        public float glideSmoothing = 0.125f;
        public Rigidbody2D rb;
        private StateMachine stateMachine;
        private Vector2 spawn;
        public Vector2 checkpoint;
        public float lastCling;
        public RaycastHit2D currentHit;
        public float minVelocityY = Mathf.NegativeInfinity;
        public PlayerAudio playerAudio;
        public AudioSource defaultSource;
        public AudioSource glideSource;
        public GameObject umbrellaOpen;
        public GameObject umbrellaClose;
        private Vector2 umbrellaCloseOffset;

        // UI
        public GameObject ui;
        public GameInterfaceController interfaceController;

        private void Start() {
            // Set spawn point for this level
            spawn = transform.position;

            rb = GetComponent<Rigidbody2D>();
            stateMachine = new StateMachine();

            interfaceController = ui.GetComponent<GameInterfaceController>();

            // Initialize states
            stateMachine.AddState("Idle", new PlayerIdleState(this, stateMachine));
            stateMachine.AddState("Move", new PlayerMoveState(this, stateMachine));
            stateMachine.AddState("Jump", new PlayerJumpState(this, stateMachine));
            stateMachine.AddState("Glide", new PlayerGlideState(this, stateMachine));
            stateMachine.AddState("Dash", new PlayerDashState(this, stateMachine));
            stateMachine.AddState("Cling", new PlayerClingState(this, stateMachine));

            // Set initial state
            stateMachine.ChangeState("Idle");

            // Fade into session
            interfaceController.fadePanel.SetActive(true);

            interfaceController.FadeScreenOut(delegate {
                interfaceController.fadePanel.SetActive(false);
            }, 2f);

            // Initialize UI elements
            if (!dashEnabled) interfaceController.dash.SetActive(false);

            // Initialize sound sources
            AudioSource[] sources = GetComponents<AudioSource>();
            if (sources.Length >= 2) {
                defaultSource = sources[0];
                glideSource = sources[1];
            }

            defaultSource.volume = 0.5f;

            glideSource.clip = playerAudio.glide;
            glideSource.loop = true;
            glideSource.volume = 0;
            glideSource.Play();

            umbrellaCloseOffset = rb.position - (Vector2)umbrellaClose.transform.position;
        }

        private void Update() {
            // Constrain velocity
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, minVelocityY));

            // Smooth velocity constraint
            Vector2 desiredVelocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);
            Vector2 smoothedVelocity = rb.velocity + (desiredVelocity - rb.velocity) * Time.deltaTime * 4;
            rb.velocity = smoothedVelocity;

            RaycastHit2D hitRight = Physics2D.Raycast(rb.position, Vector2.right, transform.localScale.x / 2 + 0.1f, ~LayerMask.GetMask("Player"));
            if (hitRight.collider != null) {
                currentHit = hitRight;
                TryCling(hitRight);
            }

            RaycastHit2D hitLeft = Physics2D.Raycast(rb.position, Vector2.left, transform.localScale.x / 2 + 0.1f, ~LayerMask.GetMask("Player"));
            if (hitLeft.collider != null) {
                currentHit = hitLeft;
                TryCling(hitLeft);
            }

            if (Time.realtimeSinceStartup - lastDash > 0.3f) {
                umbrellaClose.SetActive(false);
            }

            if (dashEnabled && Time.realtimeSinceStartup - lastDash > dashCooldown) {
                interfaceController.DashEnable();
            } else {
                interfaceController.DashDisable();
            }

            // Smooth glide volume towards zero
            float smoothedVolume = glideSource.volume - glideSource.volume * Time.deltaTime * 2;
            glideSource.volume = smoothedVolume;

            // Update closed umbrella visual
            umbrellaClose.transform.position = GetComponent<SpriteRenderer>().flipX ? 
                rb.position + new Vector2(umbrellaCloseOffset.x, -umbrellaCloseOffset.y) : 
                rb.position + new Vector2(umbrellaCloseOffset.x * -1, -umbrellaCloseOffset.y);
            umbrellaClose.GetComponent<SpriteRenderer>().flipY = GetComponent<SpriteRenderer>().flipX;

            stateMachine.Update();
        }

        public void TryCling(RaycastHit2D hit) {
            if (!clingEnabled || !hit.collider.CompareTag("Ground") || Time.realtimeSinceStartup - lastCling < 0.3f || !IsAirborn()) return;

            if (hit.normal.y < 0.01f) {
                transform.position = hit.point + (hit.normal * transform.localScale.x / 2);
                stateMachine.ChangeState("Cling");
            }
        }

        public bool IsGrounded() {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, transform.localScale.y + 0.1f, ~LayerMask.GetMask("Player", "Ignore Raycast"));
            return hit.collider != null;
        }

        public bool IsAirborn() {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, transform.localScale.y * 2, ~LayerMask.GetMask("Player", "Ignore Raycast"));
            return hit.collider == null;
        }

        public bool CanDash() {
            return dashEnabled ? Time.realtimeSinceStartup - lastDash > dashCooldown : false;
        }

        public void Die(string deathType) {
            interfaceController.fadePanel.SetActive(true);

            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<Animator>().SetTrigger("Death");
            interfaceController.FadeScreenIn(delegate {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                stateMachine.ChangeState("Idle");
                rb.velocity = Vector3.zero;
                rb.position = checkpoint;
                interfaceController.FadeScreenOut(delegate {
                    interfaceController.fadePanel.SetActive(false);
                }, 1f, 1f);
            }, 0.25f);
        }

        public void EndLevel() {
            SceneManager.LoadScene("MainMenu");
        }
    }

    [System.Serializable]
    public struct PlayerAudio {
        public AudioClip jump;
        public AudioClip dash;
        public AudioClip glide;
    }
}