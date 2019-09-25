using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : StateBase
{
    Rigidbody rb;
    float distanceToGround = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }

    public override void Enter()
    {
        if (rb == null)
            return;

        rb.AddForce(Vector3.up * 300);
    }

    public override void Exit()
    {
        Debug.Log("End");
    }

    public override void Execute()
    {
        if (IsGrounded() && rb.velocity.y < 0)
        {
            GetComponent<StateManager>()?.ChangeState(nextState);
        }
    }

    //Ugly
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
}
