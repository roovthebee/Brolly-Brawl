
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerDashState : PlayerState {
        public PlayerDashState(PlayerController player, StateMachine stateMachine) : base (player, stateMachine) {}

        public float startTime;

        public override void OnEnter() {
            player.GetComponent<Animator>().SetInteger("Animation", 5);

            startTime = Time.realtimeSinceStartup;
            player.lastDash = startTime;

            // Dash in current direction
            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), 0).normalized;
            player.rb.velocity = new Vector2(dir.x * player.dashForce, 0);

            player.GetComponent<SpriteRenderer>().flipX = dir.x < 0;
        }

        public override void Update() {
            stateMachine.ChangeState("Idle");
        }
    }
}
