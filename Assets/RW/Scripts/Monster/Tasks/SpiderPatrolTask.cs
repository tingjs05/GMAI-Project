using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class SpiderPatrolTask : SpiderTask
{
    [Task]
    public void Patrol()
    {
        // set the bot speed to walk speed
        bot.agent.speed = bot.data.WalkSpeed;
        // set a new destination if reached target location
        if (bot.agent.remainingDistance <= bot.agent.stoppingDistance)
        {
            // play walk animation
            bot.anim.SetFloat("x", 1f);
            // get a random point to walk to and set target position to walk towards
            bot.agent.SetDestination(bot.RandomPoint(transform.position, bot.data.PatrolRange));
        }
        // complete task
        ThisTask.Succeed();
    }
}
