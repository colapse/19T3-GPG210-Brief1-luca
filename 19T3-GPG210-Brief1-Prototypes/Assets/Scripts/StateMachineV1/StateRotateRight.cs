using UnityEngine;

namespace StateMachineV1
{
    public class StateRotateRight : StateBase
    {
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Execute()
        {
            transform.Rotate(Vector3.up,5);
        
            if(/*Input.GetKeyUp(KeyCode.RightArrow)*/ !owner.slimeInputManager.inputTurnLeft)
                GetComponent<StateManager>()?.ChangeState(nextState);
        }
    }
}
