using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using RayWenderlich.Unity.StatePatternInUnity;

public class SpiderAttackTask : SpiderTask
{
    float timeInAction = 0f;

    // check and chase target
    [Task]
    public bool TargetWithinRange()
    {
        if (bot.PlayerNearby(bot.data.DetectionRange, out Transform player))
        {
            // play running animation
            bot.anim.SetFloat("x", 2f);
            return true;
        }
        return false;
    }

    [Task]
    public void ChaseTarget()
    {
        // check for task completion
        if (taskCompleted)
        {
            taskCompleted = false;
            ThisTask.Fail();
            return;
        }

        // ensure player is within range
        if (!bot.PlayerNearby(bot.data.DetectionRange, out Transform player))
        {
            ThisTask.Fail();
            return;
        }

        // chase player by setting speed and destination
        bot.agent.speed = bot.data.RunSpeed;
        bot.agent.SetDestination(player.position);

        // succeed task once reached player
        if (bot.agent.remainingDistance <= bot.agent.stoppingDistance)
            ThisTask.Succeed();
    }

    // normal attack
    [Task]
    public void Attack(int combo)
    {
        // check for task completion
        if (taskCompleted)
        {
            taskCompleted = false;
            ThisTask.Succeed();
            return;
        }

        // check when to activate hitbox
        CheckHitbox();
        // count attack duration
        if (bot.CounterRunning()) return;
        // reset movement param
        bot.anim.SetFloat("x", 0f);
        // trigger animation
        bot.anim.SetTrigger("Attack");
        // set hitbox damage
        bot.hitbox.GetComponent<HitBox>()?.SetDamage(bot.data.Damage); 
        // start coroutine to count duration in state
        // make first hit shorter, and second hit longer
        bot.CountDuration(bot.data.AttackDuration * (combo == 0 ? 0.75f : 1f), () => 
            {
                taskCompleted = true;
                // deactivate hitbox
                bot.hitbox.SetActive(false);
                // reset time in action
                timeInAction = 0f;
            });
    }

    void CheckHitbox()
    {
        // check if hitbox is already active
        if (bot.hitbox.activeSelf) return;
        // increment time in action
        timeInAction += Time.deltaTime;
        // check if need to activate hitbox
        if ((timeInAction / bot.data.AttackDuration) >= bot.data.NormalizedAttackWindow)
            bot.hitbox.SetActive(true);
    }
}
