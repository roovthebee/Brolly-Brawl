
using JetBrains.Annotations;
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerMoveState : PlayerState {
        public PlayerMoveState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine) {}

        public override void Update() {
            float moveInput = Input.GetAxis("Horizontal");
            player.rb.velocity = new Vector2(moveInput * player.moveSpeed, player.rb.velocity.y);

            bool isGrounded = player.IsGrounded();

            if (moveInput == 0) {
                stateMachine.ChangeState("Idle");
            } else if (Input.GetButtonDown("Jump") && player.IsGrounded()) {
                stateMachine.ChangeState("Jump");
            } else if (Input.GetKeyDown(KeyCode.LeftShift) && player.CanDash()) {
                stateMachine.ChangeState("Dash");
            } else if (player.glideEnabled && Input.GetButtonDown("Jump") && player.IsAirborn()) {
                stateMachine.ChangeState("Glide");
            }
        }
    }
}