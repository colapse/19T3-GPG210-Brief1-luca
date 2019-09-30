using UnityEngine;

namespace StateMachineV1
{
    public class StateIdle : StateBase
    {
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Execute()
        {
            transform.localScale = new Vector3(Mathf.PerlinNoise(Time.time,0), Mathf.PerlinNoise(Time.time, 0), Mathf.PerlinNoise(Time.time, 0));
        }
    }
}
