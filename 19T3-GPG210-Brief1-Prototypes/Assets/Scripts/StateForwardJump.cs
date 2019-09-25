using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateForwardJump : StateBase
{
    Rigidbody rb;
    float distanceToGround = 0;

    private float jumpCharge = 1;
    private bool jumped = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        jumpCharge = 1;
        jumped = false;
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        
    }

    public override void Exit()
    {
        
    }

    public override void Execute()
    {
        if (jumpCharge < 3 && Input.GetKey(KeyCode.Alpha1))
        {
            jumpCharge += Time.deltaTime*6;
        }
        
        if (!jumped && (Input.GetKeyUp(KeyCode.Alpha1) || jumpCharge >= 3))
        {
            Vector3 forwardJumpForce = transform.forward * 100;
            forwardJumpForce.y = 100;
        
            rb.AddForce(forwardJumpForce*jumpCharge);
            jumped = true;
        }
        
        if (IsGrounded() && rb.velocity.y < 0 && jumped)
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
