using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;

namespace TriggerSystemV2
{
    public class PressurePlateTrigger : Trigger
    {
        public override Type TriggerStatusType { get; } = typeof(float);

        public float maxMass;

        // List of objects standing on the pressure plate
        //private readonly List<Rigidbody> containedRigidBodies = new List<Rigidbody>();
        private readonly ObservableCollection<Rigidbody> containedRigidBodies = new ObservableCollection<Rigidbody>();

        private void Start()
        {
            containedRigidBodies.CollectionChanged += HandleCollectionChanged;
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
            Debug.Log("Trigger Status Changed "+Convert.ToSingle(TriggerStatus));
        }

        private float CalculateContainedMass()
        {
            float totalMass = 0;

            if (containedRigidBodies != null && containedRigidBodies.Count > 0)
            {
                foreach (var rb in containedRigidBodies)
                {
                    if (rb != null)
                    {
                        totalMass += rb.mass;
                    }
                }
            }
            
            return totalMass;
        }
    }
}