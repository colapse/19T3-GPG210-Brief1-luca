using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerInputManager : SlimeInputManager
{
    public bool inputRotateCameraLeft = false;
    public bool inputRotateCameraRight = false;

    public KeyCode actionJump = KeyCode.Space;
    public KeyCode actionForwardJump = KeyCode.W;
    public KeyCode actionFeed = KeyCode.F;
    public KeyCode actionRotateLeft = KeyCode.A;
    public KeyCode actionRotateRight = KeyCode.D;
    public KeyCode actionRotateCamLeft = KeyCode.Q;
    public KeyCode actionRotateCamRight = KeyCode.E;
    public KeyCode actionSplit = KeyCode.Return;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Reset Some Vars
        //inputJump = false;
        
        // Capture Input
        
        // Input Jump
        if (Input.GetKeyDown(actionJump))
        {
            inputJump = true;
        }else if(Input.GetKeyUp(actionJump))
        {
            inputJump = false;
        }

        // Input Forward Jump / Hop
        if (Input.GetKeyDown(actionForwardJump))
        {
            inputForwardJump = true;
        }else if (Input.GetKeyUp(actionForwardJump))
        {
            inputForwardJump = false;
        }

        // Input Feed
        if (Input.GetKeyDown(actionFeed))
        {
            inputFeed = true;
        }else if (Input.GetKeyUp(actionFeed))
        {
            inputFeed = false;
        }

        // Input Turn Left
        if (Input.GetKeyDown(actionRotateLeft))
        {
            inputTurnLeft = true;
        }else if (Input.GetKeyUp(actionRotateLeft))
        {
            inputTurnLeft = false;
        }
        
        // Input Turn Right
        if (Input.GetKeyDown(actionRotateRight))
        {
            inputTurnRight = true;
        }else if (Input.GetKeyUp(actionRotateRight))
        {
            inputTurnRight = false;
        }

        // Input Rotate Cam Left
        if (Input.GetKeyDown(actionRotateCamLeft))
        {
            inputRotateCameraLeft = true;
        }
        else if(Input.GetKeyUp(actionRotateCamLeft))
        {
            inputRotateCameraLeft = false;   
        }

        // Input Rotate Cam Left
        if (Input.GetKeyDown(actionRotateCamRight))
        {
            inputRotateCameraRight = true;
        }
        else if(Input.GetKeyUp(actionRotateCamRight))
        {
            inputRotateCameraRight = false;   
        }

        // Input Split
        if (Input.GetKeyDown(actionSplit))
        {
            inputSplit = true;
        }
        else if(Input.GetKeyUp(actionSplit))
        {
            inputSplit = false;   
        }
            
    }
}
