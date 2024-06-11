using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class AttackState : MeleeState
    {
        public AttackState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.meleeSwings[0]);
            // activate hitbox
            character.ActivateHitBox();
            // trigger animation
            character.TriggerAnimation(character.SwingMelee);
            // wait for draw animation duration
            Wait(0.25f, () => stateMachine.ChangeState(character.weaponIdle));
        }
        
        public override void Exit()
        {
            base.Exit();
            // deactivate hitbox
            character.DeactivateHitBox();
        }
    }
}