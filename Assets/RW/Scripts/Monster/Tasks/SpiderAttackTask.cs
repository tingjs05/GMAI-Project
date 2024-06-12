using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class SpiderAttackTask : SpiderTasks
{
    // check and chase target
    [Task]
    public bool TargetWithinRange()
    {
        return false;
    }

    [Task]
    public void ChaseTarget()
    {
        ThisTask.Fail();
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
