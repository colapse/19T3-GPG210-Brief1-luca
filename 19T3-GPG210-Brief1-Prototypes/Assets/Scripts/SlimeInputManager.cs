using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class SlimeInputManager : MonoBehaviour
{
    public bool inputJump = false;
    public bool inputForwardJump = false;
    public bool inputTurnLeft = false;
    public bool inputTurnRight = false;
    public bool inputSplit = false;
    public bool inputFeed = false;

    public List<Slime> enemiesInSight;
    public Slime currentTarget;

    public bool wasGroundedLastFrame = false; // Hack. shouldnt be in here

    private void Start()
    {
        enemiesInSight = new List<Slime>();
    }
    

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        // TODO HACK (The player inputmanager is on the camera... ==> IsGroundedCheck throws errors & would make no sense)
        if(!gameObject.CompareTag("MainCamera"))
            wasGroundedLastFrame = IsGrounded();
    }

    // HACK. Shouldnt be in here
    public bool IsGrounded()
    {
        float distanceToGround = GetComponent<Collider>().bounds.extents.y;
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }

    // HACK. SHOULDNT BE IN THERE
    public static bool IsUpright(Transform obj)
    {
        return !(obj.rotation.eulerAngles.x < -60 ||
                 (obj.rotation.eulerAngles.x > 60 && obj.rotation.eulerAngles.x < 300) ||
                obj.rotation.eulerAngles.z < -60 ||
                 (obj.rotation.eulerAngles.z > 60 && obj.rotation.eulerAngles.z < 300));
    }
}
