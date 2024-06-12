using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class SpiderPrioritiesTasks : SpiderTask
{
    // death tree
    [Task]
    public bool CheckDeath()
    {
        if (bot.Died)
        {
            // set death animation
            bot.anim.SetTrigger("Die");
            return true;
        }
        return false;
    }

    [Task]
    public void Die()
    {
        if (taskCompleted)
        {
            taskCompleted = false;
            // destroy game object after death animation
            Destroy(gameObject);
            // mark task as successful
            ThisTask.Succeed();
            return;
        }
        // count duration for death animation
        if (bot.CounterRunning()) return;
        bot.CountDuration(bot.data.DeathDuration, () => taskCompleted = true);
    }

    // stun tree
    [Task]
    public bool CheckStun()
    {
        return bot.Stunned;
    }

    [Task]
    public void Stun()
    {
        // complete task once counter is over
        if (taskCompleted)
        {
            taskCompleted = false;
            ThisTask.Succeed();
            return;
        }

        // only start coroutine counter if not already running
        if (!bot.CounterRunning()) return;
        bot.CountDuration(bot.data.StunDuration, () => 
            {
                bot.SetStun(false);
                taskCompleted = true;
            });
    }
}
