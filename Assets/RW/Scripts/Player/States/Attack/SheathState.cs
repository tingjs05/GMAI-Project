using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class SheathState : MeleeState
    {
        public SheathState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.meleeSheath);
            // sheath weapon
            character.SheathWeapon();
            // trigger animation
            character.TriggerAnimation(character.SheathMelee);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check if animation has ended, if so, switch state
            if (character.GetAnimationState(1).normalizedTime < 1) return;
            stateMachine.ChangeState(character.weaponIdle);
        }
    }
}
