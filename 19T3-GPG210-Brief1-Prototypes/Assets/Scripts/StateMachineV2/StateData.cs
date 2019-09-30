using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateData
{
    public Dictionary<Action, ActionData> actionData;

    public StateData()
    {
        actionData = new Dictionary<Action, ActionData>();
    }
}
