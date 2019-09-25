using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public StateBase currentState;
    public StateBase defaultState;

    public StateBase jumpState;
    public StateBase idleState;
    public StateBase randomPushState;
    public StateBase rotateLeftState;
    public StateBase rotateRightState;

    private Slime slime;
    private SlimeManager sm;
    private bool isActiveSlime = false;
    
    // Start is called before the first frame update
    void Start()
    {
        slime = GetComponent<Slime>();
        sm = Camera.main?.gameObject.GetComponent<SlimeManager>();
        currentState = defaultState;
    }

    // Update is called once per frame
    void Update()
    {
        isActiveSlime = (sm.activeSlime == slime);
        
        if(currentState != null)
            currentState.Execute();

        if (Input.GetKeyUp(KeyCode.Space) && isActiveSlime)
        {
            ChangeState(jumpState);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && isActiveSlime)
        {
            ChangeState(randomPushState);
        }
        
        if(Input.GetKeyDown(KeyCode.LeftArrow) && isActiveSlime)
            ChangeState(rotateLeftState);
        
        if(Input.GetKeyDown(KeyCode.RightArrow) && isActiveSlime)
            ChangeState(rotateRightState);
    }

    public void ChangeState(StateBase newState)
    {
        if (currentState == newState)
            return;

        if (currentState != null)
            currentState.Exit();

        if (newState != null)
            newState.Enter();

        currentState = newState;
    }
}
