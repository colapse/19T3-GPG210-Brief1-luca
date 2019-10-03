using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/PickEnemyAction")]
public class PickEnemyAction : Action
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEnter(StateController controller, ActionData actionData)
    {
        
    }

    public override void Act(StateController controller, ActionData actionData)
    {
        if (controller.inputManager.currentTarget != null || controller.inputManager.enemiesInSight == null || controller.inputManager.enemiesInSight.Count == 0)
            return;

        Slime me = controller.GetComponent<Slime>();
        if (me == null)
            return;
        
        Slime target = null;
        float targetDistance = 0f;

        foreach (var enemy in controller.inputManager.enemiesInSight)
        {
            if (enemy == null)
                continue;
            
            var enemyDistance = Vector3.Distance(enemy.transform.position, me.transform.position);
            if (target == null)
            {
                target = enemy;
                targetDistance = enemyDistance;
                continue;
            }

            if (target.Volume >= me.Volume)
            {
                if (enemy.Volume >= me.Volume)
                {
                    if (!(enemyDistance < targetDistance)) continue;
                    target = enemy;
                    targetDistance = enemyDistance;
                }
                else
                {
                    target = enemy;
                    targetDistance = enemyDistance;
                }
            }
            else
            {
                if (!(enemy.Volume < me.Volume)) continue;
                if (!(enemyDistance < targetDistance)) continue;
                target = enemy;
                targetDistance = enemyDistance;
            }
        }

        controller.inputManager.currentTarget = target;

    }

    public override void OnExit(StateController controller, ActionData actionData)
    {
        
    }
}
