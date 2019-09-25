using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PressurePlateTrigger : Trigger
{
    public float weightResistance = 5;
    
    public Material defaultMaterial;
    public Material activeMaterial;

    private Renderer _renderer;
    
    private void OnTriggerEnter(Collider other)
    {
        Slime slime = other.GetComponent<Slime>();
        if (slime != null)
        {
            CurrentState += Mathf.FloorToInt(slime.Volume / (weightResistance / states));
            _renderer.material = activeMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Slime slime = other.GetComponent<Slime>();
        if (slime != null)
        {
            CurrentState -= Mathf.FloorToInt(slime.Volume / (weightResistance / states));
            _renderer.material = defaultMaterial;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
