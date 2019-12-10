using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraTarget : MonoBehaviour, IEqualityComparer<CinemachineTargetGroup.Target>
    {
        public CinemachineTargetGroup.Target targetSetup;

        private void OnEnable()
        {
            if (targetSetup.target == null)
                targetSetup.target = transform;
        }

        public bool Equals(CinemachineTargetGroup.Target x, CinemachineTargetGroup.Target y)
        {
            return x.target.Equals(y.target);
        }

        public int GetHashCode(CinemachineTargetGroup.Target obj)
        {
            return base.GetHashCode();
        }
    }
}