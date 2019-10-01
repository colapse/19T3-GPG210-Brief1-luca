using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{
    
    public SlimeInputManager slimeInputManager; // TODO Hack

    private void OnTriggerEnter(Collider other)
    {
        if (slimeInputManager && other.CompareTag("Player"))
        {
            Slime slime = other.GetComponent<Slime>();
            if (slime != null)
            {
                slimeInputManager.enemiesInSight.Add(slime);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (slimeInputManager && other.CompareTag("Player"))
        {
            Slime slime = other.GetComponent<Slime>();
            if (slime != null && slimeInputManager.enemiesInSight.Contains(slime))
            {
                slimeInputManager.enemiesInSight.Remove(slime);
            }
        }
    }
}
