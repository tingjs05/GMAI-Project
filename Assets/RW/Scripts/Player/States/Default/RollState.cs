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
            // trigger animation
            character.TriggerAnimation(character.rollParam);
            // display state on UI 
            DisplayOnUI(UIManager.Alignment.Left);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check if animation has ended, if so, switch state
            if (character.GetAnimationState(0).normalizedTime < 1) return;
            stateMachine.ChangeState(character.standing);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            // move player forward, do not play animation
            character.Move(character.RollSpeed, 0, false);
        }

        public override void Exit()
        {
            base.Exit();
            SoundManager.Instance.StopPlaying();
        }
    }
}
