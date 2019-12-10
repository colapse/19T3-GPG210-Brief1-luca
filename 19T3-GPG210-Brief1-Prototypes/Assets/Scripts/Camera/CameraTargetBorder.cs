using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Camera
{
    
    public class CameraTargetBorder : MonoBehaviour
    {
        public bool emptyAllFocusTargets;
        public List<Transform> removeFocusTargets = new List<Transform>();
        
        [OnValueChanged("AddTargetToList"), ShowInInspector]
        private CameraTarget addCameraTarget; // Just for Inspector & usability
        public List<CinemachineTargetGroup.Target> addFocusTargets = new List<CinemachineTargetGroup.Target>();

        public SlimeManager playerSlimeManager; // hacky

        private void Start()
        {
            playerSlimeManager = FindObjectOfType<SlimeManager>();
        }

        private void AddTargetToList()
        {

            if (addCameraTarget != null && !(addCameraTarget.targetSetup).Equals(default) && !addFocusTargets.Contains(addCameraTarget.targetSetup, addCameraTarget))
            {
                
                addFocusTargets.Add(addCameraTarget.targetSetup);
            }
                
            addCameraTarget = null;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Slime s = other.GetComponent<Slime>();
            
            if(s == null)
                return;

            SlimeCameraTargetGroup sct = null;
            if (!(playerSlimeManager?.slimeCameraTargetGroups?.TryGetValue(s, out sct) ?? false) || sct == null) return;
            if (emptyAllFocusTargets)
                sct.RemoveAllFocusTargets();

            if (!emptyAllFocusTargets && removeFocusTargets?.Count > 0) 
                sct.RemoveFocusTargets(removeFocusTargets);
            if(addFocusTargets?.Count > 0)
                sct.AddFocusTargets(addFocusTargets);
        }
    }
}