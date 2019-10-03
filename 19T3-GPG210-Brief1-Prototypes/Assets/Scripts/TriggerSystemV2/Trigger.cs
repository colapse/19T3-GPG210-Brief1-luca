using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace TriggerSystemV2
{
    public class Trigger : MonoBehaviour
    {
        
        public delegate void TriggerValueChangedDel(Trigger trigger, Object obj);
        public TriggerValueChangedDel onValueChanged;
        public void NotifyValueChanged(Object obj)
        {
            if(onValueChanged != null)
                onValueChanged (this, obj);
        }

        // Can be used to define the type of the trigger status (Useful in Child-Classes)
        public virtual Type TriggerStatusType { get; } = typeof(object);

        private Object triggerStatus;
        public virtual Object TriggerStatus
        {
            get => triggerStatus;
            set { triggerStatus = value; NotifyValueChanged(triggerStatus); }
        }

        public virtual Object GetTriggerStatus()
        {
            return triggerStatus;
        }
    }
}

