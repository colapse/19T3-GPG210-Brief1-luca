using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRotateLeft : StateBase
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Execute()
    {
        transform.Rotate(Vector3.up,-5);
        
        if(Input.GetKeyUp(KeyCode.LeftArrow))
            GetComponent<StateManager>()?.ChangeState(nextState);
    }
}
