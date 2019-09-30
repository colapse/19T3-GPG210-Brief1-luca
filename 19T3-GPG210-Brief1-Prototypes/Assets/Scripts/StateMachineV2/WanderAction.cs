using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "States/AI Wander Action")]
public class WanderAction : Action
{
    private void OnEnable()
    {
        
        // TODO HACK
        if (data == null)
        {
            data = new StringObjectDictionary()
            {
                {"ExitPosition",Vector3.negativeInfinity} // The position the slime was at before exiting state
            };
        }
        else if(!data.ContainsKey("ExitPosition"))
        {
            data.Add("ExitPosition",Vector3.negativeInfinity);
        }
    }

    public override void OnEnter(StateController controller, ActionData actionData)
    {//controller.inputManager.inputForwardJump = true;
        if(actionData != null)
            Debug.Log("ENTER; Wandering Exit Pos: "+(Vector3)actionData.objectData["ExitPosition"]);
    }

    public override void Act(StateController controller, ActionData actionData)
    {
        Wander(controller, actionData);
    }

    public override void OnExit(StateController controller, ActionData actionData)
    {
        //controller.inputManager.inputForwardJump = false;
        if (actionData != null)
        {
            actionData.objectData["ExitPosition"] = controller.transform.position;
            Debug.Log("EXIT; Wandering Exit Pos: "+(Vector3)actionData.objectData["ExitPosition"]);
        }
        controller.inputManager.inputForwardJump = false;
        controller.inputManager.inputTurnLeft = false;
        controller.inputManager.inputTurnRight = false;
    }

    public void Wander(StateController controller, ActionData actionData)
    {
        if (!controller.inputManager.IsGrounded())
        {
            controller.inputManager.inputForwardJump = false;
            return;
        }else if (controller.inputManager.inputForwardJump)
        {
            return;
        }

        
        if (data.ContainsKey("RemainingRotation") && Convert.ToSingle(data["RemainingRotation"]) > 0)
        {
            // TODO HACKY MATHS / NAMING. NOT ACCURATE IN ANY SENSE
            float remainingRotation = Convert.ToSingle(data["RemainingRotation"]);
            float rotateAmount = 20 * Time.deltaTime;
            remainingRotation -= rotateAmount;
            
            data["RemainingRotation"] = remainingRotation;
            // Rotate A bit
            //controller.transform.Rotate(rotateAmount * Vector3.left); // HACK Always rotates left....    
            controller.inputManager.inputTurnLeft = true;
            
            if (remainingRotation <= 0f)
            {
                controller.inputManager.inputForwardJump = true;
                controller.inputManager.inputTurnLeft = false;
            }

            return;
        }
        
        // TODO HACKY AMOUNTS
        if(!data.ContainsKey("RemainingRotation"))
        {
            data.Add("RemainingRotation",5);
        }else
        {
            data["RemainingRotation"] = 5;
        }
        // Functionality to wander around
        //Debug.Log("Wandering...");
    }
}
