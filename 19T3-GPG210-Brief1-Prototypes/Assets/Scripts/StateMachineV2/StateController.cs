using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public class StateController : MonoBehaviour
{
    public SlimeInputManager inputManager;
    Dictionary<State, StateData> stateDataDict;
    
    public State currentState;
    [FormerlySerializedAs("lastState")] public State previousState;
    public State remainStateID;
    public State lastStateID;
    
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<SlimeInputManager>();
        stateDataDict = new Dictionary<State, StateData>();
        
        StoreStateData(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.UpdateState(this, (stateDataDict.ContainsKey(currentState)?stateDataDict[currentState]:null));
    }

    public void TransitionToState(State newState, float transitionTime)
    {
        if (newState.Equals(remainStateID))
        {
            return;
        }
        StartCoroutine(TransitToState(newState, transitionTime));
    }

    IEnumerator TransitToState(State newState, float transitionTime)
    {
        State tmpCurrentState = currentState;
        currentState = null;
        
        if(transitionTime > 0)
            yield return new WaitForSeconds(transitionTime);
        
        State nextState = newState;

        /*
        if (newState == remainStateID)
        {
            currentState = tmpCurrentState;
            yield return 0;
        }
        else */if (newState == lastStateID && previousState != null)
            nextState = previousState;

        previousState = tmpCurrentState;
        StoreStateData(nextState);
        
        tmpCurrentState?.OnExitState(this, (stateDataDict.ContainsKey(tmpCurrentState)?stateDataDict[tmpCurrentState]:null));
        
        currentState = nextState;
        
        currentState?.OnEnterState(this, (stateDataDict.ContainsKey(tmpCurrentState)?stateDataDict[tmpCurrentState]:null));

        yield return 0;
    }

    private void StoreStateData(State state)
    {
        if (state == null || stateDataDict.ContainsKey(state) || state.actions.Length == 0)
            return;

        stateDataDict.Add(state,new StateData());
        foreach (var action in state.actions)
        {
            if (action.data != null && action.data.Count > 0)
            {
                ActionData ad = new ActionData();
                ad.objectData = new Dictionary<string, object>(action.data);
                /*
                foreach (var data in action.data)
                {
                    ad.objectData.Add(data.Key,data.Value);
                }*/
                stateDataDict[state].actionData.Add(action,ad);
            }
        }
    }
}
