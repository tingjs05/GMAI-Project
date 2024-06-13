using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class RollState : State
    {
        Coroutine coroutine;
        float startHealth;

        public RollState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // cache current health
            startHealth = character.Health;
            // reset coroutine
            coroutine = null;
            // trigger animation
            character.TriggerAnimation(character.rollParam);
            // display state on UI 
            DisplayOnUI(UIManager.Alignment.Left);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check if health has decreased, detect dodging through an attack
            if (character.Health < startHealth) HandleDodge();
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
            // reset time scale
            Time.timeScale = 1f;
        }

        void HandleDodge()
        {
            // play dodge sound effect
            SoundManager.Instance.PlaySound(SoundManager.Instance.dodge);
            // revert damage done
            character.Damage(character.Health - startHealth);
            // temporarily slow down time
            if (coroutine != null) character.StopCoroutine(coroutine);
            coroutine = character.StartCoroutine(SlowDownTime(0.5f));
        }

        IEnumerator SlowDownTime(float duration)
        {
            Time.timeScale = 0.5f;
            yield return new WaitForSeconds(duration);
            Time.timeScale = 1f;
        }
    }
}
