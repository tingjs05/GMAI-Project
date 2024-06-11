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
            // draw weapon
            character.Equip();
            // trigger animation
            character.TriggerAnimation(character.DrawMelee);
            // wait for draw animation duration
            // Wait(character.GetAnimationDuration(1), () => stateMachine.ChangeState(character.weaponIdle));
            Wait(0.05f, () => stateMachine.ChangeState(character.weaponIdle));
        }
    }
}
