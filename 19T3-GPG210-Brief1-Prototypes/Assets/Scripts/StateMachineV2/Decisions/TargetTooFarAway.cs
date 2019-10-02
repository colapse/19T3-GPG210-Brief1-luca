using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Decisions/TargetTooFarAway")]
public class TargetTooFarAway : Decision
{
    public float farDistance;

    public override bool Decide(StateController controller)
    {
        if (controller.inputManager.currentTarget == null)
            return false;

        var distance = Vector3.Distance(controller.inputManager.currentTarget.transform.position,
            controller.transform.position);
        

        return distance >= farDistance;
    }
}
