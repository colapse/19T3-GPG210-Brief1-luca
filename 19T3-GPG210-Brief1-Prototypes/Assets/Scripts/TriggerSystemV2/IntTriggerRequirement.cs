using System;
using UnityEngine;
namespace TriggerSystemV2
{
    [System.Serializable]
    public class IntTriggerRequirement : TriggerRequirement
    {
        public int requiredValue;

        public override bool CheckRequirement()
        {
            
            
            return requiredValue == Convert.ToInt32(Trigger.GetTriggerStatus());
        }
    }
}