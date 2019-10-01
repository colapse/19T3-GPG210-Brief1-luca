using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "States/Decisions/Enemy Stronger")]
public class EnemyStrongerDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        Debug.Log("Blah ");
        Slime me = controller.GetComponent<Slime>();

        if (me == null || controller.inputManager.currentTarget == null)
            return false;

        Debug.Log("Enemy Strong Dec: "+(controller.inputManager?.currentTarget.Volume >= me.Volume));

        return controller?.inputManager?.currentTarget.Volume >= me.Volume;
    }
}
