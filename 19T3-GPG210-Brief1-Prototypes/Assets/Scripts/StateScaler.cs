using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateScaler : StateBase
{
    private Slime slime;
    
    public Vector3 maxScale;
    public Vector3 minScale;
    public float scaleStep = 0.1f;
    private int xScalerDir = 1;
    private int yScalerDir = 1;
    private int zScalerDir = 1;

    private void Start()
    {
        slime = GetComponent<Slime>();
        
        maxScale = 1.1f * slime.Volume * Vector3.one;
        minScale = 0.9f * slime.Volume * Vector3.one;
    }

    public override void Enter()
    {
        // TODO Handle by Event - OnVolumeChange
        maxScale = 1.1f * slime.Volume * Vector3.one;
        minScale = 0.9f * slime.Volume * Vector3.one;
    }

    public override void Exit()
    {
    }

    public override void Execute()
    {
        var newScale = transform.localScale;

        if (newScale.x > maxScale.x || newScale.x < minScale.x)
            xScalerDir *= -1;
        if (newScale.y > maxScale.y || newScale.y < minScale.y)
            yScalerDir *= -1;
        if (newScale.z > maxScale.z || newScale.z < minScale.z)
            zScalerDir *= -1;

        newScale.x += scaleStep * xScalerDir;
        newScale.y += scaleStep * yScalerDir;
        newScale.z += scaleStep * zScalerDir;
        
        transform.localScale = newScale;
    }
}
