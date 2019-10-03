using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Door : SerializedMonoBehaviour
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

    public void OpenDoor(bool open)
    {
        renderer.enabled = !open;
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
