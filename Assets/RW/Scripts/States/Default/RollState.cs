using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class RollState : State
    {
        Vector3 targetVelocity;

        public RollState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play sound
            // SoundManager.Instance.PlaySound(SoundManager.Instance.);
            // activate hitbox
            character.ActivateHitBox();
            // trigger animation
            character.TriggerAnimation(character.rollParam);
            // wait for draw animation duration
            Wait(0.5f, () => stateMachine.ChangeState(character.standing));
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            // move player forward
            targetVelocity = character.RollSpeed * character.transform.forward * Time.deltaTime;
            targetVelocity.y = character.rb.velocity.y;
            character.rb.velocity = targetVelocity;
        }


        public override void Exit()
        {
            base.Exit();
            // deactivate hitbox
            character.DeactivateHitBox();
        }
    }
}
