using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider collider;
    private Renderer renderer;
    
    public int requiredSwitches = 3;
    [SerializeField]
    private int switchCounter = 0;

    public int SwitchCounter
    {
        get => switchCounter;
        set
        {
            switchCounter = value;
            if (switchCounter >= requiredSwitches)
            {
                collider.enabled = false;
                renderer.enabled = false;
            }
            else
            {
                collider.enabled = true;
                renderer.enabled = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
