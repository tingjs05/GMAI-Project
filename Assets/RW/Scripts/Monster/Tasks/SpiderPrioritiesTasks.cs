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
        return bot.Died;
    }

    [Task]
    public void Die()
    {
        // count duration for death animation
        if (bot.CounterRunning()) return;
        // start coroutine to count animation duration
        // destroy game object after death animation
        bot.CountDuration(bot.data.DeathDuration, () => Destroy(gameObject));
        // set death animation
        bot.anim.SetFloat("x", 0f);
        bot.anim.SetTrigger("Die");
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
            // set task as complete
            taskCompleted = false;
            bot.SetStun(false);
            ThisTask.Succeed();
            // unflip spider
            // transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            // transform.rotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
            return;
        }

        // only start coroutine counter if not already running
        if (bot.CounterRunning()) return;
        // start coroutine
        bot.CountDuration(bot.data.StunDuration, () => taskCompleted = true);
        // flip spider
        // transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        // transform.rotation = Quaternion.Euler(transform.forward.x * 180f, transform.rotation.y, transform.forward.z * 180f);
    }
}
