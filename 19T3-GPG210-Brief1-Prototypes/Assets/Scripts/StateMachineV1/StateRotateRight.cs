using UnityEngine;

namespace StateMachineV1
{
    public class StateRotateRight : StateBase
    {
        public float rotationSpeed = 5; // Degree per second
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Execute()
        {
            if (!SlimeInputManager.IsUpright(transform))
                return;
            transform.Rotate(Vector3.up,rotationSpeed * Time.deltaTime);
        
            if(/*Input.GetKeyUp(KeyCode.RightArrow)*/ !owner.slimeInputManager.inputTurnLeft)
                GetComponent<StateManager>()?.ChangeState(nextState);
        }
    }
}
