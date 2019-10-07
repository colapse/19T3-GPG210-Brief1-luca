using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.XR;

namespace TriggerSystemV2
{
    [Serializable]
    public abstract class TriggerRequirement
    {
        
        // TODO Event notifying when the requirement status changed (requirement met true, else false)
        public delegate void StatusUpdatedDel(TriggerRequirement triggerReq, bool requirementStatus);
        public StatusUpdatedDel onStatusChanged;
        public void NotifyStatusChanged(bool status)
        {
            if(onStatusChanged != null)
                onStatusChanged (this, status);
        }
        
        // TODO Reference to trigger
        [SerializeField, ShowInInspector]
        protected Trigger trigger;
        public Trigger Trigger
        {
            get => trigger;
            set
            {
                if (trigger != null)
                        trigger.onValueChanged -= HandleTriggerValueChanged;
                
                trigger = value;
                
                if (trigger != null)
                    trigger.onValueChanged += HandleTriggerValueChanged;
            }
        }

        protected virtual void HandleTriggerValueChanged(Trigger triggerOrigin, System.Object value)
        {
            if(triggerOrigin == trigger)
                NotifyStatusChanged(CheckRequirement());
            
        }


        public abstract bool CheckRequirement();

        
    }
}
