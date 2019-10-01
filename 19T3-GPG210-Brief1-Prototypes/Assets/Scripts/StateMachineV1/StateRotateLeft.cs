using UnityEngine;

namespace StateMachineV1
{
    public class StateRotateLeft : StateBase
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
            transform.Rotate(Vector3.up,-rotationSpeed * Time.deltaTime);
        
            if(/*Input.GetKeyUp(KeyCode.LeftArrow)*/ !owner.slimeInputManager.inputTurnLeft)
                GetComponent<StateManager>()?.ChangeState(nextState);
        }
    }
}
