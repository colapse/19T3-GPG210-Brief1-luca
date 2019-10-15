using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace TriggerSystemV2
{
    [System.Serializable]
    public class TriggerableEvent : UnityEvent<Triggerable, bool>
    {
    }
    
    public class Triggerable : SerializedMonoBehaviour 
    {
        /*
        public delegate void TriggerableEventDel(Triggerable triggerable, bool isTriggered);
        public event TriggerableEventDel OnStatusChanged;
        */
        public TriggerableEvent statusChangedEvent;
        private void NotifyValueChanged(bool isTriggered)
        {/*
            if(OnStatusChanged != null)
                OnStatusChanged (this, isTriggered);*/
            
            statusChangedEvent.Invoke(this,isTriggered);
        }

        
        
        [NonSerialized, OdinSerialize]
        public List<TriggerRequirement> triggerRequirements;
        [SerializeField] public bool allReqMustBeMet = true;

        // Stores which triggerRequirements are met & which not // Kind of hacky
        private Dictionary<TriggerRequirement, bool> triggerReqStatus;

        
        // Start is called before the first frame update
        void Start()
        {
            triggerReqStatus = new Dictionary<TriggerRequirement, bool>();
            
            if (triggerRequirements == null)
            {
                triggerRequirements = new List<TriggerRequirement>();
            }else if (triggerRequirements.Count > 0)
            {
                // Subscribe to requirementUpdates
                foreach (var requirement in triggerRequirements)
                {
                    requirement.Trigger = requirement.Trigger; // HACK: Make sure the Property's set{} method gets called
                    requirement.onStatusChanged += HandleRequirementStatusChange;
                    triggerReqStatus.Add(requirement,requirement.CheckRequirement());
                }
            }
        }

        private void HandleRequirementStatusChange(TriggerRequirement triggerReq, bool requirementStatus)
        {
            bool currentStatus = IsTriggered();
            
            if (triggerReqStatus.ContainsKey(triggerReq))
            {
                triggerReqStatus[triggerReq] = requirementStatus;
            }
            
            bool newStatus = IsTriggered();

            
            
            if(/*!currentStatus.Equals(newStatus)*/ currentStatus != newStatus)
            {
                NotifyValueChanged(newStatus);
            }
        }

        public bool IsTriggered()
        {
            return allReqMustBeMet ? !triggerReqStatus.Values.Contains(false) : triggerReqStatus.Values.Contains(true);
        }
    }
}

