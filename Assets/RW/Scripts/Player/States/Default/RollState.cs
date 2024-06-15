using System.Collections;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class RollState : State
    {
        float startHealth;

        public RollState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // set dodge direction
            SetDodgeDirection();
            // cache current health
            startHealth = character.Health;
            // trigger animation
            character.TriggerAnimation(character.rollParam);
            // change collider size
            character.ColliderSize = character.CrouchColliderHeight;
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
            // end dodge, check if below ceiling
            if (character.CheckCollisionOverlap(character.transform.position + Vector3.up * character.NormalColliderHeight))
                stateMachine.ChangeState(character.ducking);
            else
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
            // revert collider size
            character.ColliderSize = character.NormalColliderHeight;
        }

        void SetDodgeDirection()
        {
            // get player input
            Vector3 input = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
            if (input == Vector3.zero) return;

            // reset roll direction vector to player forward vector
            Vector3 rollDir = character.transform.forward;

            // rotate player forward vector depending on input
            if (input.x != 0f)
                rollDir *= input.x < 0? -1 : 1;
            if (input.z > 0f)
                rollDir = new Vector3(rollDir.z, 0f, -rollDir.x);
            if (input.z < 0f)
                rollDir = new Vector3(-rollDir.z, 0f, rollDir.x);
            
            // set final roll direction
            character.transform.forward = rollDir;
        }

        void HandleDodge()
        {
            // play dodge sound effect
            SoundManager.Instance.PlaySound(SoundManager.Instance.dodge);
            // revert damage done
            character.Damage(character.Health - startHealth);
            // cancel hit animation trigger
            character.ResetTrigger(character.hitParam);
        }
    }
}
