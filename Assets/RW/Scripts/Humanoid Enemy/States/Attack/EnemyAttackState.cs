using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyAttackState : RootMotionState
    {
        public EnemyAttackState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // disallow movement
            character.agent.speed = 0f;
            // trigger attack animation
            character.anim.SetTrigger("Attack");
            // equip weapon
            character.Equip(0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check if player is within attack range
            if (!character.PlayerNearby(character.data.MeleeAttackRange, out Transform player))
            {
                // return to idle state if player is not within attack range
                stateMachine.ChangeState(character.idle);
                return;
            }
            // face player
            character.transform.forward = (player.position - character.transform.position).normalized;
        }

        public override void Exit()
        {
            base.Exit();
            // unequip weapon
            character.Unequip();
        }
    }
}
