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
            // trigger animation
            character.TriggerAnimation(character.SheathMelee);
            // wait for draw animation duration
            Wait(0.05f, () => stateMachine.ChangeState(character.weaponIdle));
        }

        public override void Exit()
        {
            base.Exit();
            // sheath weapon
            character.SheathWeapon();
        }
    }
}
