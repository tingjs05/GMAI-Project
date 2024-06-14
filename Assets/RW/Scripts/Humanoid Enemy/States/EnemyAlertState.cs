using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayWenderlich.Unity.StatePatternInUnity;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyAlertState : EnemyState
    {
        public EnemyAlertState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
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

            // check if player is within detection range
            if (!character.PlayerNearby(character.data.DetectionRange, out Transform player))
            {
                // return to idle if player is not within range
                stateMachine.ChangeState(character.idle);
                return;
            }

            // check if player is within shooting range
            // if (Vector3.Distance(character.transform.position, player.transform.position) <= character.data.RangedAttackRange)
            // {
            //     // change to circling state to fire arrows
            //     // stateMachine.ChangeState(character.);
            //     return;
            // }

            // walk towads the player to close the distance
            character.agent.SetDestination(player.transform.position);
        }

        public override void Exit()
        {
            base.Exit();
            // stop playing walk animation
            character.anim.SetBool("Moving", false);
        }
    }
}
