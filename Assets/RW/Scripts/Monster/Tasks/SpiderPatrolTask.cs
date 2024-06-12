using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class SpiderPatrolTask : SpiderTasks
{
    [Task]
    public void Patrol()
    {
        ThisTask.Fail();
    }
}
