using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyComboAttackState : RootMotionState
    {
        public EnemyComboAttackState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // check if player is within attack range
            if (!character.PlayerNearby(character.data.MeleeAttackRange, out Transform player))
            {
                // return to idle state if player is not within attack range
                stateMachine.ChangeState(character.idle);
                return;
            }
            // face player
            character.transform.forward = (player.position - character.transform.position).normalized;
            // disallow movement
            character.agent.speed = 0f;
            // trigger attack animation
            character.anim.SetTrigger("ComboAttack");
            // equip weapon
            character.Equip(0);
            // start coroutine to count animation duration, then return to idle state
            character.CountDuration(character.data.ComboAttackDuration, () => stateMachine.ChangeState(character.idle));
        }

        public override void Exit()
        {
            base.Exit();
            // unequip weapon
            character.Unequip();
        }
    }
}
