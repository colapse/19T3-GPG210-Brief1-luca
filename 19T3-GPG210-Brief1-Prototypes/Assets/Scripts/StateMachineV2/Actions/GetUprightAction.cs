using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "States/AI Get Upright Action")]
public class GetUprightAction : Action
{

    public override void OnEnter(StateController controller, ActionData actionData)
    {
        
    }

    public override void Act(StateController controller, ActionData actionData)
    {
        Debug.Log("Trying to get upright");
        if (!controller.inputManager.IsGrounded())
        {
            controller.inputManager.inputJump = false;
        }
        else if(controller.GetComponent<Rigidbody>()?.velocity.magnitude < .2f) // TODO HACK (Only try to jump when not moving
        {
            controller.inputManager.inputJump = true;
        }
        //controller.inputManager.inputJump = true;
    }

    public override void OnExit(StateController controller, ActionData actionData)
    {
        controller.inputManager.inputJump = false;
    }
}
