using System.Collections;
using System.Collections.Generic;
using Helper;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    public float desiredDistance = 15;
    public float minDistance = 3;
    public float maxDistanceStep = 0.5f;
    
    public float followSpeed = 2;
    

    public float rotationSpeed = 3;

    public float desiredHeightAboveTarget = 5;
    public float minHeightAboveTarget = 2;
    public float maxHeightAboveTarget = 15;
    public float maxHeightStep = 0.5f;
    
    [ShowInInspector]
    private float currentHeightAboveTarget = 5;
    [ShowInInspector]
    private float currentDistance = 0;
    [ShowInInspector]
    private float currentDesiredDistance = 0;
    public Vector3 currentDirection = Vector3.zero;
    public GameObject focusTarget;

    [ShowInInspector]
    private bool posChangedThisFrame = false;
    [ShowInInspector]
    private bool hadObstacleCollisionLastFrame = false;
    [ShowInInspector]
    private bool decrementHeight = true;
    
    private SlimeManager slimeManager;
    public PlayerInputManager playerInputManager;
    
    
    //TODO used for determing if no available height will reveil the focused slime (When an obstacle is in the way) 
    [ShowInInspector]
    private bool hitMaxHeightNoSuccess = false;
    [ShowInInspector]
    private bool hitMinHeightNoSuccess = false;
    [ShowInInspector]
    private bool focusTargetMovedSinceLastFrame = false;
    private Vector3 focusTargetPosLastFrame = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        slimeManager = GetComponent<SlimeManager>();
        if(slimeManager)
            slimeManager.onFocusChange += HandleFocusChanged;

        if (!playerInputManager)
            playerInputManager = GetComponent<PlayerInputManager>();

        currentDesiredDistance = desiredDistance;
    }


    // Update is called once per frame
    void Update()
    {
        if (focusTarget == null)
            return;
        
        focusTargetMovedSinceLastFrame = !focusTargetPosLastFrame.Equals(focusTarget.transform.position, .0001f);//0.0001f=>0.01m (Squared!)   focusTargetPosLastFrame != focusTarget.transform.position;

        posChangedThisFrame = false; // Reset var
        
        // Set if the camera will be risen or lowered if there's an obstacle
        if (!decrementHeight && currentHeightAboveTarget >= maxHeightAboveTarget)
        {
            decrementHeight = true;
        }else if (decrementHeight && currentHeightAboveTarget <= minHeightAboveTarget)
        {
            decrementHeight = false;
        }
        
        // Handle Player Input Rotation
        if (playerInputManager.inputRotateCameraLeft)
        {
            transform.RotateAround(focusTarget.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            posChangedThisFrame = true;
        }else if (playerInputManager.inputRotateCameraRight)
        {
            transform.RotateAround(focusTarget.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
            posChangedThisFrame = true;
        }

        // Calculate current Dir & Dist
        currentDistance = Vector3.Distance(transform.position, focusTarget.transform.position);
        currentDirection = focusTarget.transform.position - transform.position;
        
        // Moves the camera towards the focused slime
        MoveToTarget();

        
        bool isInDesiredHeight = Mathf.Approximately(currentHeightAboveTarget,desiredHeightAboveTarget);
        bool isInDesiredDistance = MathHelper.FApproximately(currentDistance, desiredDistance, 0.05f);
        // TODO Kind of HACKY; Made so the Raycast is only fired when the camera moved
        if (posChangedThisFrame || hadObstacleCollisionLastFrame || !isInDesiredHeight)
        {
            // Sight-Collision Check, Increases/Decreases height of the camera (Applied in the next frame)
            Debug.DrawRay(transform.position, currentDirection, Color.red);

            if (Physics.Raycast(transform.position, currentDirection, out var hit, (currentDistance +1f), Physics.DefaultRaycastLayers,QueryTriggerInteraction.Ignore))
            {
                
                if (hit.collider.gameObject != focusTarget)
                {
                    //TODO Add a var "Cant resolve with height... then try going closer with cam. currentDesiredDistance
                    if ((hitMaxHeightNoSuccess && hitMinHeightNoSuccess) || !isInDesiredDistance)
                    {
                        // TODO Go closer to focus 
                        if (currentDistance > minDistance)
                            currentDesiredDistance = Mathf.MoveTowards(currentDistance, minDistance, maxDistanceStep);
                        
                        /*
                        // Go towards desired Height
                        if(!isInDesiredHeight)
                            currentHeightAboveTarget = Mathf.MoveTowards(currentHeightAboveTarget, desiredHeightAboveTarget, maxHeightStep);*/
                        
                    }
                    else if (!decrementHeight)
                    {
                        currentHeightAboveTarget += maxHeightStep;
                        
                        if (currentHeightAboveTarget >= maxHeightAboveTarget)
                            hitMaxHeightNoSuccess = true;
                    }
                    else
                    {
                        currentHeightAboveTarget -= maxHeightStep;
                        
                        if (currentHeightAboveTarget <= minHeightAboveTarget)
                            hitMinHeightNoSuccess = true;
                    }

                    hadObstacleCollisionLastFrame = true;
                }
                else
                {
                    // Only reset if the target moved; TODO HACKY NOT WORKING AS DESIRED
                    if (focusTargetMovedSinceLastFrame)
                    {
                        hitMaxHeightNoSuccess = false;
                        hitMinHeightNoSuccess = false;
                    }
                    
                    // Move camera to the desired distance if not already
                    if (!MathHelper.FApproximately(/*currentDesiredDistance*/currentDistance, desiredDistance, 0.05f) && !hitMaxHeightNoSuccess && !hitMinHeightNoSuccess)
                        currentDesiredDistance = Mathf.MoveTowards(/*currentDesiredDistance*/currentDistance, desiredDistance, maxDistanceStep);

                    if (!isInDesiredHeight)
                    {
                        bool desireDecrement = currentHeightAboveTarget > desiredHeightAboveTarget;
                        currentHeightAboveTarget = Mathf.MoveTowards(currentHeightAboveTarget, desiredHeightAboveTarget, maxHeightStep);
/*

                        // Check if there was a collision last frame while moving in the same direction (To avoid Camera jitter/go forth & back)
                        if ((!(desireDecrement && decrementHeight && hadObstacleCollisionLastFrame) && !(!desireDecrement && !decrementHeight && hadObstacleCollisionLastFrame)))
                        {
                            currentHeightAboveTarget = Mathf.MoveTowards(currentHeightAboveTarget, desiredHeightAboveTarget, maxHeightStep);
                        }*/
                    }
                        
                    hadObstacleCollisionLastFrame = false;
                }
                
                /*if (hit.collider.gameObject != focusTarget && currentHeightAboveTarget+0.5f <= maxHeightAboveTarget)
                {
                    currentHeightAboveTarget += heightStep;
                }else if (currentHeightAboveTarget > desiredHeightAboveTarget)
                {
                    currentHeightAboveTarget -= heightStep;
                }*/
            }/*
            else if(!isInDesiredHeight)
            {
                currentHeightAboveTarget =
                    Mathf.MoveTowards(currentHeightAboveTarget, desiredHeightAboveTarget, heightStep);
            }*/
        }
        transform.LookAt(focusTarget.transform);
        focusTargetPosLastFrame = focusTarget.transform.position;
    }

    void MoveToTarget()
    {
        if (/*currentDistance < desiredDistance-.1 || currentDistance > desiredDistance+.1*/!MathHelper.FApproximately(currentDistance,currentDesiredDistance,0.05f) || !MathHelper.FApproximately(currentHeightAboveTarget,Mathf.Abs(currentDirection.y), 0.05f))
        {
            var desiredPos = focusTarget.transform.position + currentDesiredDistance * currentDirection.normalized *-1; //(currentDistance<desiredDistance?1:-1) *
            desiredPos.y = focusTarget.transform.position.y + currentHeightAboveTarget;
            transform.position = Vector3.Lerp(transform.position,desiredPos, /*Time.deltaTime * 500*/ currentDistance/followSpeed * Time.deltaTime);
            //Debug.Log("Wus " + desiredPos);
            posChangedThisFrame = true;
        }
    }

    void HandleFocusChanged(GameObject gameObject)
    {
        focusTarget = gameObject;
    }
}
