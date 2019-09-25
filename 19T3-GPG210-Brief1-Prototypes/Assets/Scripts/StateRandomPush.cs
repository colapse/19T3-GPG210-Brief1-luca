using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateRandomPush : StateBase
{
    //public float duration = 5;
    //public float timer = 0;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        //timer = duration;
        rb.AddForce(new Vector3(Random.Range(-300,300), 0, Random.Range(-300,300)));
        GetComponent<StateManager>()?.ChangeState(nextState);
    }

    public override void Exit()
    {
    }

    public override void Execute()
    {
        //timer -= Time.deltaTime;
        //rb.AddForce(new Vector3(Random.Range(-70,70), 0, Random.Range(-70,70)));

        //if (timer < 0)
        //{
            //GetComponent<StateManager>()?.ChangeState(nextState);
        //}
    }
}
