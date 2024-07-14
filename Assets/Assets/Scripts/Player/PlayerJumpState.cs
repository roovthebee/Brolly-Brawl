
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerJumpState : PlayerState {
        public PlayerJumpState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine) {}

        public override void OnEnter() {
            player.isGrounded = false;
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        }

        public override void Update() {
            if (player.isGrounded) {
                stateMachine.ChangeState("Idle");
            } else if (Input.GetAxis("Horizontal") != 0) {
                player.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * player.moveSpeed, player.rb.velocity.y);
            }
        }
    }
}