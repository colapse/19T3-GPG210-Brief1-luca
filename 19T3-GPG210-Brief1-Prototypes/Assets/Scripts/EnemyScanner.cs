using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{
    public bool isPlayer = false; //hacky
    public SlimeInputManager slimeInputManager; // TODO Hack
    public Slime slime;

    private void Start()
    {
        // SUPER HACKY 
        if (slimeInputManager == null)
        {
            slimeInputManager = GetComponentInParent<SlimeInputManager>();
            if (slimeInputManager == null)
            {
                slimeInputManager = GameObject.Find("Main Camera")?.GetComponent<SlimeInputManager>(); // Super ugly hacky
            }
        }
        if (slime == null)
        {
            
            slime = GetComponentInParent<Slime>();
            //slime = slimeInputManager?.currentTarget;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!slimeInputManager || other.CompareTag("Player") == isPlayer) return;
        var enemySlime = other.GetComponent<Slime>();
        if (enemySlime == null) return;
        slimeInputManager.AddEnemyInsight(slime,enemySlime);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!slimeInputManager || other.CompareTag("Player") == isPlayer) return;
        var enemySlime = other.GetComponent<Slime>();
        if (enemySlime == null) return;
        slimeInputManager.RemoveEnemyInsight(slime,enemySlime);
    }
}
