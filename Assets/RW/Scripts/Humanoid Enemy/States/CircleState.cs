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
            character.anim.SetBool("Moving", true);
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
            // if (Vector3.Distance(character.transform.position, player.transform.position) <= character.data.MeleeAttackRange)
            // {
            //     // change to circling state to fire arrows
            //     // stateMachine.ChangeState(character.);
            //     return;
            // }

            // circle around the player at this distance
            character.agent.SetDestination(PerpendicularDirection(player));
        }

        public override void Exit()
        {
            base.Exit();
            // stop playing walk animation
            character.anim.SetBool("Moving", false);
        }

        Vector3 PerpendicularDirection(Transform player)
        {
            Vector3 dirToPlayer = (player.position - character.transform.position).normalized;
            Vector3 perpVector = new Vector3(dirToPlayer.z, dirToPlayer.y, -dirToPlayer.x);
            return character.transform.position + (perpVector * 5f * character.agent.stoppingDistance);
        }
    }
}
