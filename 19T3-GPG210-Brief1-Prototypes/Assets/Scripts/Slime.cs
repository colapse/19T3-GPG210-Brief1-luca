using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour
{
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
        transform.localScale = Vector3.one * newValue;
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
}
