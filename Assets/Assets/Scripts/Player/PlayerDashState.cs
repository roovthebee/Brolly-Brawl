
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerDashState : PlayerState {
        public PlayerDashState(PlayerController player, StateMachine stateMachine) : base (player, stateMachine) {}

        public float startTime;

        public override void OnEnter() {
            startTime = Time.realtimeSinceStartup;
            player.lastDash = startTime;

            // Dash in current direction
            Vector2 dir = new Vector2(player.rb.velocity.x, 0).normalized;
            player.rb.velocity = new Vector2(dir.x * player.dashForce, player.rb.velocity.y);
        }

        public override void Update() {
            if (Time.realtimeSinceStartup - startTime > 0.25f) {
                stateMachine.ChangeState("Idle");
            }
        }
    }
}
