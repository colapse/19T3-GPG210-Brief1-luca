using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Material defaultMaterial;
    public Material activeMaterial;

    // TODO Ugly, just for testing
    public GameObject doorObject;

    private Renderer renderer;

    public bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
            doorObject.SetActive(false);
            renderer.material = activeMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
            doorObject.SetActive(true);
            renderer.material = defaultMaterial;
        }
    }
}
