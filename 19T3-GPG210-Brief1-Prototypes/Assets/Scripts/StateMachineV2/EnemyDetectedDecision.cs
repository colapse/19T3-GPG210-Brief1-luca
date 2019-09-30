﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Decisions/EnemyDetected")]
public class EnemyDetectedDecision : Decision
{
    public bool detected = false; // TEMP TESTING HACK
    public override bool Decide(StateController controller)
    {
        return EnemyDetected(controller);
    }

    private bool EnemyDetected(StateController controller)
    {
      // Check if player is detected
      return detected;
    }
}