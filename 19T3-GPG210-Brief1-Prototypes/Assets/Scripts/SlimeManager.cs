using System;
using System.Collections;
using System.Collections.Generic;
using Camera;
using Cinemachine;
using Sirenix.Utilities;
using StateMachineV1;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Serialization;

public class SlimeManager : MonoBehaviour
{
    public delegate void FocusChangeDel (GameObject gameObject);
    public FocusChangeDel onFocusChange;

    public SlimeInputManager slimeInputManager;
    public CinemachineVirtualCamera playerVirtualCamera;
      
    public void NotifyFocusChange(GameObject gameObject)
    {
        UpdateVirtualCameraTarget();
        if(onFocusChange != null)
            onFocusChange (gameObject);
    }
    
    public Material defaultSlimeMaterial;
    public Material activeSlimeMaterial;
    public GameObject slimePrefab;
    
    public float slimeMinVolume = 1;
    public float slimeMaxVolume = 1;
    
    public List<Slime> slimes;
    public GameObject slimeCameraTargetGroupPrefab;
    public Dictionary<Slime, SlimeCameraTargetGroup> slimeCameraTargetGroups;

    public Slime activeSlime;
    // Start is called before the first frame update
    void Start()
    {
        if (activeSlime == null && slimes?.Count > 0)
        {
            activeSlime = slimes[0];
        }

        if (slimeCameraTargetGroups == null)
        {
            slimeCameraTargetGroups = new Dictionary<Slime, SlimeCameraTargetGroup>();
        }

        if (slimes == null)
        {
            slimes = new List<Slime>();
        }
        
        if (activeSlime != null)
            activeSlime.GetComponent<Renderer>().material = activeSlimeMaterial;
    
        UpdateVirtualCameraTarget();
        
        slimes?.ForEach(slime =>
        {
            slime.OnSlimeSplit += HandleSlimeHasSplit;
            slime.OnGettingDestroyed += HandleSlimeGetsDestroyed;
        });

        if (slimeInputManager != null)
        {
            slimeInputManager.OnEnemyEnteredSight += HandleEnemyEnteredSight;
            slimeInputManager.OnEnemyExitedSight += HandleEnemyExitedSight;
        }
    }

    private void HandleEnemyExitedSight(Slime detectorSlime, Slime detectedSlime)
    {

        slimeCameraTargetGroups?.Values.ForEach(sctg =>
        {
            var ct = detectedSlime.GetComponent<CameraTarget>();
            if(ct != null)
                sctg.RemoveFocusTarget(ct.targetSetup);
        });
        /*if (slimeCameraTargetGroups.TryGetValue(activeSlime, out var sctg))
        {
            sctg.
        }*/
    }

    private void HandleEnemyEnteredSight(Slime detectorSlime, Slime detectedSlime)
    {
        slimeCameraTargetGroups?.Values.ForEach(sctg =>
        {
            var ct = detectedSlime.GetComponent<CameraTarget>();
            if(ct != null)
                sctg.AddFocusTarget(ct.targetSetup);
        });
    }

    private void UpdateVirtualCameraTarget()
    {
        if (activeSlime == null || playerVirtualCamera == null) return;
        SlimeCameraTargetGroup sctg = null;
        if (!slimeCameraTargetGroups.TryGetValue(activeSlime, out sctg))
        {
            sctg = Instantiate(slimeCameraTargetGroupPrefab)?.GetComponent<SlimeCameraTargetGroup>();
            if (sctg != null)
            {
                sctg.slime = activeSlime;
                slimeCameraTargetGroups.Add(activeSlime, sctg);
            }
        }
        
        playerVirtualCamera.m_LookAt = activeSlime.transform;
        playerVirtualCamera.m_Follow = sctg.transform; // Required SlimeCameraTargetgGroup Component to work correctly!
    }

    private void OnDestroy()
    {
        slimes?.ForEach(slime => {
            slime.OnSlimeSplit -= HandleSlimeHasSplit;
            slime.OnGettingDestroyed -= HandleSlimeGetsDestroyed;
        });

        if (slimeInputManager != null)
        {
            slimeInputManager.OnEnemyEnteredSight += HandleEnemyEnteredSight;
            slimeInputManager.OnEnemyExitedSight += HandleEnemyExitedSight;
        }
    }

    private void HandleSlimeHasSplit(Slime arg1, Slime arg2)
    {
        SlimeCameraTargetGroup sctg = Instantiate(slimeCameraTargetGroupPrefab)?.GetComponent<SlimeCameraTargetGroup>();
        if (sctg != null)
        {
            sctg.slime = activeSlime;
            slimeCameraTargetGroups.Add(activeSlime, sctg);
        }
        UpdateVirtualCameraTarget();
        slimes.Add(arg2);
    }

    private void HandleSlimeGetsDestroyed(Slime obj)
    {
        slimes.Remove(obj);
        obj.OnSlimeSplit -= HandleSlimeHasSplit;
        obj.OnGettingDestroyed -= HandleSlimeGetsDestroyed;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO Currently only used here for the PLAYER. Do not use for AI. Delete this at some point and use Split() in Slime class (& Notify player SlimeManager via event or so)
        // HACKY; NOT PERFORMANT
        if (slimeInputManager is PlayerInputManager manager && Input.GetKeyUp(manager.actionSplit)) 
        {
            SplitSlime(activeSlime);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            int nextSlimeIndex = activeSlime != null?slimes.IndexOf(activeSlime)+1:0;
            if (nextSlimeIndex > slimes.Count - 1 || nextSlimeIndex < 0)
            {
                nextSlimeIndex = 0;
            }
            
            if(activeSlime != null)
                activeSlime.GetComponent<Renderer>().material = defaultSlimeMaterial;

            activeSlime = slimes[nextSlimeIndex];
            if (activeSlime != null)
            {
                activeSlime.GetComponent<Renderer>().material = activeSlimeMaterial;
            }
            else
            {
                slimes.RemoveAll(item => item == null);

                if (slimes.Count > 0)
                {
                    activeSlime = slimes[0];
                    activeSlime.GetComponent<Renderer>().material = activeSlimeMaterial;
                }
            }
            NotifyFocusChange(activeSlime.gameObject);
        }
    }

    // Hacky, put in Slime Script
    void SplitSlime(Slime slime)
    {
        if (slime != null && slime.Volume >= slimeMinVolume * 2 && slimePrefab)
        {
            Renderer slimeRenderer = slime.GetComponent<Renderer>();
            Vector3 newSlimeSpawnPos = slime.transform.position + slime.transform.forward.normalized * (slimeRenderer.bounds.extents.z);
            GameObject newSlimeObj = Instantiate(slimePrefab,newSlimeSpawnPos,slime.transform.rotation);
            Slime newSlime = newSlimeObj.GetComponent<Slime>();
            StateManager newSlimeStateManager = newSlimeObj.GetComponent<StateManager>();

            newSlimeStateManager.slimeInputManager = slimeInputManager;
            
            newSlime.Volume = slime.Volume / 2;
            slime.Volume /= 2;
            
            if(newSlime)
                slimes.Add(newSlime);
            
            // Push slimes apart from each other
            Vector3 forwardForce = slime.rb.mass * newSlime.Volume * slime.splitForceMultiplier * slime.transform.forward;
            forwardForce.y = 10 * slime.rb.mass;
            newSlime.rb.AddForce(forwardForce);
            //slime.rb.AddForce(slime.transform.right*-30*newSlime.Volume);
        }
    }
}
