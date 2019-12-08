using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Portal
{
    [System.Serializable]
    public class PortalEvent : UnityEvent<SlimePortal, GameObject>
    {
    }
    
    public class SlimePortal : SerializedMonoBehaviour
    {
        public List<GameObject> portalStorage = new List<GameObject>();
    /*

        [ShowInInspector]
        public event Action<SlimePortal, GameObject> OnObjectEnteredPortal;*/

        [ShowInInspector] public PortalEvent OnObjectEnteredPortal;

    
    
        public Transform spitOutTarget;
        public Transform spawnLocation;
        public bool doSpitOut;
        public float spitOutVelocity;

        // TODO -> Use external pooling
        public GameObject spitOutObjectWrapperPrefab;
        private readonly List<GameObject> availableWrappers = new List<GameObject>();
    
        public Renderer portalRenderer;

        [ShowInInspector, SerializeField]
        private bool portalIsActive = true;
        public bool PortalIsActive
        {
            get => portalIsActive;
            set
            {
                if (portalIsActive != value)
                {
                    portalIsActive = value;
                    OnPortalStatusChanged();
                }
            }
        }
        public bool canBeEntered = false;
        public bool knockBackWhenCantBeEntered = false;
        public bool knockBackWhenInactive = false;
        public float knockbackVelocity = 5;

        private GameObject GetSpitOutObjWrapper()
        {
            GameObject wrapper;
            if (availableWrappers.Count > 0)
            {
                wrapper = availableWrappers[availableWrappers.Count - 1];
                availableWrappers.RemoveAt(availableWrappers.Count - 1);
            }else
            {
                wrapper = Instantiate(spitOutObjectWrapperPrefab,spawnLocation.transform.position, transform.rotation);
                wrapper.GetComponent<PortalObjectWrapper>().OnObjectLanded += HandleWrapperLanded;
            }
            return wrapper;
        }

        private void HandleWrapperLanded(PortalObjectWrapper obj)
        {
            obj.transform.DetachChildren();
            ReturnSpitOutWrapper(obj.gameObject);
        }

        private void ReturnSpitOutWrapper(GameObject wrapper)
        {
            wrapper.transform.position = spawnLocation.transform.position;
            wrapper.SetActive(false);
            availableWrappers.Add(wrapper);
        }

        private void OnPortalStatusChanged()
        {
            switch (PortalIsActive)
            {
                case true:
                    if (activeMaterial != null && portalRenderer != null)
                        portalRenderer.material = activeMaterial;
                    break;
                case false:
                    if (inactiveMaterial != null && portalRenderer != null)
                        portalRenderer.material = inactiveMaterial;
                    break;
            }
        }

        public Material activeMaterial;
        public Material inactiveMaterial;

        public ParticleSystem collisionSplashParticles;
    
        // TODO Add Limitations (Only certain objects, only enemy/player, only certain color... )
    
        // Start is called before the first frame update
        void Start()
        {
            if(portalRenderer == null)
                portalRenderer = GetComponent<Renderer>();
        
            // Initial Call to ensure everything is set up correctly
            OnPortalStatusChanged();
        }

        // Update is called once per frame
        void Update()
        {
            // Testing  code
            if (Input.GetKeyUp(KeyCode.Alpha9))
            {
                EvokeFirstContained();
            }
        }

        private void OnDestroy()
        {
            availableWrappers?.ForEach(wrapper =>
            {
                if(wrapper != null)
                    wrapper.GetComponent<PortalObjectWrapper>().OnObjectLanded -= HandleWrapperLanded;
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!(collisionSplashParticles?.isPlaying ?? true))
                collisionSplashParticles.Play();
            HandleObjectEntered(other.gameObject);
            /*var slime = other.GetComponent<Slime>();
        if (slime != null)
        {
            HandleSlimeEntry(slime);
        }*/
        }
/*

    private void HandleSlimeEntry(Slime slime)
    {
        // TODO do stuff
        
        AddObjectToPortal(slime.gameObject);
        
        objectEnteredPortal?.Invoke(slime.gameObject);
        
        
    }*/

        private void HandleObjectEntered(GameObject go)
        {
            if (!PortalIsActive || !canBeEntered)
            {
                if (knockBackWhenInactive || knockBackWhenCantBeEntered)
                    SpitOutObject(go,knockbackVelocity);
                
                return;
            }
            go.SetActive(false);
            AddObjectToPortal(go);
            OnObjectEnteredPortal?.Invoke(this, go);
        }

        public void AddObjectToPortal(GameObject go)
        {
            portalStorage.Add(go);
        }

        public GameObject RemoveObjectFromPortal(GameObject go)
        {
            if (portalStorage.Contains(go))
            {
                portalStorage.Remove(go);
                return go;
            }

            return null;
        }

        // Spits out first object from portalStorage 
        public void EvokeFirstContained()
        {
            if((portalStorage?.Count ?? 0) <= 0)
                return;
        
            DoEvokeObject(portalStorage[0]);
            portalStorage.RemoveAt(0);
        }
    
        // Sets the position of the gameobject to portal & "spits it out"
        public void EvokeObject(GameObject gameObject)
        {
            DoEvokeObject(gameObject);
        }
    
        // Instantiates new object & spits it out
        public void EvokePrefabObject(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, spawnLocation.position, transform.rotation);
            DoEvokeObject(go);
        }

        // TODO @SpitOut would be nicer to define a path and move it along there instead of using wrappers/rigidbodies
        private void DoEvokeObject(GameObject go)
        {
            go.transform.position = spawnLocation.position;
            go.SetActive(true);
            if (doSpitOut)
            {
                /*Vector3 dir = spitOutTarget != null ? (spitOutTarget.position - spawnLocation.position) : transform.forward;

                Rigidbody objRb = go.GetComponent<Rigidbody>();
                if (objRb == null)
                {
                    var wrapper = GetSpitOutObjWrapper();
                    go.transform.SetParent(wrapper.transform);
                    objRb = wrapper.GetComponent<Rigidbody>();
                    wrapper.SetActive(true);
                    wrapper.GetComponent<PortalObjectWrapper>().Initialize();
                }
            
                objRb.AddForce(spitOutVelocity * dir, ForceMode.VelocityChange);*/
                SpitOutObject(go);
            }
        }

        private void SpitOutObject(GameObject go, float velocity = 0)
        {
            velocity = Mathf.Approximately(velocity, 0) ? spitOutVelocity : velocity;
            Vector3 dir = spitOutTarget != null ? (spitOutTarget.position - spawnLocation.position) : transform.forward;

            Rigidbody objRb = go.GetComponent<Rigidbody>();
            if (objRb == null)
            {
                var wrapper = GetSpitOutObjWrapper();
                go.transform.SetParent(wrapper.transform);
                objRb = wrapper.GetComponent<Rigidbody>();
                wrapper.SetActive(true);
                wrapper.GetComponent<PortalObjectWrapper>().Initialize();
            }
            
            objRb.AddForce(velocity * dir, ForceMode.VelocityChange);
        }

        public bool teleportObjectsFromLinkedPortal = false;
        public void HandleOtherPortalObjectEntered(SlimePortal otherPortal, GameObject enteredGo)
        {
            if (!teleportObjectsFromLinkedPortal)
                return;
            
            GameObject go = otherPortal.RemoveObjectFromPortal(enteredGo);
            
            if (go != null)
            {
                EvokeObject(go);
            }
        }
    }
}
