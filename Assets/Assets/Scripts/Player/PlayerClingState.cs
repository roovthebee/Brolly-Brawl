
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerClingState : PlayerState {
        public PlayerClingState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine) {}

        public override void OnEnter() {
            player.rb.velocity = Vector2.zero;
            player.rb.gravityScale = 0.2f;
        }

        public override void OnExit() {
            player.rb.gravityScale = 3;
        }

        public override void Update() {
            player.rb.velocity -= new Vector2(player.rb.velocity.x, 0);
            player.lastCling = Time.realtimeSinceStartup;

            if (!player.IsAirborn()) {
                stateMachine.ChangeState("Idle");
            } else if (Input.GetButtonDown("Jump")) {
                player.rb.velocity = (player.currentHit.normal * 12) + (Vector2.up * 10);
                stateMachine.ChangeState("Idle");
            }
        }
    }
}
