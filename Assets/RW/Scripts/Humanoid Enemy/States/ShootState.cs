using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class ShootState : EnemyState
    {
        Vector3 shootDir;

        public ShootState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // disallow movement
            character.agent.speed = 0f;
            // equip bow
            character.Equip(1);
            // play shoot animation
            character.anim.SetTrigger("Shoot");
            // play voiceline
            character.voiceManager.PlaySound(character.voiceManager.arrow);
            // increment shoot count
            character.shotsFired++;
            // set shoot direction
            SetShootDirection();
            // start coroutine to count state duration
            character.CountDuration(character.data.ShootAnimDuration, 
                () => stateMachine.ChangeState(character.idle));
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // look at shoot direction
            character.transform.forward = shootDir;
        }

        public override void Exit()
        {
            base.Exit();
            // create arrow
            character.ShootArrow();
            // unequip weapons
            character.Unequip();
            // start counter for shot cooldown
            character.shootCoroutine = character.StartCoroutine(
                WaitForShotCooldown(
                        character.shotsFired >= character.data.ShotsInARow ? 
                        Random.Range(character.data.ShotGroupCooldown.x, character.data.ShotGroupCooldown.y) : 
                        character.data.ShotCooldown
                    )
                );
        }

        void SetShootDirection()
        {
            if (!character.PlayerNearby(character.data.RangedAttackRange, out Transform player)) return;
            shootDir = (player.position - character.transform.position).normalized;
            shootDir.y = 0f;
        }

        IEnumerator WaitForShotCooldown(float duration)
        {
            yield return new WaitForSeconds(duration);
            // reset shot counter
            character.shootCoroutine = null;
            // reset shots if duration is longer than duration of normal shots
            if (duration > character.data.ShotCooldown)
                character.shotsFired = 0;
        }
    }
}
