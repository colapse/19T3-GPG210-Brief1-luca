using System;
using UnityEngine;

namespace TriggerSystemV2
{
    public class AC_MaterialHighlighter : MonoBehaviour
    {
        public Material originalMaterial;
        public Material highlightMaterial;

        private Renderer renderer;
        
        private void Start()
        {
            renderer = GetComponent<Renderer>();
            if (originalMaterial == null)
            {
                originalMaterial = renderer?.material;
            }
            
        }

        public void ActivateHighlightColor(Triggerable trig, bool activate)
        {
            if(renderer == null)
                return;
            Debug.Log("Skruh "+ activate+" - "+(activate ? highlightMaterial : originalMaterial).name+" - "+(highlightMaterial == null )+" - "+(originalMaterial == null));
            renderer.material = activate ? highlightMaterial : originalMaterial;
        }
    }
}