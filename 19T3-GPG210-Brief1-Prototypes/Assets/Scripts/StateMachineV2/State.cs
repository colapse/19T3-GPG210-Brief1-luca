using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "States/State")]
public class State : ScriptableObject
{
    public Action[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.gray;
    

    public void OnEnterState(StateController controller, StateData stateData)
    {
        if (actions.Length > 0)
        {
            foreach (Action action in actions)
            {
                ActionData data = (stateData?.actionData.ContainsKey(action)??false)?stateData.actionData[action]:null;
                action.OnEnter(controller, data);
            }
        }
        
    }
    
    public void UpdateState(StateController controller, StateData stateData)
    {
        DoActions(controller, stateData);
        CheckTransition(controller);
    }
    
    public void OnExitState(StateController controller, StateData stateData)
    {
        if (actions.Length > 0)
        {
            foreach (Action action in actions)
            {
                ActionData data = (stateData?.actionData.ContainsKey(action)??false)?stateData.actionData[action]:null;
                action.OnExit(controller, data);
            }
        }
        
    }

    private void DoActions(StateController controller, StateData stateData)
    {
        if (actions.Length > 0)
        {
            foreach (Action action in actions)
            {
                // TODO; Inperformant?
                ActionData data = (stateData?.actionData.ContainsKey(action)??false)?stateData.actionData[action]:null;
                action.Act(controller, data);
            }
        }
        
    }

    private void CheckTransition(StateController controller)
    {
        foreach (Transition transition in transitions)
        {
            bool decisionSucceeded = transition.decision.Decide(controller);
            State transitionTo = (decisionSucceeded ? transition.trueState : transition.falseState);
            float transitionTime = (decisionSucceeded
                ? transition.trueTransitionTime
                : transition.falseTransitionTime);
            controller.TransitionToState(transitionTo,transitionTime);

            // Break if a transition occurs - Make sure that not multiple transitions occur in the same frame
            if (transitionTo != controller.remainStateID)
                break;
        }
    }
}
