using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class RollState : State
    {
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
            Wait(1f, () => stateMachine.ChangeState(character.standing));
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // move player
            character.Move(character.RollSpeed, 0f);
        }


        public override void Exit()
        {
            base.Exit();
            // deactivate hitbox
            character.DeactivateHitBox();
        }
    }
}
