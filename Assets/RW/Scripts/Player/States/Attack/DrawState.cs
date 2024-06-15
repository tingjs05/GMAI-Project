using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class DrawState : MeleeState
    {
        public DrawState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.meleeEquip);
            // draw weapon
            character.Equip();
            // trigger animation
            character.TriggerAnimation(character.DrawMelee);
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
