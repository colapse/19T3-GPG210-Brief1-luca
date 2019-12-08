using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortalObjectWrapper : MonoBehaviour
{
    public BoxCollider col;
    
    private bool doGroundedChecks = false;
    private float boundExtents = 1;

    public float maxGroundCheckInterval = 0.2f;
    private float groundCheckCooldown = 0;

    private bool doNoChildChecks = true;
    public float noChildCheckInterval = 5f;
    private float noChildCheckCooldown = 1f;
    public float minColliderExtent = 0.5f;

    public event Action<PortalObjectWrapper> OnObjectLanded;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (doGroundedChecks && groundCheckCooldown <= 0 && IsOnGround()) // super hacky
        {
            OnObjectLanded?.Invoke(this);
            doGroundedChecks = false;
            groundCheckCooldown = maxGroundCheckInterval;
        }

        if (groundCheckCooldown > 0)
        {
            groundCheckCooldown -= Time.deltaTime;
        }

        if (noChildCheckCooldown > 0)
        {
            noChildCheckCooldown -= Time.deltaTime;
        }
        else if(doNoChildChecks)
        {
            if (transform.childCount <= 0)
            {
                OnObjectLanded?.Invoke(this);
            }
            noChildCheckCooldown = noChildCheckInterval;
        }
    }

    private void OnEnable()
    {
        doGroundedChecks = false;
        doNoChildChecks = true;
        boundExtents = 1;
        col.enabled = false;
        noChildCheckCooldown = noChildCheckInterval;
    }

    public void Initialize()
    {
        if (transform.childCount > 0)
        {
            Vector3 maxChildExtents = Vector3.zero;
            bool childrenHaveColliders = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Collider childCol = child.GetComponent<Collider>();
                if (col != null)
                {
                    if (!childCol.isTrigger)
                    {
                        childrenHaveColliders = true;
                        maxChildExtents = Vector3.Max(maxChildExtents, childCol.bounds.extents);
                    }
                        
                }

                if (!childrenHaveColliders || maxChildExtents.Equals(Vector3.zero))
                {
                    Renderer r = child.GetComponent<Renderer>();

                    if (r != null)
                    {
                        maxChildExtents = Vector3.Max(maxChildExtents, r.bounds.extents);
                    }
                    else
                    {
                        Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
                        if (renderers != null && renderers.Length > 0)
                        {
                            foreach (var t in renderers)
                            {
                                maxChildExtents = Vector3.Max(maxChildExtents, t.bounds.extents);
                            }
                        }
                        
                    }
                }
            }

            boundExtents = (new float[]
            {
                maxChildExtents.x,
                maxChildExtents.y,
                maxChildExtents.z
            }).Max(); // Super Hacky
            
            
            
            if (!childrenHaveColliders)
            {
                if (maxChildExtents.Equals(Vector3.zero))
                    maxChildExtents = Vector3.one;

                maxChildExtents.x = maxChildExtents.x < minColliderExtent ? minColliderExtent : maxChildExtents.x;
                maxChildExtents.y = maxChildExtents.y < minColliderExtent ? minColliderExtent : maxChildExtents.y;
                maxChildExtents.z = maxChildExtents.z < minColliderExtent ? minColliderExtent : maxChildExtents.z;
                
                col.enabled = true;
                col.size = maxChildExtents;
                
            }
            else
            {
                col.enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        doGroundedChecks = true;
    }

    private bool IsOnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 2* boundExtents)) // HACKY
        {
            if (hit.distance < boundExtents + 1f)
            {
                return true;
            }
        }

        return false;
    }
}
