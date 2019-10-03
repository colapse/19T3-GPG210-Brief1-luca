﻿using UnityEngine;

namespace StateMachineV2.Decisions
{
    [CreateAssetMenu(menuName = "States/Decisions/TargetTwiceSmaller")]
    public class TargetTwiceSmallerDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            Slime me = controller.GetComponent<Slime>();
            Slime target = controller.inputManager.currentTarget;
            Debug.Log(me+ "  "+target);
            if (controller.inputManager.currentTarget == null || me == null || target == null)
                return false;
            
            Debug.Log("2 "+me.Volume+" "+target.Volume+" "+(me.Volume > target.Volume * 2));
            
            return me.Volume > target.Volume * 2;
        }
    }
}