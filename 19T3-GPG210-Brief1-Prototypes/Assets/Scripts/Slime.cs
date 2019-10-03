using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour
{
    public static int minSlimeVolume = 1;
    public static int maxSlimeVolume = 5;
    public static GameObject enemySlimePrefab; // TODO, maybe outsource to a object pool; Bad location Hacky.

    public float initialAngle; // TODO? Split force claculation
    public float splitForceMultiplier = 300;
    public bool freeFeed = false;
    
    public Rigidbody rb;
     [SerializeField] private float volume = 1;
    public float Volume
    {
        get => volume;
        set
        { OnVolumeChanged(value); volume = value; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Volume != 0)
        {
            OnVolumeChanged(Volume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnVolumeChanged(float newValue)
    {
        transform.localScale = Vector3.one * newValue * 0.5f;
    }

    private void OnCollisionEnter(Collision other)
    {
        Slime otherSlime = other.collider.GetComponent<Slime>();
        if (otherSlime != null && (other.impulse.magnitude > 2 || freeFeed || otherSlime.freeFeed))
        {
            if (otherSlime.Volume > volume)
            {
                // This Slime is smaller, gets eaten
                //StartCoroutine(DeleteSlime()); // Ugly solution. Waits for a second to make sure the CollisionEnter method in the other slime is fully executed
                Destroy(gameObject);
            }else if (otherSlime.Volume < volume)
            {
                // This Slime is bigger, eats the smaller one
                Volume += otherSlime.Volume;
            }
            else
            {
                // Same Size nothig happens
            }
            Debug.Log("Collision between Slimes "+Volume+" Impulse: "+other.impulse.magnitude+" Rel. Velocity: "+other.relativeVelocity);
        }
    }

    IEnumerator DeleteSlime()
    {
        yield return new WaitForSeconds(1);
        
        Destroy(gameObject);
        yield return 0;
    }
    
    
    public void SplitSlime(Vector3 requiredForce)
    {
        if (Volume >= minSlimeVolume * 2)
        {
            Renderer slimeRenderer = GetComponent<Renderer>();
            Vector3 newSlimeSpawnPos = transform.position + transform.forward.normalized * (slimeRenderer.bounds.extents.z);
            
            GameObject newSlimeObj = Instantiate(gameObject,newSlimeSpawnPos,transform.rotation);
            Slime newSlime = newSlimeObj.GetComponent<Slime>();
            newSlime.Volume = Volume / 2;
            Volume /= 2;

            if (requiredForce != Vector3.zero)
            {
                newSlime.rb.AddForce(requiredForce, ForceMode.Impulse);
            }
            else
            {
                // Push slimes apart from each other
                Vector3 forwardForce = newSlime.Volume * splitForceMultiplier * transform.forward;
                forwardForce.y = newSlime.Volume * 50;
                newSlime.rb.AddForce(forwardForce);
                //slime.rb.AddForce(slime.transform.right*-30*newSlime.Volume);
            }
            
        }
    }

    // TODO Hacky; Shouldnt be in here. Should go into some kind of helper class
    public float CalculateForceNeeded(float jumpHeight)
    {
        return Mathf.Sqrt(2 * jumpHeight * Physics.gravity.y);
    }

    // https://forum.unity.com/threads/how-to-calculate-force-needed-to-jump-towards-target-point.372288/
    public Vector3 CalculateSplitForceNeeded(Vector3 targetPosition)
    {
        
        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;
 
        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(targetPosition.x, 0, targetPosition.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);
 
        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - targetPosition.y;
 
        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
 
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
 
        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
 
        return finalVelocity * rb.mass;
 
        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }
}
