
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerClingState : PlayerState {
        public PlayerClingState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine) {}

        public override void OnEnter() {
            player.GetComponent<Animator>().SetInteger("Animation", 4);
            player.rb.velocity = Vector2.zero;
            player.rb.gravityScale = 0.2f;
        }

        public override void OnExit() {
            player.rb.gravityScale = 3;
        }

        public override void Update() {
            player.rb.velocity -= new Vector2(player.rb.velocity.x, 0);
            player.lastCling = Time.realtimeSinceStartup;

            RaycastHit2D hit = Physics2D.Raycast(player.rb.position, -player.currentHit.normal, player.transform.localScale.x, ~LayerMask.GetMask("Player"));

            if (!player.IsAirborn() || !hit.collider) {
                stateMachine.ChangeState("Idle");
            } else if (Input.GetButtonDown("Jump")) {
                player.defaultSource.PlayOneShot(player.playerAudio.jump);
                player.rb.velocity = (player.currentHit.normal * 12) + (Vector2.up * 10);
                stateMachine.ChangeState("Idle");
            }
        }
    }
}
