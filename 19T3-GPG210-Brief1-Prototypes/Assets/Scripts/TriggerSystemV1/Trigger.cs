using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public delegate void StateChangedDel (Trigger trigger, int newState);
    public StateChangedDel onStateChanged;
      
    public void NotifyStateChanged(int newState)
    {
        if(onStateChanged != null)
            onStateChanged (this, newState);
    }

    public int states = 2;
    
    [SerializeField]
    private int currentState = 0;

    public int CurrentState
    {
        get => currentState;
        set
        {
            currentState = value > states ? states : (value < 0 ? 0 : value);
            NotifyStateChanged(currentState);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
