
using JetBrains.Annotations;
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerIdleState : PlayerState {
        public PlayerIdleState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine) {}

        public override void Update() {
            Vector2 desiredVelocity = Vector2.zero;
            Vector2 smoothedVelocity = player.rb.velocity + (desiredVelocity - player.rb.velocity) * Time.deltaTime * 3;
            player.rb.velocity = smoothedVelocity;
            
            if (Input.GetAxis("Horizontal") != 0) {
                stateMachine.ChangeState("Move");
            } else if (Input.GetButtonDown("Jump") && player.IsGrounded()) {
                stateMachine.ChangeState("Jump");
            } else if (player.glideEnabled && Input.GetButtonDown("Jump") && player.IsAirborn()) {
                stateMachine.ChangeState("Glide");
            } else if (player.rb.velocity.y < -0.1f) {
                player.GetComponent<Animator>().SetInteger("Animation", 3);
            } else {
                player.GetComponent<Animator>().SetInteger("Animation", 0);
            }
        }
    }
}