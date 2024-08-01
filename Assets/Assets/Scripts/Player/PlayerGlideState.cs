
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Player {
    public class PlayerGlideState : PlayerState {
        public PlayerGlideState(PlayerController player, StateMachine stateMachine) : base (player, stateMachine) {}

        public override void OnEnter() {
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0);
            player.rb.gravityScale = 0.1f;
        }

        public override void OnExit() {
            player.rb.gravityScale = 3f;
            player.rb.rotation = 0;
        }

        public override void Update() {
            if (player.IsGrounded() || Input.GetButtonUp("Jump")) {
                stateMachine.ChangeState("Idle");
            } else if (Input.GetKeyDown(KeyCode.LeftShift) && player.CanDash()) {
                stateMachine.ChangeState("Dash");
            } else if (Input.GetAxis("Horizontal") != 0) {
                Vector2 desiredVelocity = new Vector2(Input.GetAxis("Horizontal") * player.moveSpeed, player.rb.velocity.y);
                Vector2 smoothedVelocity = Vector2.Lerp(player.rb.velocity, desiredVelocity, Time.deltaTime);
                player.rb.velocity = smoothedVelocity;
            }

            // Update yaw
            float desiredYaw = -20 * (player.rb.velocity.x / player.moveSpeed);
            float smoothedYaw = player.rb.rotation + (desiredYaw - player.rb.rotation) * Time.deltaTime * 3;
            player.rb.rotation = smoothedYaw;
        }
    }
}
