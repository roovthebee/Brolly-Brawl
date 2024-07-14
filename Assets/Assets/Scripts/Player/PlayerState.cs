
using Utility;

namespace Player {
    public abstract class PlayerState : IState {
        protected PlayerController player;
        protected StateMachine stateMachine;

        public PlayerState(PlayerController player, StateMachine stateMachine) {
            this.player = player;
            this.stateMachine = stateMachine;
        }

        public virtual void OnEnter() {}
        public virtual void OnExit() {}
        public abstract void Update();
    }
}