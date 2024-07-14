
using System.Collections.Generic;

namespace Utility {
    public class StateMachine {
        private Dictionary<string, IState> states;
        private IState currentState;

        public StateMachine() {
            states = new Dictionary<string, IState>();
        }

        public void AddState(string key, IState state) {
            states[key] = state;
        }

        public void ChangeState(string key) {
            if (currentState != null) {
                currentState.OnExit();
            }

            if (states.ContainsKey(key)) {
                currentState = states[key];
                currentState.OnEnter();
            }
        }

        public void Update() {
            if (currentState != null) {
                currentState.Update();
            }
        }
    }
}