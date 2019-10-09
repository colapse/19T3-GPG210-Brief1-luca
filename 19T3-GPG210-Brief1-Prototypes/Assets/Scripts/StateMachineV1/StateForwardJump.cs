using DG.Tweening;
using UnityEngine;

namespace StateMachineV1
{
    public class StateForwardJump : StateBase
    {
        public float jumpForce = 150;
        
        Rigidbody rb;
        float distanceToGround = 0;

        private float jumpCharge = 1;
        private bool jumped = false;
        
        public float elasticityMultiplier = 1f;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void Enter()
        {
            jumpCharge = 1;
            jumped = false;
            distanceToGround = GetComponent<Collider>().bounds.extents.y;
        
        }

        public override void Exit()
        {
        
        }

        public override void Execute()
        {
            Slime slime = GetComponent<Slime>();
            
            if (jumpCharge < 3 && owner.slimeInputManager.inputForwardJump)
            {
                jumpCharge += Time.deltaTime*6;
            }
        
            if (IsGrounded() && !jumped && (!owner.slimeInputManager.inputForwardJump || jumpCharge >= 3))
            {
                Vector3 forwardJumpForce = (slime.rb.mass) * jumpForce * transform.forward;
                forwardJumpForce.y = 1.5f * slime.rb.mass * jumpForce;
        
                rb.AddForce(forwardJumpForce*jumpCharge);
                jumped = true;
                
                DOTween.To(() => transform.localScale, (x) => transform.localScale = x, new Vector3(0.4f*slime.Volume*elasticityMultiplier, 0.65f*slime.Volume*elasticityMultiplier, 0.65f*slime.Volume*elasticityMultiplier), .75f).SetEase(Ease.OutElastic).OnComplete(()=>
                {
                    DOTween.To(() => transform.localScale, (x) => transform.localScale = x,
                        new Vector3(0.5f*slime.Volume, 0.5f*slime.Volume, 0.5f*slime.Volume),.6f).SetEase(Ease.OutElastic); // TODO Setelay HACK. The reverse function should be executed ongrounded!
                });
            }
        
            if (IsGrounded() && rb.velocity.y < 0 && jumped)
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
