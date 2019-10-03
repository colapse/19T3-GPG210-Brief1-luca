using UnityEngine;

namespace StateMachineV2.Actions
{
    
    [CreateAssetMenu(menuName = "States/SplitAttackAction")]
    public class SplitAttackAction : Action
    {
        public override void OnEnter(StateController controller, ActionData actionData)
        {
            Debug.Log("Split Attack!");
            Slime slime = controller.GetComponent<Slime>();
            if (slime != null && controller.inputManager.currentTarget != null)
            {
                Vector3 requiredForce =
                    slime.CalculateSplitForceNeeded(controller.inputManager.currentTarget.transform.position);
                slime.SplitSlime(requiredForce);
            }
        }

        public override void Act(StateController controller, ActionData actionData)
        {
            
        }

        public override void OnExit(StateController controller, ActionData actionData)
        {
            
        }
    }
}