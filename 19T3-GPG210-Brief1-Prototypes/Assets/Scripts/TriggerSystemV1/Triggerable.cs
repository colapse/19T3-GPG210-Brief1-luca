using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Triggerable : MonoBehaviour
{
    [System.Serializable]
    public class OnStateChangedEvent : UnityEvent<bool> {}
    public OnStateChangedEvent onStateChanged;
    /*
    public delegate void StateChangedDel (bool active);
    public StateChangedDel onStateChanged;
      
    public void NotifyStateChanged(bool newState)
    {
        if(onStateChanged != null)
            onStateChanged (newState);
    }*/
    
    private bool active = false;
    public bool Active
    {
        get => active;
        set {
            active = value;
            //NotifyStateChanged(value);
            if (onStateChanged != null)
            {
                Debug.Log("WUSH SEND "+value);
                onStateChanged.Invoke(active);
            }
            
            //Debug.Log("Triggerable State Changed to: "+active);
        }
    }

    // Required States of Triggers
    public TriggerIntDictionary triggerStateRequirements;
    private TriggerBoolDictionary currentTriggerStates;
    
    // Triggers AND/OR (If true, all triggerstate requirements in triggerStateRequirements must be met, otherwise only one)
    public bool allTriggersTrue = true;
    
    // Start is called before the first frame update
    private void Start()
    {
        currentTriggerStates = new TriggerBoolDictionary();
        
        if (triggerStateRequirements != null)
        {
            foreach (var trigger in triggerStateRequirements.Keys)
            {
                if (trigger != null)
                {
                    trigger.onStateChanged += TriggerStateChanged;
                }
            }
        }
    }

    private void TriggerStateChanged(Trigger trigger, int newState)
    {
        if (!triggerStateRequirements.ContainsKey(trigger)) return;
        var requirementMet = (newState == triggerStateRequirements[trigger]);
        Debug.Log("CHANGEER "+ requirementMet);
        if (currentTriggerStates.ContainsKey(trigger))
            currentTriggerStates[trigger] = requirementMet;
        else
            currentTriggerStates.Add(trigger,requirementMet);
        
        CheckRequirements();
    }

    private void CheckRequirements()
    {
        if (currentTriggerStates != null)
        {
            foreach (var trigger in currentTriggerStates.Values)
            {
                Debug.Log(trigger);
            }
        }
        
        Debug.Log("CHANGEER2 "+ ((allTriggersTrue && !currentTriggerStates.Values.Contains(false)) ||
                  (!allTriggersTrue && currentTriggerStates.Contains(true))) + "    "+currentTriggerStates.Values.Contains(false));
        Active = (allTriggersTrue && !currentTriggerStates.Values.Contains(false)) ||
                 (!allTriggersTrue && currentTriggerStates.Values.Contains(true)) ? true : false;
        /*
        if (allTriggersTrue && !currentTriggerStates.Contains(false))
        {
            active = true;
        }
        else if(!allTriggersTrue && currentTriggerStates.Contains(true))
        {
            active = true;
        }
        else
        {
            active = false;
        }*/
    }
}
