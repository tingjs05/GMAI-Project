using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Astar.Pathfinding;

public class SpiderAttackTask : SpiderTask
{
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

    // strong attack
    [Task]
    public bool CanStrongAttack()
    {
        return bot.CanStrongAttack;
    }

    float timeInAction = 0f;

    [Task]
    public void StrongAttack()
    {
        // check for task completion
        if (taskCompleted)
        {
            taskCompleted = false;
            ThisTask.Succeed();
            return;
        }

        // increment time in action
        timeInAction += Time.deltaTime;
        // get animation normalized time
        float normalizedTime = timeInAction / bot.data.StrongAttackDuration;

        // check parry
        if (normalizedTime >= bot.data.NormalizedStartParryWindow && 
            normalizedTime <= (bot.data.NormalizedStartParryWindow + bot.data.NormalizedParryWindow))
        {
            // subscribe to damaged event
            bot.Damaged += Parried;
            // show parry indicator
            bot.parryIndicator.SetActive(true);
        }
        // stop parry window
        else
        {
            // unsubscribe from damaged event
            bot.Damaged -= Parried;
            // hide parry indicator
            bot.parryIndicator.SetActive(false);
            // activate hitbox
            bot.hitbox.SetActive(true);
        }

        // count attack duration
        if (bot.CounterRunning()) return;
        // reset movement param
        bot.anim.SetFloat("x", 0f);
        // trigger animation
        bot.anim.SetTrigger("StrongAttack");
        // disallow strong attack
        bot.CanStrongAttack = false;
        // reset time in action
        timeInAction = 0f;
        // start coroutine to count duration in state
        bot.CountDuration(bot.data.StrongAttackDuration, () => 
            {
                taskCompleted = true;
                timeInAction = 0f;
                // deactivate hitbox
                bot.hitbox.SetActive(false);
            });
    }

    // normal attack
    [Task]
    public void Attack(int combo)
    {
        ThisTask.Fail();
    }

    // parry event listener
    void Parried(float damage)
    {
        // unsubscribe from event
        bot.Damaged -= Parried;
        // set stun to true
        bot.SetStun(true);
        // hide parry indicator
        bot.parryIndicator.SetActive(false);
        // deactivate hitbox
        bot.hitbox.SetActive(false);
        // interrupt task
        taskCompleted = true;
    }
}
