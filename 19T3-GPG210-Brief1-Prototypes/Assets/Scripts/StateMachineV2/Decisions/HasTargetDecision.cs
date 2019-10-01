using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Decisions/HasTargetDecision")]
public class HasTargetDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return controller.inputManager.currentTarget != null;
    }
}
