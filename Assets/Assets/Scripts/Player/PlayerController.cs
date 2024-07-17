
using UnityEngine;
using Utility;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {
        public float moveSpeed = 5f;
        public float jumpForce = 7f;
        public float dashForce = 5f;
        public float dashCooldown = 1;
        public bool canGlide = true;
        public float lastDash = 0;
        public float minVelocityY = Mathf.NegativeInfinity;
        public float glideSmoothing = 0.125f;
        public Rigidbody2D rb;
        private StateMachine stateMachine;

        private void Start() {
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
            if (hit.collider != null) canGlide = true;

            return hit.collider != null;
        }

        public bool IsAirborn() {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, transform.localScale.y * 2, ~LayerMask.GetMask("Player"));
            return hit.collider == null;
        }

        public bool CanDash() {
            return Time.realtimeSinceStartup - lastDash > dashCooldown;
        }
    }
}