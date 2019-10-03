using System;
using UnityEngine;

namespace StateMachineV2.Actions
{
    
    [CreateAssetMenu(menuName = "States/SplitAttackAction")]
    public class SplitAttackAction : Action
    {
        
        private void OnEnable()
        {
        
            // TODO HACK, UGLY WAY OF STORING DATA
            if (data == null)
            {
                data = new StringObjectDictionary()
                {
                    {"SplitDone",false} // boolean
                };
            }
            else
            {
                if(!data.ContainsKey("SplitDone"))
                    data.Add("SplitDone",false);
            }
        }
        
        public override void OnEnter(StateController controller, ActionData actionData)
        {
            
        }

        public override void Act(StateController controller, ActionData actionData)
        {
            Slime slime = controller.GetComponent<Slime>();
            // TODO Only split when grounded & not moving? Must then be in Act()... but only split once!
            if (slime != null && controller.inputManager.currentTarget != null && controller.inputManager.IsGrounded() && Mathf.Abs(controller.GetComponent<Rigidbody>().velocity.x) < 0.1 && Mathf.Abs(controller.GetComponent<Rigidbody>().velocity.z) < 0.1 && ((actionData.objectData.ContainsKey("SplitDone") && !Convert.ToBoolean(actionData.objectData["SplitDone"])) || !actionData.objectData.ContainsKey("SplitDone")))
            {
                controller.transform.LookAt(controller.inputManager.currentTarget.transform); // TODO HACK: Better smoothly turn towards target!
                Vector3 requiredForce =
                    slime.CalculateSplitForceNeeded(controller.inputManager.currentTarget.transform.position);
                slime.SplitSlime(requiredForce);

                if(actionData.objectData.ContainsKey("SplitDone"))
                    actionData.objectData["SplitDone"] = true;
                else
                    actionData.objectData.Add("SplitDone", true);
            }
        }

        public override void OnExit(StateController controller, ActionData actionData)
        {
            if(actionData.objectData.ContainsKey("SplitDone"))
                actionData.objectData["SplitDone"] = false;
        }
    }
}