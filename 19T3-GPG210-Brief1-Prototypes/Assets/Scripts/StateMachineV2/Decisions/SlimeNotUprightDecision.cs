using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Decisions/EnemyNotUpright")]
public class SlimeNotUprightDecision : Decision
{
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
        return !SlimeInputManager.IsUpright(controller.transform);
    }
}
