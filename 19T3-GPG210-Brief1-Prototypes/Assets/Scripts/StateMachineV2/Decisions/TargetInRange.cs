using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Decisions/TargetInRange")]
public class TargetInRange : Decision
{
    public float range;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool Decide(StateController controller)
    {
        if (controller.inputManager.currentTarget == null)
            return false;

        Collider c = controller.GetComponent<Collider>();
        float extents = c?.bounds.extents.x ?? 0;
        
        var distance = Vector3.Distance(controller.inputManager.currentTarget.transform.position,
            controller.transform.position)-extents;
        

        return distance <= range;
    }
}
