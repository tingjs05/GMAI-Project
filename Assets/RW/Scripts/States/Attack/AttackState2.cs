using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class AttackState2 : MeleeState
    {
        public AttackState2(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.meleeSwings[2]);
            // activate hitbox
            character.ActivateHitBox();
            // trigger animation
            character.TriggerAnimation(character.SwingMelee);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check if animation has ended, if so, switch state
            if (character.GetAnimationState(1).normalizedTime < 1) return;
            stateMachine.ChangeState(character.weaponIdle);
        }
        
        public override void Exit()
        {
            base.Exit();
            // deactivate hitbox
            character.DeactivateHitBox();
        }
    }
}