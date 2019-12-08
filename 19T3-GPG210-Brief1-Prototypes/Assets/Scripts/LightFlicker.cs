using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light light;

    public Vector2 intensityRange = Vector2.up;
    
    public float flickerSpeed = .5f;
    private float flickerCooldown = 0;

    public bool ease = true;
    public float easeDuration = 0.1f;

    private float currentIntensityTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        if (light == null)
            light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flickerCooldown > 0)
        {
            flickerCooldown -= Time.deltaTime;
        }
        else
        {
            currentIntensityTarget = Random.Range(intensityRange.x, intensityRange.y);

            if (ease)
            {
                light.DOIntensity(currentIntensityTarget, easeDuration);
            }
            else
            {
                light.intensity = currentIntensityTarget;
            }

            flickerCooldown = flickerSpeed;
        }
    }
}
