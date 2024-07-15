
using UnityEngine;
using Utility;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {
        public float moveSpeed = 5f;
        public float jumpForce = 7f;
        public Rigidbody2D rb;
        private StateMachine stateMachine;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            stateMachine = new StateMachine();

            // Initialize states
            stateMachine.AddState("Idle", new PlayerIdleState(this, stateMachine));
            stateMachine.AddState("Move", new PlayerMoveState(this, stateMachine));
            stateMachine.AddState("Jump", new PlayerJumpState(this, stateMachine));

            // Set initial state
            stateMachine.ChangeState("Idle");
        }

        private void Update() {
            stateMachine.Update();
        }

        public bool IsGrounded() {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, transform.localScale.y + 0.1f, ~LayerMask.GetMask("Player"));
            return hit.collider != null;
        }
    }
}