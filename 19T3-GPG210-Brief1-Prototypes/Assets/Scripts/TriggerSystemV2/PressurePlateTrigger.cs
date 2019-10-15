using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Object = System.Object;

namespace TriggerSystemV2
{
    public class PressurePlateTrigger : Trigger
    {
        public override Type TriggerStatusType { get; } = typeof(float);
        
        public override Object TriggerStatus
        {
            get => triggerStatus;
            set {
                if (!Mathf.Approximately(Convert.ToSingle(triggerStatus),Convert.ToSingle(value)))
                {
                    triggerStatus = value;
                    NotifyValueChanged(triggerStatus);
                }
            }
        }

        public float maxMass;
        
        public float weightCheckerInterval = .5f;
        private float weightCheckCooldown = 0;

        public float emptyYPos = 0;
        public float maxWeightYPosition = 0;
        private bool weightChanged = false;

        // List of objects standing on the pressure plate
        //private readonly List<Rigidbody> containedRigidBodies = new List<Rigidbody>();
        [ShowInInspector]
        private readonly ObservableCollection<Rigidbody> containedRigidBodies = new ObservableCollection<Rigidbody>();

        private void Start()
        {
            emptyYPos = transform.position.y;
            maxWeightYPosition = emptyYPos-(transform.GetComponent<Renderer>()?.bounds.size.y ?? 0)+0.05f;
            containedRigidBodies.CollectionChanged += HandleCollectionChanged;
            onValueChanged += HandleTriggerValueChanged;
        }

        private void OnDestroy()
        {
            containedRigidBodies.CollectionChanged -= HandleCollectionChanged;
            onValueChanged -= HandleTriggerValueChanged;
        }

        private void Update()
        {
            if (weightCheckCooldown <= 0 && containedRigidBodies.Count > 0)
            {
                TriggerStatus = CalculateContainedMass();
                weightCheckCooldown = weightCheckerInterval;
            }
            else
            {
                weightCheckCooldown -= Time.deltaTime;
            }
            
            if (weightChanged)
            {
                float cTargetYPos = emptyYPos - (Mathf.Abs(emptyYPos - maxWeightYPosition)/maxMass) * Mathf.Clamp(Convert.ToSingle(TriggerStatus),0,maxMass);
                if (!Mathf.Approximately(cTargetYPos, transform.position.y))
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(transform.position.x, cTargetYPos, transform.position.z),0.01f);
                }
                else
                {
                    weightChanged = false;
                }
            }
            
            
        }

        private void OnTriggerEnter(Collider collider)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                containedRigidBodies.Add(rb);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();

            if (rb != null && containedRigidBodies.Contains(rb))
            {
                containedRigidBodies.Remove(rb);
            }
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TriggerStatus = CalculateContainedMass();
        }

        private float CalculateContainedMass()
        {
            float totalMass = 0;

            if (containedRigidBodies != null && containedRigidBodies.Count > 0)
            {
                foreach (var rb in containedRigidBodies)
                {
                    if (rb != null && SlimeInputManager.IsGrounded(rb.gameObject))
                    {
                        totalMass += rb.mass;
                    }
                }
            }
            
            return totalMass;
        }

        public void HandleTriggerValueChanged(Trigger trigger, System.Object value)
        {
            Debug.Log("Changed "+Convert.ToSingle(value));
            weightChanged = true;
            
        }
    }
}