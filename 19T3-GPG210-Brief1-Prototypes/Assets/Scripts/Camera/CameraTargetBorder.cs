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

        private void AddTargetToList()
        {

            if (!(addCameraTarget?.targetSetup ?? default).Equals(default) && addFocusTargets.Contains(addCameraTarget.targetSetup, addCameraTarget))
            {
                
                addFocusTargets.Add(addCameraTarget.targetSetup);
            }
                
            addFocusTargets = default;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var sct = other.GetComponent<SlimeCameraTargetGroup>();
            if (sct == null) return;
            if (emptyAllFocusTargets)
                sct.RemoveAllFocusTargets();

            if (!emptyAllFocusTargets && removeFocusTargets?.Count > 0) 
                sct.RemoveFocusTargets(removeFocusTargets);
            if(addFocusTargets?.Count > 0)
                sct.AddFocusTargets(addFocusTargets);
        }
    }
}