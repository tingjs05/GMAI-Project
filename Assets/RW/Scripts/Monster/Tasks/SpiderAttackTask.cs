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
        if (bot.CanStrongAttack)
        {
            // reset movement param
            bot.anim.SetFloat("x", 0f);
            // trigger animation
            bot.anim.SetTrigger("StrongAttack");
            // disallow strong attack
            bot.CanStrongAttack = false;
            return true;
        }
        return false;
    }

    [Task]
    public void StrongAttack()
    {
        // check for task completion
        if (taskCompleted)
        {
            taskCompleted = false;
            ThisTask.Fail();
            return;
        }

        // activate hitbox
        bot.hitbox.SetActive(true);
        // get animation normalized time
        float normalizedTime = bot.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

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
        }

        // check if animation has ended
        if (normalizedTime < 1f) return;
        bot.hitbox.SetActive(false);
        ThisTask.Succeed();
    }

    // normal attack
    [Task]
    public void Attack1()
    {
        ThisTask.Fail();
    }

    [Task]
    public void Attack2()
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
        // interrupt task
        taskCompleted = true;
    }
}
