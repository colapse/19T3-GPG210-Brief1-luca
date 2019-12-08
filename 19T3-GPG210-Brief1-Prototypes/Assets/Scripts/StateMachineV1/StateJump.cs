using DG.Tweening;
using UnityEngine;

namespace StateMachineV1
{
    public class StateJump : StateBase
    {
        public float jumpForceMultiplier = 100;
        public float rotationSpeed = 5;
        
        Rigidbody rb;
        float distanceToGround = 0;
        private bool forwardPushDone = false; // HACK
        private bool jumpForceAdded = false; // HACK
        private bool getUpright = false;
        private bool liftedOff = false;
        private Collider collider;
        
        public float elasticityMultiplier = 1f;

        public bool doTweenSquashEffect = false;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
        }

        public override void Enter()
        {
            if (rb == null)
                return;
            jumpForceAdded = false;
            forwardPushDone = false;
            getUpright = false;
            liftedOff = false;
            
            distanceToGround = collider.bounds.extents.y;
        }

        public override void Exit()
        {
        }

        public override void Execute()
        {
            
            if (!IsGrounded() && !liftedOff)
            {
                liftedOff = true;
            }
            
            if (!getUpright && IsGrounded() && !SlimeInputManager.IsUpright(transform,30))
            {
                getUpright = true;
                if (!jumpForceAdded)
                {
                    Vector3 force = rb.mass * jumpForceMultiplier * Vector3.up;
                    rb.AddForce(force);
                    jumpForceAdded = true;
                }

                return;
            }
            
            // Make slime upright if its not (Move upright if angle is >30° while grounded, if not grounded execute until angle is less than 5°
            if (getUpright && !IsGrounded() && !SlimeInputManager.IsUpright(transform,10)/*(transform.rotation.eulerAngles.x < -20 || transform.rotation.eulerAngles.x > 20 || transform.rotation.eulerAngles.z < -20 ||
                transform.rotation.eulerAngles.z > 20)*/)
            {
                Quaternion targetRot = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
                
                /*
                if(IsGrounded())
                     rb.AddForce(rb.mass * 0.1f * Vector3.up);*/
            
                ////Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.y,0);//Quaternion.identity;
                //targetRotation.y = transform.localRotation.y;
            
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 500f);
                ////transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);
                return;
            }

            //HACK Forwardpush while jumping; Input Hack
            if (jumpForceAdded && !IsGrounded() && Input.GetKey(KeyCode.LeftShift) && !forwardPushDone)
            {
                Slime slime = GetComponent<Slime>();
                Vector3 forwardForce = (slime.rb.mass) * 100 * transform.forward;
                forwardForce.y = 0;
                rb.AddForce(forwardForce);
                forwardPushDone = true;
            }
            
            if (IsGrounded() && !jumpForceAdded && rb.velocity.y <= 0)
            {
                Vector3 force = rb.mass * jumpForceMultiplier * Vector3.up;
                
                rb.AddForce(force);
                jumpForceAdded = true;
                Slime slime = owner.GetComponent<Slime>(); // hack
                if (doTweenSquashEffect)
                {
                    DOTween.To(() => transform.localScale, (x) => transform.localScale = x, new Vector3(0.25f*slime.Volume*elasticityMultiplier, 0.75f*slime.Volume*elasticityMultiplier, 0.25f*slime.Volume*elasticityMultiplier), 1f).SetEase(Ease.OutElastic).OnComplete(()=>
                    {
                        DOTween.To(() => transform.localScale, (x) => transform.localScale = x,
                            new Vector3(0.5f*slime.Volume, 0.5f*slime.Volume, 0.5f*slime.Volume),1f).SetEase(Ease.OutElastic).SetDelay(.5f); // TODO Setelay HACK. The reverse function should be executed ongrounded!
                    });
                }
            }
            
            
        
            if (jumpForceAdded && liftedOff && (IsGrounded() && (rb.velocity.y <= 0 && rb.velocity.y > -.9)))
            {
                owner.ChangeState(nextState);
            }
        }

        //Ugly
        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
        }
    }
}
