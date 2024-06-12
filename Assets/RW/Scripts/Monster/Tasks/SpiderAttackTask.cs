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
        return false;
    }

    [Task]
    public void StrongAttack()
    {
        ThisTask.Fail();
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
}
