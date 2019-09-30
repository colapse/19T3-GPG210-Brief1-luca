using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public StringObjectDictionary data;
    
    public abstract void OnEnter(StateController controller, ActionData actionData);
    public abstract void Act(StateController controller, ActionData actionData);
    public abstract void OnExit(StateController controller, ActionData actionData);
}
