using UnityEngine;

namespace StateMachineV1
{
    public class StateBase : MonoBehaviour
    {
        public StateManager owner;
        public StateBase nextState;
    
        public virtual void Enter(){ }
        public virtual void Exit() { }
        public virtual void Execute() { }
    }
}
