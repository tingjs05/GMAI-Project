using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class CircleState : EnemyState
    {
        public CircleState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // set speed to walk speed
            character.agent.speed = character.data.WalkSpeed;
            // play walk animation
            character.anim.SetFloat("Movement", 1f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // check if player is within ranged attack range
            if (!character.PlayerNearby(character.data.RangedAttackRange, out Transform player))
            {
                // return to alert if player is not within range
                stateMachine.ChangeState(character.alert);
                return;
            }

            // check if player is within melee range
            if (Vector3.Distance(character.transform.position, player.transform.position) <= character.data.MeleeAttackRange)
            {
                // check if can perform combo attack, otherwise perform normal attack
                stateMachine.ChangeState(character.comboCoroutine == null ? character.comboAttack : character.attack1);
                return;
            }

            // check if can rush player
            if (character.CanRush(out Transform _player))
            {
                stateMachine.ChangeState(character.rush);
                return;
            }

            // check if can shoot player (in shoot cooldown)
            if (character.shootCoroutine == null)
            {
                stateMachine.ChangeState(character.shoot);
                return;
            }

            // circle around the player at this distance
            character.agent.SetDestination(PerpendicularDirection(player));
        }

        public override void Exit()
        {
            base.Exit();
            // stop playing walk animation
            character.anim.SetFloat("Movement", 0f);
        }

        Vector3 PerpendicularDirection(Transform player)
        {
            Vector3 dirToPlayer = (player.position - character.transform.position).normalized;
            Vector3 perpVector = new Vector3(dirToPlayer.z, dirToPlayer.y, -dirToPlayer.x);
            return character.transform.position + (perpVector * 5f * character.agent.stoppingDistance);
        }
    }
}
