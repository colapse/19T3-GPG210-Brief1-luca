using UnityEngine;

namespace StateMachineV1
{
    public class StateFeed : StateBase
    {
        public Transform feedExhaust;
        public GameObject slimeFeedPrefab;

        private void Start()
        {
        }

        public override void Enter()
        {
            Slime slime = GetComponent<Slime>();

            if (slime.Volume < 0.5f)
                return;
        
            GameObject feedObj = Instantiate(slimeFeedPrefab, feedExhaust.position, Quaternion.identity);
            Slime feedSlime = feedObj.GetComponent<Slime>();
            feedSlime.Volume = 0.2f;
            Vector3 feedForce = transform.forward * 500;
            feedForce.y = 30;
            feedObj.GetComponent<Rigidbody>()?.AddForce(feedForce);

            slime.Volume -= 0.2f;
        
        }

        public override void Exit()
        {
        
        }

        public override void Execute()
        {
        
            GetComponent<StateManager>()?.ChangeState(nextState);
        }
    }
}
