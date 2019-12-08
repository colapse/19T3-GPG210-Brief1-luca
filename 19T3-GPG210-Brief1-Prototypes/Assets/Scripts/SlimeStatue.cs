using System;
using System.Collections;
using TriggerSystemV2;
using UnityEngine;

public class SlimeStatue : MonoBehaviour, ITriggerableStateListener
{
    public GameObject activeEyeL;
    public GameObject activeEyeR;
    public GameObject defaultEyeL;
    public GameObject defaultEyeR;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float maxGlowTime = 0;


    public void HandleTriggerableStateChanged(TriggerSystemV2.Triggerable triggerable, bool state)
    {
        GlowEye(state);
    }

    public void GlowEye(bool state)
    {
        if (state)
        {
            activeEyeL?.SetActive(true);
            defaultEyeL?.SetActive(false);
            activeEyeR?.SetActive(true);
            defaultEyeR?.SetActive(false);

            if (maxGlowTime > 0)
            {
                StartCoroutine(TurnOffEyesCountdown());
            }
        }
        else
        {
            activeEyeL?.SetActive(false);
            defaultEyeL?.SetActive(true);
            activeEyeR?.SetActive(false);
            defaultEyeR?.SetActive(true);
        }
        
        
    }

    private IEnumerator TurnOffEyesCountdown()
    {
        
        yield return new WaitForSeconds(maxGlowTime);
        
        activeEyeL?.SetActive(false);
        defaultEyeL?.SetActive(true);
        activeEyeR?.SetActive(false);
        defaultEyeR?.SetActive(true);
        
        yield return 0;
    }
}
