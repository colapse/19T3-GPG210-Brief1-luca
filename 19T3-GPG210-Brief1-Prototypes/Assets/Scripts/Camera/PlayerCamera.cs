using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    public float desiredDistance = 15;
    public float currentDistance = 0;
    public float followSpeed = 2;

    public float desiredHeightAboveTarget = 5;
    public float maxHeightAboveTarget = 15;
    public float currentHeightAboveTarget = 5;
    
    public GameObject focusTarget;

    
    private SlimeManager slimeManager;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        slimeManager = GetComponent<SlimeManager>();
        slimeManager.onFocusChange += HandleFocusChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (focusTarget == null)
            return;

        currentDistance = Vector3.Distance(transform.position, focusTarget.transform.position);
        
        MoveToTarget();
        
        transform.LookAt(focusTarget.transform);
    }

    void MoveToTarget()
    {
        if (currentDistance < desiredDistance-.1 || currentDistance > desiredDistance+.1)
        {
            var dir = focusTarget.transform.position - transform.position;
            var desiredPos = focusTarget.transform.position + desiredDistance * dir.normalized *-1; //(currentDistance<desiredDistance?1:-1) *
            desiredPos.y = focusTarget.transform.position.y + currentHeightAboveTarget;
            transform.position = Vector3.Lerp(transform.position,desiredPos, /*Time.deltaTime * 500*/ currentDistance/followSpeed * Time.deltaTime);
            //Debug.Log("Wus " + desiredPos);

            Debug.DrawRay(transform.position, dir, Color.red);
            if (Physics.Raycast(transform.position, dir, out var hit))
            {
                if (hit.collider.gameObject != focusTarget && currentHeightAboveTarget+0.5f <= maxHeightAboveTarget)
                {
                    currentHeightAboveTarget += .5f;
                }else if (currentHeightAboveTarget > desiredHeightAboveTarget)
                {
                    currentHeightAboveTarget -= .5f;
                }
            }
            
            //TODO: Rotation?
        }
    }

    void HandleFocusChanged(GameObject gameObject)
    {
        focusTarget = gameObject;
    }
}
