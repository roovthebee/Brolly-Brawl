
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerJumpState : PlayerState {
        public PlayerJumpState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine) {}

        public override void OnEnter() {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        }

        public override void Update() {
            if (player.IsGrounded() && player.rb.velocity.y <= 0.01f) {
                stateMachine.ChangeState("Idle");
            } else if (player.canGlide && Input.GetButtonDown("Jump") && player.IsAirborn()) {
                stateMachine.ChangeState("Glide");
            } else if (Input.GetAxis("Horizontal") != 0) {
                player.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * player.moveSpeed, player.rb.velocity.y);
            }
        }
    }
}