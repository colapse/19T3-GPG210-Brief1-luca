using UnityEngine;

namespace StateMachineV2.Actions
{
    
    [CreateAssetMenu(menuName = "States/JumpAttachAction")]
    public class JumpAttackAction : Action
    {
        public override void OnEnter(StateController controller, ActionData actionData)
        {
            Debug.Log("Jump Attack!");
        }

        public override void Act(StateController controller, ActionData actionData)
        {
            
        }

        public override void OnExit(StateController controller, ActionData actionData)
        {
            
        }
    }
}