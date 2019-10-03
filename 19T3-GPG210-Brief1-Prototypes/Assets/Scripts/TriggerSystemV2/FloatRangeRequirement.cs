using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TriggerSystemV2
{
    [Serializable]
    public class FloatRangeRequirement : TriggerRequirement
    {
        
        [SerializeField]
        public float minValue;
        [SerializeField]
        public float maxValue;
        [SerializeField]
        public bool inclusive;
        public override bool CheckRequirement()
        {
            float triggerValue = Convert.ToSingle(Trigger.GetTriggerStatus());

            if (inclusive)
            {
                return triggerValue >= minValue && triggerValue <= maxValue;
            }
            else
            {
                return triggerValue > minValue && triggerValue < maxValue;
            }
        }
    }
}