using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "States/Decisions/Enemy Stronger")]
public class EnemyStrongerDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        Slime me = controller.GetComponent<Slime>();
        if (me == null || controller.inputManager.currentTarget == null)
            return false;

        return controller?.inputManager?.currentTarget.Volume >= me.Volume;
    }
}
