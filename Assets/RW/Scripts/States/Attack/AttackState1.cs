using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class AttackState1 : MeleeState
    {
        bool hasEnded, attacked;

        public AttackState1(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            // reset bools
            hasEnded = false;
            attacked = false;

            // play sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.meleeSwings[0]);
            // activate hitbox
            character.ActivateHitBox();
            // trigger animation
            character.TriggerAnimation(character.SwingMelee);
            // wait for draw animation duration
            Wait(0.5f, () => hasEnded = true);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check whether player clicked again
            if (!attacked && triggerAttack) 
                attacked = true;
            // check exit
            if (hasEnded)
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