using System.Collections.Generic;
using UnityEngine;

namespace TriggerSystemV2
{
    public class ObjectToggler : MonoBehaviour, ITriggerableStateListener
    {
        public List<GameObject> stateInactiveVisibleObjects = new List<GameObject>();
        public List<GameObject> stateActiveVisibleObjects = new List<GameObject>();
    
        // Start is called before the first frame update
        void Start()
        {
            ToggleObjects(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void HandleTriggerableStateChanged(Triggerable triggerable, bool state)
        {
            ToggleObjects(state);
        }

        public void ToggleObjects(bool state)
        {
            stateInactiveVisibleObjects?.ForEach(obj =>
            {
                obj.SetActive(!state);
            });
            stateActiveVisibleObjects?.ForEach(obj =>
            {
                obj.SetActive(state);
            });
        }
    }
}
