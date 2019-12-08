using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Helper;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class OrbitalCamera : MonoBehaviour
{
    [Header("References")]
    public PlayerInputManager inputManager;
    public SlimeManager slimeManager;
    
    [Header("Reference Points")]
    public Transform rotationPoint; // The camera will rotate around this axis
    public Transform focusPoint; // F.ex. Player
    
    
    [Header("Speed Settings")]
    public float maxRotationSpeed = 45;
    public float maxLookAtRotationSpeed = 30;
    public float maxMovementSpeed = 2;
    
    [Header("Distance Settings")]
    public float minDistanceToFocus = 5;
    public float minDistanceToRotPoint = 15;
    public float maxDistanceToRotPoint = 40; // TODO NOTIMPLEMENTED
    public float levelGroundExtentLength = 30; // TODO: AUtomatically get the value?
    
    
    public float defaultAngleToRotPoint = 10;
    [ShowInInspector]
    private float currentAngleToRotPoint = 0;
    
    [Header("Other Settings")]
    public float focusCamMaxAngle = 20; // When the angle between the focus & the cam (to the rotation point) is large than this, the cam will be rotated

    [HideInInspector]
    public bool autoDistance = true;
    
    private Vector3 dirCamToRotPoint = Vector3.zero;
    private Vector3 dirFocusToRotPoint = Vector3.zero;
    private Vector3 desiredCamToRotDir = Vector3.zero;
    
    private readonly Vector3 zeroYVec = new Vector3(1,0,1);

    // Start is called before the first frame update
    void Start()
    {
        if (inputManager == null)
            inputManager = GetComponent<PlayerInputManager>();
        if (slimeManager == null)
            slimeManager = GetComponent<SlimeManager>();

        if (slimeManager != null)
            slimeManager.onFocusChange += HandleSlimeFocusChange;
    }

    private void HandleSlimeFocusChange(GameObject gameobject)
    {
        focusPoint = gameobject.transform;
    }

    private void OnDestroy()
    {
        if (slimeManager != null)
            slimeManager.onFocusChange -= HandleSlimeFocusChange;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(focusPoint == null || rotationPoint == null)
            return;
            
        HandleHorizontalRotation();

        HandleCameraDistance();

        HandleCameraAngleToRotPoint();

        /*
        // TODO TEMP Rotation
        if (inputManager.inputRotateCameraLeft)
        {
            transform.RotateAround(rotationPoint.position, Vector3.up, maxRotationSpeed*Time.deltaTime);
        }else if (inputManager.inputRotateCameraRight)
        {
            transform.RotateAround(rotationPoint.position, Vector3.up, -maxRotationSpeed*Time.deltaTime);
        }*/
        
        // Look At Focus
        var targetRotation = Quaternion.LookRotation(focusPoint.position - transform.position);
        float lookAtSpeedMultiplier = Quaternion.Angle(targetRotation, transform.rotation) / 180;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeedMultiplier * maxLookAtRotationSpeed * Time.deltaTime);
        //transform.LookAt(focusPoint);
    }

    private void HandleHorizontalRotation()
    {
        Vector3 rotPointPos = rotationPoint.position;
        //// Check if the focus target is outside the "cam area"
        dirCamToRotPoint = rotPointPos - transform.position;
        dirFocusToRotPoint = rotPointPos - focusPoint.position;
        
        
        Debug.DrawLine(rotPointPos,rotPointPos - (Quaternion.Euler(0,focusCamMaxAngle,0)*dirCamToRotPoint));
        Debug.DrawLine(rotPointPos,rotPointPos - (Quaternion.Euler(0,-focusCamMaxAngle,0)*dirCamToRotPoint));
        Debug.DrawLine(rotPointPos,transform.position,Color.blue);
        Debug.DrawLine(rotPointPos,focusPoint.position,Color.green);
        
        // Calculate Angle between the two points
        float focusToCamAngle = Vector3.Angle(Vector3.Scale(dirCamToRotPoint.normalized,zeroYVec), Vector3.Scale(dirFocusToRotPoint.normalized,zeroYVec));

        // Set a new desired position (/direction) between the camera & the rotation point // TODO Need to make sure it only sets a new desired camtorotdir once until neccessary again
        if (focusToCamAngle >= focusCamMaxAngle)
        {
            desiredCamToRotDir = dirFocusToRotPoint;
        }
        
        //// Check if Camera needs to be rotated
        float currentCamToDesiredRotPointAngle = Vector3.Angle(Vector3.Scale(dirCamToRotPoint.normalized,zeroYVec), Vector3.Scale(desiredCamToRotDir.normalized,zeroYVec));
        if (currentCamToDesiredRotPointAngle > .5f)
        {
            int rotDir = MathHelper.AngleDir(dirCamToRotPoint, desiredCamToRotDir, Vector3.up); // -1: Left, 1: Right
            float speedMultiplier = currentCamToDesiredRotPointAngle / 180; // The further away, the faster it turns
            transform.RotateAround(rotationPoint.position, Vector3.up, speedMultiplier*rotDir*maxRotationSpeed*Time.deltaTime);
        }
    }

    private void HandleCameraDistance()
    {
        float distCamToRotPoint = dirCamToRotPoint.magnitude;
        float distCamToFocus = Vector3.Distance(transform.position, focusPoint.position);
        float distFocusToRotPoint = dirFocusToRotPoint.magnitude;

        if (autoDistance)
        {
            if (distCamToRotPoint > minDistanceToRotPoint && distCamToRotPoint > 1.5f*distFocusToRotPoint /*Hacky*/ && distCamToFocus > minDistanceToFocus && !Mathf.Approximately(distCamToFocus, minDistanceToFocus))
            {
                transform.position = Vector3.MoveTowards(transform.position, rotationPoint.position, maxMovementSpeed*Time.deltaTime);
            }
            else if((distCamToRotPoint < minDistanceToRotPoint || distCamToFocus < minDistanceToFocus || distCamToRotPoint < distFocusToRotPoint) && !Mathf.Approximately(distCamToFocus, minDistanceToFocus))
            {
                float speedMultiplier = distCamToFocus / levelGroundExtentLength;
                //transform.position += maxMovementSpeed*speedMultiplier*Time.deltaTime * -dirCamToRotPoint.normalized; //Vector3.MoveTowards(transform.position, -rotationPoint.position, 0.1f);
                transform.position = Vector3.MoveTowards(transform.position,
                    rotationPoint.position + -dirCamToRotPoint.normalized * maxDistanceToRotPoint,
                    maxMovementSpeed * speedMultiplier * Time.deltaTime);
            }
        }
        else
        {
            // TODO: If scroll input...?
        }
    }

    // TODO HACKY: This only works if the rotation point is BELOW the camera)
    private void HandleCameraAngleToRotPoint()
    {
        // Calculate angle between rotation point & camera
        Vector3 groundPointCamToRot = transform.position;
        groundPointCamToRot.y = rotationPoint.position.y;
        Vector3 groundDirToRotPoint = rotationPoint.position - groundPointCamToRot;
        
        currentAngleToRotPoint = Vector3.Angle(dirCamToRotPoint, groundDirToRotPoint);

        // Check for obstacle in the way, increase angle
        //Vector3 dirCamToFocusPoint = focusPoint.position - transform.position; // CAM CENTER TO FOCUS CENTER LINE
        /* TODO NOTE: Currently checking for obstacles between the Camera and the BOTTOM CENTER of the focus point so the cam will go up higher & the player sees more when the slime moves behind a wall */
        Vector3 focusPointCenterBottom = focusPoint.position;
        focusPointCenterBottom.y -= focusPoint.GetComponent<Renderer>().bounds.extents.y - 0.1f; // HACK
        Vector3 dirCamToFocusPoint = focusPointCenterBottom - transform.position; // CAM CENTER TO FOCUS BOTTOM LINE
        float distCamToFocusPoint = Vector3.Distance(focusPointCenterBottom/*focusPoint.position*/, transform.position);
        Debug.DrawRay(transform.position, dirCamToFocusPoint.normalized * distCamToFocusPoint, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dirCamToFocusPoint, out hit,distCamToFocusPoint)) // TODO INPERFORMANT; Don't check every frame?
        {
            if (!hit.collider.CompareTag("Player")) // Something's in the way!
            {
                // Increase height = this will increase the angle to the rot point. Distance is handled elsewhere!
                transform.position += Time.deltaTime * 3 * Vector3.up; // TODO HACKY. Make variable
                return;
            }

        }
        
        // Go closer if not in default angle
        if (!MathHelper.FApproximately(currentAngleToRotPoint, defaultAngleToRotPoint, .1f))
        {
            Vector3 futurePosition = transform.position + Time.deltaTime *3 * (currentAngleToRotPoint > defaultAngleToRotPoint?-1:1) * Vector3.up;
            Vector3 dirFuturePosToFocus = /*focusPoint.position*/focusPointCenterBottom - futurePosition;
            float distFutureCamPosToFocusPoint = Vector3.Distance(/*focusPoint.position*/focusPointCenterBottom, futurePosition);
            if (Physics.Raycast(futurePosition, dirFuturePosToFocus, out hit, distFutureCamPosToFocusPoint)) // TODO INPERFORMANT; Don't check every frame?
            {
                if (hit.collider.CompareTag("Player")) // Something's in the way!
                {
                    transform.position = futurePosition;
                    return;
                }
            }
        }
    }
}
