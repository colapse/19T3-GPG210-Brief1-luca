using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : StateBase
{
    public override void Enter()
    {
        Debug.Log("Enter Idle");
    }

    public override void Exit()
    {
        Debug.Log("Exit Idle");
    }

    public override void Execute()
    {
        transform.localScale = new Vector3(Mathf.PerlinNoise(Time.time,0), Mathf.PerlinNoise(Time.time, 0), Mathf.PerlinNoise(Time.time, 0));
    }
}
