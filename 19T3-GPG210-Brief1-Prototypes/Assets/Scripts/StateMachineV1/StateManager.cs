using UnityEngine;

namespace StateMachineV1
{
    public class StateManager : MonoBehaviour
    {
        public StateBase currentState;
        public StateBase defaultState;

        public StateBase jumpState;
        public StateBase idleState;
        public StateBase randomPushState;
        public StateBase rotateLeftState;
        public StateBase rotateRightState;
        public StateBase feedState;

        public SlimeInputManager slimeInputManager;

        private Slime slime;
        private SlimeManager sm;
        private bool isActiveSlime = false;
    
        // Start is called before the first frame update
        void Start()
        {
            slime = GetComponent<Slime>();
            
            // TODO HACK
            if (gameObject.CompareTag("Player"))
            {
                sm = FindObjectOfType<UnityEngine.Camera>()?.GetComponent<SlimeManager>();
            }
            
            currentState = defaultState;
        }

        // Update is called once per frame
        void Update()
        {
            // TODO HACK
            if(gameObject.CompareTag("Player"))
                isActiveSlime = (sm.activeSlime == slime);
            else
            {
                isActiveSlime = true;
            }
        
            if(currentState != null)
                currentState.Execute();

        
        }

        private void LateUpdate()
        {
            if (!isActiveSlime)
                return;
        
            if (/*Input.GetKeyUp(KeyCode.Space)*/ slimeInputManager.inputJump && currentState != jumpState)
            {
                ChangeState(jumpState);
            }

            if (/*Input.GetKeyDown(KeyCode.Alpha1)*/ slimeInputManager.inputForwardJump && currentState != randomPushState)
            {
                ChangeState(randomPushState);
            }

            if (/*Input.GetKeyDown(KeyCode.Alpha2)*/ slimeInputManager.inputFeed && currentState != feedState)
            {
                ChangeState(feedState);
            }
        
            if(/*Input.GetKeyDown(KeyCode.LeftArrow)*/ slimeInputManager.inputTurnLeft && currentState != rotateLeftState)
                ChangeState(rotateLeftState);
        
            if(/*Input.GetKeyDown(KeyCode.RightArrow)*/ slimeInputManager.inputTurnRight && currentState != rotateRightState)
                ChangeState(rotateRightState);
        }

        public void ChangeState(StateBase newState)
        {
            if (currentState == newState)
                return;

            if (currentState != null)
                currentState.Exit();

            currentState = newState;
            if (newState != null)
            {
                newState.owner = this; // Hacky & unnecessary to set on each time.
                newState.Enter();
            }

        }
    }
}
