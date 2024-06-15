using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyComboAttackState : RootMotionState
    {
        float startHealth;
        Transform player;

        public EnemyComboAttackState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // check if player is within attack range and get a reference to player
            character.PlayerNearby(character.data.MeleeAttackRange, out player);
            // cache start health
            startHealth = character.Health;
            // equip weapon
            character.Equip(0);
            // disallow movement
            character.agent.speed = 0f;
            // trigger attack animation
            character.anim.SetTrigger("Combo");
            // play voiceline
            character.voiceManager.PlaySound(character.voiceManager.comboAttack);
            // start coroutine to count animation duration, then return to idle state
            character.CountDuration(character.data.ComboAttackDuration, () => stateMachine.ChangeState(character.idle));
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // revert damages during combo state
            if (character.Health < startHealth)
            {
                character.Damage(character.Health - startHealth);
            }
            // ensure player is not null
            if (player == null) return;
            // face player
            character.transform.forward = (player.position - character.transform.position).normalized;
        }

        public override void Exit()
        {
            base.Exit();
            // count combo cooldown
            character.comboCoroutine = character.StartCoroutine(WaitForComboCooldown(Random.Range(character.data.ComboAttackCooldown.x, character.data.ComboAttackCooldown.y)));
        }

        IEnumerator WaitForComboCooldown(float duration)
        {
            yield return new WaitForSeconds(duration);
            // reset combo attack counter
            character.comboCoroutine = null;
            // reset trigger
            character.anim.ResetTrigger("Combo");
        }
    }
}
