
using JetBrains.Annotations;
using UnityEngine;
using Utility;

namespace Player {
    public class PlayerIdleState : PlayerState {
        public PlayerIdleState(PlayerController player, StateMachine stateMachine) : base(player, stateMachine) {}

        public override void Update() {
            if (Input.GetAxis("Horizontal") != 0) {
                stateMachine.ChangeState("Move");
            } else if (Input.GetButtonDown("Jump") && player.IsGrounded()) {
                stateMachine.ChangeState("Jump");
            } else if (player.glideEnabled && Input.GetButtonDown("Jump") && player.IsAirborn()) {
                stateMachine.ChangeState("Glide");
            }
        }
    }
}