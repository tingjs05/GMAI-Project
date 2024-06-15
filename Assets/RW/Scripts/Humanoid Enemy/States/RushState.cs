using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class RushState : EnemyState
    {
        Rigidbody rb;

        public RushState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
            // get rigidbody reference
            rb = character.GetComponent<Rigidbody>();
        }

        public override void Enter()
        {
            base.Enter();
            // do not let agent move, manually handle movement
            character.agent.speed = 0f;
            // play rush animation
            character.anim.SetFloat("Movement", 2f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // check if can rush, otherwise return to circle state
            // or if within attack range, also return to circle state
            if (!character.CanRush(out Transform player) || Vector3.Distance(character.transform.position, player.position) <= character.data.MeleeAttackRange)
            {
                stateMachine.ChangeState(character.circle);
                return;
            }

            // rush towards player
            rb.velocity = (player.position - character.transform.position).normalized * character.data.RushSpeed;
        }

        public override void Exit()
        {
            base.Exit();
            // reset movement animation
            character.anim.SetFloat("Movement", 0f);
            // count rush cooldown
            character.rushCoroutine = character.StartCoroutine(WaitForRushCooldown(Random.Range(character.data.RushCooldown.x, character.data.RushCooldown.y)));
        }

        IEnumerator WaitForRushCooldown(float duration)
        {
            yield return new WaitForSeconds(duration);
            // reset rush counter
            character.rushCoroutine = null;
        }
    }

}
