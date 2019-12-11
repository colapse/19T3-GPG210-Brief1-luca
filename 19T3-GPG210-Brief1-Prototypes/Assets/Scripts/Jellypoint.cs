using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellypoint : MonoBehaviour
{
    public int points = 1;


    private void OnTriggerEnter(Collider other)
    {
        var jpc = other.GetComponent<JellypointConsumer>();
        if (jpc == null) return;
        jpc.playerData.Points += points;
        Destroy(gameObject);
    }
}
