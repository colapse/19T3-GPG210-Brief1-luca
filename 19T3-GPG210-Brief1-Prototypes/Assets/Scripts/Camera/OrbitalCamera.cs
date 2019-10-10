using System;
using System.Collections;
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
    public float minDistanceToFocus = 10;
    public float minDistanceToRotPoint = 30;
    public float levelGroundExtentLength = 30; // TODO: AUtomatically get the value?

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

        HandleHorizontalRotation();

        HandleCameraDistance();

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
        //// Check if the focus target is outside the "cam area"
        dirCamToRotPoint = rotationPoint.position - transform.position;
        dirFocusToRotPoint = rotationPoint.position - focusPoint.position;
        
        
        Debug.DrawLine(rotationPoint.position,rotationPoint.position - (Quaternion.Euler(0,focusCamMaxAngle,0)*dirCamToRotPoint));
        Debug.DrawLine(rotationPoint.position,rotationPoint.position - (Quaternion.Euler(0,-focusCamMaxAngle,0)*dirCamToRotPoint));
        Debug.DrawLine(rotationPoint.position,transform.position,Color.blue);
        Debug.DrawLine(rotationPoint.position,focusPoint.position,Color.green);
        
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
            if (distCamToRotPoint > minDistanceToRotPoint && distCamToRotPoint > 2.5f*distFocusToRotPoint /*Hacky*/ && distCamToFocus > minDistanceToFocus && !Mathf.Approximately(distCamToFocus, minDistanceToFocus))
            {
                transform.position = Vector3.MoveTowards(transform.position, rotationPoint.position, maxMovementSpeed*Time.deltaTime);
            }
            else if((distCamToRotPoint < minDistanceToRotPoint || distCamToFocus < minDistanceToFocus || distCamToRotPoint < distFocusToRotPoint) && !Mathf.Approximately(distCamToFocus, minDistanceToFocus))
            {
                float speedMultiplier = distCamToFocus / levelGroundExtentLength;
                transform.position += maxMovementSpeed*speedMultiplier*Time.deltaTime * -dirCamToRotPoint.normalized; //Vector3.MoveTowards(transform.position, -rotationPoint.position, 0.1f);
            }
        }
        else
        {
            // TODO: If scroll input...
        }
    }
}
