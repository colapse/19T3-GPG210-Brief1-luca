using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "States/AI Chase Action")]
public class ChaseAction : Action
{
    private void OnEnable()
    {
        
        // TODO HACK, UGLY WAY OF STORING DATA
        if (data == null)
        {
            data = new StringObjectDictionary()
            {
                {"WaitForSeconds",0} // Float
            };
        }
        else
        {
            if(!data.ContainsKey("WaitForSeconds"))
                data.Add("WaitForSeconds",0);
        }
    }
    
    public override void OnEnter(StateController controller, ActionData actionData)
    {
        
    }

    public override void Act(StateController controller, ActionData actionData)
    {
        Chase(controller, actionData);
    }

    public override void OnExit(StateController controller, ActionData actionData)
    {
        controller.inputManager.currentTarget = null;
    }

    private void Chase(StateController controller, ActionData actionData)
    {
        
        if (controller.inputManager.currentTarget == null)
            return;
        
        if (!controller.inputManager.IsGrounded())
        {
            controller.inputManager.inputForwardJump = false;
            return;
        }else if (controller.inputManager.inputForwardJump)
        {
            return;
        }
        //Debug.Log("Chasing...");
        // TODO Chase functionality
        if (controller.inputManager.IsGrounded())
        {
            controller.transform.LookAt(controller.inputManager.currentTarget.transform);
            controller.inputManager.inputForwardJump = true;
        }
    }
}
