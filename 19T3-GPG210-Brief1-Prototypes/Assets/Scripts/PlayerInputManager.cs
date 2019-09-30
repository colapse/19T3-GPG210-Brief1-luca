using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerInputManager : SlimeInputManager
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Reset Some Vars
        inputJump = false;
        
        // Capture Input
        
        // Input Jump
        if (Input.GetKeyUp(KeyCode.Space))
        {
            inputJump = true;
        }

        // Input Forward Jump / Hop
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            inputForwardJump = true;
        }else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            inputForwardJump = false;
        }

        // Input Feed
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            inputFeed = true;
        }else if (Input.GetKeyUp(KeyCode.Backspace))
        {
            inputFeed = false;
        }

        // Input Turn Left
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            inputTurnLeft = true;
        }else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            inputTurnLeft = false;
        }
        
        // Input Turn Right
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            inputTurnRight = true;
        }else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            inputTurnRight = false;
        }
            
    }
}
