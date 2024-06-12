using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class AttackState1 : MeleeState
    {
        bool attacked;

        public AttackState1(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // reset bools
            attacked = false;
            // play sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.meleeSwings[0]);
            // activate hitbox
            character.ActivateHitBox();
            // trigger animation
            character.TriggerAnimation(character.SwingMelee);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // check whether player clicked again
            if (!attacked && triggerAttack) 
                attacked = true;
                
            // check if animation has ended, if so, switch state
            if (character.GetAnimationState(1).normalizedTime < 1) return;
            stateMachine.ChangeState(attacked ? character.attack2 : character.weaponIdle);
        }

        public override void Exit()
        {
            base.Exit();
            // deactivate hitbox
            character.DeactivateHitBox();
        }
    }
}