using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Sirenix.Utilities;
using UnityEngine;

namespace Camera
{
    public class SlimeCameraTargetGroup : CinemachineTargetGroup
    {
        /*[NonSerialized]
        private HashSet<Transform> currentFocusTargets;*/

        public bool alwaysFocusSelf = true;
        public float weight = 2;
        public float radius = 3;
    
        public Slime slime;

        public event Action<SlimeCameraTargetGroup> OnTargetsChange;
    
        // Start is called before the first frame update
        void Start()
        {
            //currentFocusTargets = new HashSet<Transform>();

            if (slime == null)
                slime = GetComponent<Slime>();
            
            
        }
        
        public void AddFocusTarget(Target focusTarget)
        {
            AddMember(focusTarget.target, focusTarget.weight, focusTarget.radius);
            OnTargetsChange?.Invoke(this);
        }

        public void RemoveFocusTarget(Target focusTarget)
        {
            RemoveMember(focusTarget.target);
            OnTargetsChange?.Invoke(this);
        }

        public void AddFocusTargets(List<Target> focusTargets)
        {
            focusTargets?.ForEach(t => { AddMember(t.target, t.weight, t.radius); });
            OnTargetsChange?.Invoke(this);
        }
        
        public void RemoveFocusTarget(Transform focusTargetTransform)
        {
            RemoveMember(focusTargetTransform);
            OnTargetsChange?.Invoke(this);
        }

        public void RemoveFocusTargets(List<Transform> focusTargets)
        {
            focusTargets?.ForEach(RemoveMember);
            OnTargetsChange?.Invoke(this);
        }

        public void RemoveAllFocusTargets()
        {
            m_Targets = new Target[0];
            OnTargetsChange?.Invoke(this);
        }

        public Target[] GetFocusTargets()
        {
            if(alwaysFocusSelf && FindMember(transform) < 0)
                AddMember(transform, weight, radius);
        
            return m_Targets;
        }
    

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
