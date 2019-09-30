using UnityEngine;

namespace StateMachineV1
{
    public class StateJump : StateBase
    {
        Rigidbody rb;
        float distanceToGround = 0;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            distanceToGround = GetComponent<Collider>().bounds.extents.y;
        }

        public override void Enter()
        {
            if (rb == null)
                return;


            rb.AddForce(rb.mass * 300 * Vector3.up);

        }

        public override void Exit()
        {
        
        }

        public override void Execute()
        {
            if (transform.rotation.eulerAngles.x < -45 || transform.rotation.eulerAngles.x > 45 || transform.rotation.eulerAngles.z < -45 ||
                transform.rotation.eulerAngles.z > 45)
            {
                if(IsGrounded())
                     rb.AddForce(rb.mass * 0.1f * Vector3.up);
            
                Quaternion targetRotation = Quaternion.identity;
                //targetRotation.y = transform.localRotation.y;
            
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 500f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100f * Time.deltaTime);
            }
        
            if (IsGrounded() && rb.velocity.y < 0)
            {
                GetComponent<StateManager>()?.ChangeState(nextState);
            }
        }

        //Ugly
        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
        }
    }
}
