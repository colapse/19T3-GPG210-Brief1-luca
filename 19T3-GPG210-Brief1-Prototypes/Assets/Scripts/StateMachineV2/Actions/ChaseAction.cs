using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "States/AI Chase Action")]
public class ChaseAction : Action
{
    public override void OnEnter(StateController controller, ActionData actionData)
    {
        
    }

    public override void Act(StateController controller, ActionData actionData)
    {
        Chase(controller, actionData);
    }

    public override void OnExit(StateController controller, ActionData actionData)
    {
        
    }

    public void Chase(StateController controller, ActionData actionData)
    {
        //Debug.Log("Chasing...");
        // TODO Chase functionality
    }
}
