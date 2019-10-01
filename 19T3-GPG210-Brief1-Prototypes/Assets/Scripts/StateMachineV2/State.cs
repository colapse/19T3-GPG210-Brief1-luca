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
        foreach (Action action in actions)
        {
            ActionData data = stateData.actionData.ContainsKey(action)?stateData.actionData[action]:null;
            action.OnEnter(controller, data);
        }
    }
    
    public void UpdateState(StateController controller, StateData stateData)
    {
        DoActions(controller, stateData);
        CheckTransition(controller);
    }
    
    public void OnExitState(StateController controller, StateData stateData)
    {
        foreach (Action action in actions)
        {
            ActionData data = stateData.actionData.ContainsKey(action)?stateData.actionData[action]:null;
            action.OnExit(controller, data);
        }
    }

    private void DoActions(StateController controller, StateData stateData)
    {
        foreach (Action action in actions)
        {
            // TODO; Inperformant?
            ActionData data = stateData.actionData.ContainsKey(action)?stateData.actionData[action]:null;
            action.Act(controller, data);
        }
    }

    private void CheckTransition(StateController controller)
    {
        foreach (Transition transition in transitions)
        {
            bool decisionSucceeded = transition.decision.Decide(controller);
            controller.TransitionToState((decisionSucceeded ? transition.trueState : transition.falseState), (decisionSucceeded ? transition.trueTransitionTime : transition.falseTransitionTime));
        }
    }
}
