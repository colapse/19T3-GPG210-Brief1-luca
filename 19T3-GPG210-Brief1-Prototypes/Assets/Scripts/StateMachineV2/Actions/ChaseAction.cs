using System.Collections;
using System.Collections.Generic;
using Helper;
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
        //controller.inputManager.currentTarget = null;
        controller.inputManager.inputTurnLeft = false;
        controller.inputManager.inputTurnRight = false;
        controller.inputManager.inputForwardJump = false;
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
        
        if (controller.inputManager.IsGrounded())
        {
            Vector3 myDir = controller.transform.forward;
            myDir.y = 0;
            Vector3 targetDirection =
                (controller.inputManager.currentTarget.transform.position-controller.transform.position);
            targetDirection.y = 0;
            
            // Calculate the angle between self.forward & the direction towards the target
            float angle = Vector3.Angle(myDir.normalized, targetDirection.normalized);//Quaternion.Angle(/*myRotation*/controller.transform.rotation,targetRotation);

            if (angle > 5) // TODO HACK: make 5 to a variable
            {
                // Check if target is on the left or right side
                int side = MathHelper.AngleDir(controller.transform.forward, targetDirection, controller.transform.up);

                if (side == -1)
                {
                    controller.inputManager.inputTurnLeft = true;
                    controller.inputManager.inputTurnRight = false;
                }
                else if (side == 1)
                {
                    controller.inputManager.inputTurnRight = true;
                    controller.inputManager.inputTurnLeft = false;
                }
                    
            }
            else
            {
                controller.inputManager.inputTurnLeft = false;
                controller.inputManager.inputTurnRight = false;
                controller.inputManager.inputForwardJump = true;
            }
            //controller.transform.LookAt(controller.inputManager.currentTarget.transform);
            
        }
    }
}
