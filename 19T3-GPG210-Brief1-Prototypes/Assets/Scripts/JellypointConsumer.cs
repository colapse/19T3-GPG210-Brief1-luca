using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class JellypointConsumer : MonoBehaviour
{
    public PlayerData playerData;
    
    // Start is called before the first frame update
    void Start()
    {
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
        var jp = other.GetComponent<Jellypoint>();
        if (jp == null || playerData == null) return;
        playerData.Points += jp.points;
        Destroy(jp.gameObject);
    }
}
