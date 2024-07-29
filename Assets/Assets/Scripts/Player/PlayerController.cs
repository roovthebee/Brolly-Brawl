
using UnityEngine;
using Utility;
using UI;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {
        public bool dashEnabled;
        public bool glideEnabled;
        public float moveSpeed = 5f;
        public float jumpForce = 7f;
        public float dashForce = 5f;
        public float dashCooldown = 1;
        public float lastDash = 0;
        public float minVelocityY = Mathf.NegativeInfinity;
        public float glideSmoothing = 0.125f;
        public Rigidbody2D rb;
        private StateMachine stateMachine;
        private Vector2 spawn;
        public Vector2 checkpoint;

        // UI
        public GameObject fadePanel;

        private void Start() {
            // Set spawn point for this level
            spawn = transform.position;

            rb = GetComponent<Rigidbody2D>();
            stateMachine = new StateMachine();

            // Initialize states
            stateMachine.AddState("Idle", new PlayerIdleState(this, stateMachine));
            stateMachine.AddState("Move", new PlayerMoveState(this, stateMachine));
            stateMachine.AddState("Jump", new PlayerJumpState(this, stateMachine));
            stateMachine.AddState("Glide", new PlayerGlideState(this, stateMachine));
            stateMachine.AddState("Dash", new PlayerDashState(this, stateMachine));

            // Set initial state
            stateMachine.ChangeState("Idle");

            // Fade into session
            fadePanel.GetComponent<FadePanelController>().FadeOut(delegate {
                fadePanel.SetActive(false);
            }, 2f);
        }

        private void Update() {
            // Constrain velocity
            Vector3 desiredVelocity = new Vector3(rb.velocity.x, Mathf.Max(rb.velocity.y, minVelocityY), 0);
            Vector3 smoothedVelocity = Vector3.Lerp(rb.velocity, desiredVelocity, glideSmoothing);
            rb.velocity = smoothedVelocity;

            stateMachine.Update();
        }

        public bool IsGrounded() {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, transform.localScale.y + 0.1f, ~LayerMask.GetMask("Player"));
            return hit.collider != null;
        }

        public bool IsAirborn() {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, transform.localScale.y * 2, ~LayerMask.GetMask("Player"));
            return hit.collider == null;
        }

        public bool CanDash() {
            return dashEnabled ? Time.realtimeSinceStartup - lastDash > dashCooldown : false;
        }

        public void Die(string deathType) {
            fadePanel.SetActive(true);

            if (deathType == "") {
                fadePanel.GetComponent<FadePanelController>().FadeIn(delegate {
                    stateMachine.ChangeState("Idle");
                    rb.velocity = Vector3.zero;
                    rb.position = checkpoint;
                    fadePanel.GetComponent<FadePanelController>().FadeOut(delegate {
                        fadePanel.SetActive(false);
                    }, 1f, 1f);
                }, 2f);
            }
        }
    }
}