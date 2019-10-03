using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "States/AI Wander Action")]
public class WanderAction : Action
{
    public float rotateJumpTreshold = 1; // In Seconds
    public float rotationAngle = 20;
    
    private void OnEnable()
    {
        
        // TODO HACK, UGLY WAY OF STORING DATA
        if (data == null)
        {
            data = new StringObjectDictionary()
            {
                {"ExitPosition",Vector3.negativeInfinity}, // Vector 3; The position the slime was at before exiting state.
                {"WaitForSeconds",0}, // Float
                {"StartRotation",0}, // Float
                {"Rotate",false} // Bool
            };
        }
        else
        {
            if(!data.ContainsKey("ExitPosition"))
                data.Add("ExitPosition",Vector3.negativeInfinity);
            if(!data.ContainsKey("WaitForSeconds"))
                data.Add("WaitForSeconds",0);
            if(!data.ContainsKey("StartRotation"))
                data.Add("StartRotation",0);
            if(!data.ContainsKey("Rotate"))
                data.Add("Rotate",false);
        }
    }

    public override void OnEnter(StateController controller, ActionData actionData)
    {
        
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
        }
        controller.inputManager.inputForwardJump = false;
        controller.inputManager.inputTurnLeft = false;
        controller.inputManager.inputTurnRight = false;
    }

    private void Wander(StateController controller, ActionData actionData)
    {
        bool rotate = Convert.ToBoolean(actionData.objectData["Rotate"]);
        if (!controller.inputManager.IsGrounded())
        {
            controller.inputManager.inputForwardJump = false;
            return;
        }else if (controller.inputManager.inputForwardJump)
        {
            return;
        }else if (controller.inputManager.IsGrounded() && !rotate && !controller.inputManager.inputForwardJump && !controller.inputManager.wasGroundedLastFrame)
        {
            actionData.objectData["StartRotation"] = controller.transform.rotation.eulerAngles.y;
            actionData.objectData["Rotate"] = true;
        }

        
        float rotatedAmount = Mathf.Abs(Convert.ToSingle(actionData.objectData["StartRotation"]) - controller.transform.rotation.eulerAngles.y) % 360;
        
        
        
        if(rotate){
            // Done rotating, wait for a bit until jump
            if (rotatedAmount >= rotationAngle)
            {
                actionData.objectData["WaitForSeconds"] = rotateJumpTreshold;
                controller.inputManager.inputTurnLeft = false; // TODO HACK.. always turns left
                actionData.objectData["Rotate"] = false;
            }
            else
            {
                controller.inputManager.inputTurnLeft = true; // TODO HACK.. always turns left
            }
            
            return;
        }else if (actionData.objectData.ContainsKey("WaitForSeconds") && Convert.ToSingle(actionData.objectData["WaitForSeconds"]) > 0)
        {
            actionData.objectData["WaitForSeconds"] = Convert.ToSingle(actionData.objectData["WaitForSeconds"]) - Time.deltaTime;
            return;
        }else if (Mathf.Abs(controller.GetComponent<Rigidbody>().velocity.x) < 0.1 && Mathf.Abs(controller.GetComponent<Rigidbody>().velocity.z) < 0.1)
        {
            controller.inputManager.inputForwardJump = true;
            
            
        }
            
    }
}
