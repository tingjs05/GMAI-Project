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
        // destroy game object after death animation
        if (bot.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) return;
        Destroy(gameObject);
        ThisTask.Succeed();
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
