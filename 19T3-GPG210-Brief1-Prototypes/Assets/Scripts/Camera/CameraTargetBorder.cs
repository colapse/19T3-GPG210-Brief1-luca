using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Camera
{
    
    [ExecuteAlways]
    public class CameraTargetBorder : MonoBehaviour
    {
        public bool emptyAllFocusTargets;
        public List<Transform> removeFocusTargets = new List<Transform>();
        
        [OnValueChanged("AddTargetToList"), ShowInInspector]
        private CameraTarget addCameraTarget; // Just for Inspector & usability
        public List<CinemachineTargetGroup.Target> addFocusTargets = new List<CinemachineTargetGroup.Target>();

        public SlimeManager playerSlimeManager; // hacky

        public Collider col;

        
        // Gizmos
        public Sprite indicatorSprite;
        private BoxCollider boxCollider;
        private SphereCollider sphereCollider;
        private MeshCollider meshCollider;
        private delegate void DrawColliderDel();
        private DrawColliderDel drawColliderGizmos;


        private void Start()
        {
            playerSlimeManager = FindObjectOfType<SlimeManager>();
            drawColliderGizmos = DrawDefaultColliderGizmos;
            if (col == null)
                col = GetComponent<Collider>();
            CheckColliderType();
            
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

        public bool drawGizmos = true;
        public bool alwaysDrawGizmos = true;
        
        private void OnDrawGizmos()
        {
            if(!drawGizmos || !alwaysDrawGizmos)
                return;
            
            Matrix4x4 cubeTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
 
            Gizmos.matrix = cubeTransform;
 
            drawColliderGizmos?.Invoke();
            Gizmos.matrix = oldGizmosMatrix;
        }

        private void OnDrawGizmosSelected()
        {
            if(!drawGizmos || alwaysDrawGizmos)
                return;
            
            Matrix4x4 cubeTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
 
            Gizmos.matrix = cubeTransform;
            drawColliderGizmos?.Invoke();
            Gizmos.matrix = oldGizmosMatrix;
        }

        private void DrawBoxColliderGizmos()
        {
            Gizmos.color = new Color32(64, 223, 255,100);
            
            Gizmos.DrawCube(boxCollider.center, boxCollider.size);
            Gizmos.DrawIcon(col.bounds.center, "Collab.BuildSucceeded", true);
        }
        
        private SphereCollider sphereCol;

        private void DrawSphereColliderGizmos()
        {
            Gizmos.color = new Color32(64, 223, 255,100);
            Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
            Gizmos.DrawIcon(col.bounds.center, "Collab.BuildSucceeded", true);
        }

        private void DrawDefaultColliderGizmos()
        {
            Gizmos.color = new Color32(64, 223, 255,100);
            Gizmos.DrawIcon(col.bounds.center, "Collab.BuildSucceeded", true);
        }

        private void CheckColliderType()
        {
            if (col == null)
                return;
            switch (col)
            {
                case BoxCollider boxCol:
                    drawColliderGizmos = DrawBoxColliderGizmos;
                    boxCollider = boxCol;
                    break;
                case SphereCollider sCol:
                    drawColliderGizmos = DrawSphereColliderGizmos;
                    sphereCollider = sCol;
                    break;
                default:
                    drawColliderGizmos = DrawDefaultColliderGizmos;
                    break;
            }
        }
    }
}