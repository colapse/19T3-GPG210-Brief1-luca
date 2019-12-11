using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using World;

public class JellypointConsumer : MonoBehaviour
{
    public PlayerData playerData;

    public float maxDistanceToEatSetting = -1f; // Temporary Hack, Enemy Scanner Trigger enables slime to eat from far otherwise. 
    [ShowInInspector, ReadOnly] private float maxDistanceToEat = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        maxDistanceToEat = maxDistanceToEatSetting < 0 ? GetComponent<MeshCollider>()?.bounds.size.z * transform.lossyScale.z + 0.2f ?? 2 : maxDistanceToEatSetting; // Temporary Hack
        
        if (playerData == null)
        {
            playerData = FindObjectOfType<WorldManager>()?.playerData;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*var jp = other.GetComponent<Jellypoint>();
        Debug.Log("Dist: "+Vector3.Distance(transform.position, other.transform.position)+" > "+maxDistanceToEat);
        if (jp == null || playerData == null || Vector3.Distance(transform.position, other.transform.position) > maxDistanceToEat) return;
        playerData.Points += jp.points;
        Destroy(jp.gameObject);*/
    }
}
