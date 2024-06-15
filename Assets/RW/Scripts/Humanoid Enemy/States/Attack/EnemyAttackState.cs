using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyAttackState : RootMotionState
    {
        Transform player;

        public EnemyAttackState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // check if player is within attack range
            if (!character.PlayerNearby(character.data.MeleeAttackRange, out player))
            {
                // return to idle state if player is not within attack range
                stateMachine.ChangeState(character.idle);
                return;
            }
            // disallow movement
            character.agent.speed = 0f;
            // trigger attack animation
            character.anim.SetTrigger("Attack");
            // equip weapon
            character.Equip(0);
            // deal damage
            character.Attack(character.data.Damage, character.transform);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // ensure player is not null
            if (player == null) return;
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
