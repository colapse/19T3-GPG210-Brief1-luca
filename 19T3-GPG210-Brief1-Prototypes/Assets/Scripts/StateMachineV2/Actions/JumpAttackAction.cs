using System;
using UnityEngine;

namespace StateMachineV2.Actions
{
    
    [CreateAssetMenu(menuName = "States/JumpAttachAction")]
    public class JumpAttackAction : Action
    {
        public override void OnEnter(StateController controller, ActionData actionData)
        {
            Debug.Log("Jump Attack Entry!");
        }

        public override void Act(StateController controller, ActionData actionData)
        {
            Slime slime = controller.GetComponent<Slime>();
            // TODO Only split when grounded & not moving? Must then be in Act()... but only split once!
            if (slime != null && controller.inputManager.currentTarget != null && controller.inputManager.IsGrounded() && Mathf.Abs(controller.GetComponent<Rigidbody>().velocity.x) < 0.1 && Mathf.Abs(controller.GetComponent<Rigidbody>().velocity.z) < 0.1)
            {
                controller.transform.LookAt(controller.inputManager.currentTarget.transform); // TODO HACK: Better smoothly turn towards target!
                Vector3 requiredForce =
                    slime.CalculateSplitForceNeeded(controller.inputManager.currentTarget.transform.position, 60); // HACK NIET WORKING WRONG DIRECTION

                if (!float.IsNaN(requiredForce.x) && !float.IsNaN(requiredForce.y) && !float.IsNaN(requiredForce.z))
                {
                    requiredForce.x *= -1; // HACK
                    requiredForce.z *= -1; // HACK
                    requiredForce *= slime.rb.mass * 3;
                    slime.rb.AddForce(requiredForce); // HACK NIET WORKING
                }
            }
        }

        public override void OnExit(StateController controller, ActionData actionData)
        {
            
        }
    }
}