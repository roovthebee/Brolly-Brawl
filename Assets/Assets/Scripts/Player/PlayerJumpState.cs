
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerJumpState : PlayerState {
        public PlayerJumpState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine) {}

        public override void OnEnter() {
            player.defaultSource.PlayOneShot(player.playerAudio.jump);
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        }

        public override void Update() {
            if (player.rb.velocity.y < -0.1f) {
                player.GetComponent<Animator>().SetInteger("Animation", 3);
            } else {
                player.GetComponent<Animator>().SetInteger("Animation", 2);
            }

            if (player.IsGrounded() && player.rb.velocity.y <= 0.01f) {
                stateMachine.ChangeState("Idle");
            } else if (player.glideEnabled && Input.GetButtonDown("Jump") && player.IsAirborn()) {
                stateMachine.ChangeState("Glide");
            } else if (Input.GetKeyDown(KeyCode.LeftShift) && player.CanDash()) {
                stateMachine.ChangeState("Dash");
            } else if (Input.GetAxis("Horizontal") != 0) {
                player.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * player.moveSpeed, player.rb.velocity.y);
                player.GetComponent<SpriteRenderer>().flipX = player.rb.velocity.x < 0;
            }
        }
    }
}