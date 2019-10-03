using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiStagePressurePlate : MonoBehaviour
{
    public GameObject tstAmple;
    public Material maxMat;
    public Material medMat;
    public Material inactiveMat;
    Renderer tstAmpleRenderer;

    public Door door;
    
    public int stages = 2;
    public float maxWeight = 5;
    
    public int currentStage = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        tstAmpleRenderer = tstAmple.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Slime slime = other.GetComponent<Slime>();
        if (slime != null)
        {
            currentStage += Mathf.CeilToInt(slime.Volume / (maxWeight / stages));
            door.SwitchCounter += currentStage / stages;
            tstAmpleRenderer.material = ((float)currentStage / (float)stages <= 0)? inactiveMat : ((currentStage / stages < 1)?medMat : maxMat);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Slime slime = other.GetComponent<Slime>();
        if (slime != null)
        {
            door.SwitchCounter -= currentStage / stages;
            currentStage -= Mathf.CeilToInt(slime.Volume / (maxWeight / stages));
            door.SwitchCounter += currentStage / stages;
            tstAmpleRenderer.material = ((float)currentStage / (float)stages <= 0)? inactiveMat : ((currentStage / stages < 1)?medMat : maxMat);
        }
    }
}
