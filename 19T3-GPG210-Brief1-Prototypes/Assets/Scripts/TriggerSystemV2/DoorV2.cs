using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace TriggerSystemV2
{
    public class DoorV2: MonoBehaviour
    {
        public Triggerable triggerable;

        public Vector3 closePosition; // Hacky
        public Vector3 openPosition; // Hacky
        public float doorMovementSpeed = 1;
        public bool doorOpen = false;
        private bool doOpenDoor = false;
        private bool doCloseDoor = false;
        
        private void Start()
        {
            if (triggerable == null)
            {
                triggerable = GetComponent<Triggerable>();
            }

            if (triggerable != null)
            {
                triggerable.onStatusChanged += HandleTriggerableStatusChanged;
            }
        }

        private void Update()
        {
            if (doOpenDoor)
            {
                OpenDoor();
            }else if (doCloseDoor)
            {
                CloseDoor();
            }
        }

        private void HandleTriggerableStatusChanged(Triggerable trig, bool isTriggered)
        {
            Debug.Log("New Triggerable Status: "+isTriggered);
            if (isTriggered)
            {
                doOpenDoor = true;
                doCloseDoor = false;
            }
            else
            {
                doCloseDoor = true;
                doOpenDoor = false;
            }
        }
        
        private void OpenDoor()
        {
            if (transform.position != openPosition)
            {
                float step = doorMovementSpeed * Time.deltaTime;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, openPosition, step);
            }
            else
            {
                doorOpen = true;
                doOpenDoor = false;
            }
        }

        private void CloseDoor()
        {
            if (transform.position != closePosition)
            {
                float step = doorMovementSpeed * Time.deltaTime;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, closePosition, step);
                
            }
            else
            {
                doorOpen = false;
                doCloseDoor = false;
            }
        }
    }
}