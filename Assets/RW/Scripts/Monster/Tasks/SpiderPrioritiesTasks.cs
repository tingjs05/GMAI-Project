using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class SpiderPrioritiesTasks : SpiderTasks
{
    // death tree
    [Task]
    public bool CheckDeath()
    {
        return false;
    }

    [Task]
    public void Die()
    {
        ThisTask.Fail();
    }

    // stun tree
    [Task]
    public bool CheckStun()
    {
        return false;
    }

    [Task]
    public void Stun()
    {
        ThisTask.Fail();
    }
}
