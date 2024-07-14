
using UnityEngine;
using Utility;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {
        public float moveSpeed = 5f;
        public float jumpForce = 7f;
        public bool isGrounded;
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

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.CompareTag("Ground")) {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision) {
            if (collision.collider.CompareTag("Ground")) {
                isGrounded = false;
            }
        }
    }
}