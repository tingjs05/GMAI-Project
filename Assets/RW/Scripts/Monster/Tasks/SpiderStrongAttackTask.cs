using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using RayWenderlich.Unity.StatePatternInUnity;

public class SpiderStrongAttackTask : SpiderTask
{
    float timeInAction = 0f;
    float normalizedTimeInAction = 0f;

    [Task]
    public bool CanStrongAttack()
    {
        return bot.CanStrongAttack;
    }

    [Task]
    public void StrongAttack()
    {
        // check for task completion
        if (taskCompleted || normalizedTimeInAction >= 1f)
        {
            // reset strong attack parameters
            EndAttack();
            // mark task as complete
            ThisTask.Succeed();
            // reset task completion
            taskCompleted = false;
            // do not continue action
            return;
        }

        // start attack duration count if time in action is 0
        if (timeInAction <= 0f) StartAttack();
        // increment time in action
        timeInAction += Time.deltaTime;
        // get normalized time
        normalizedTimeInAction = timeInAction / bot.data.StrongAttackDuration;
        // check parry
        CheckParry();
    }

    void StartAttack()
    {
        // reset movement param
        bot.anim.SetFloat("x", 0f);
        // trigger animation
        bot.anim.SetTrigger("StrongAttack");
        // disallow strong attack
        bot.CanStrongAttack = false;
        // set hitbox damage
        bot.hitbox.GetComponent<HitBox>()?.SetDamage(bot.data.StrongDamage); 
    }

    void EndAttack()
    {
        // unsubscribe from event
        bot.Damaged -= Parried;
        // hide parry indicator
        bot.parryIndicator.SetActive(false);
        // deactivate hitbox
        bot.hitbox.SetActive(false);
        // reset time in action
        timeInAction = 0f;
        // reset normalized time in action
        normalizedTimeInAction = 0f;
    }

    void CheckParry()
    {
        // check if within parry window
        bool parryActive = normalizedTimeInAction >= bot.data.NormalizedStartParryWindow &&
            normalizedTimeInAction <= bot.data.NormalizedAttackWindow;
        
        // toggle parry indicator
        bot.parryIndicator.SetActive(parryActive);
        // toggle hitbox, only activate after parry window
        bot.hitbox.SetActive(normalizedTimeInAction >= bot.data.NormalizedAttackWindow);

        // subscribe to damaged event if parry is active
        if (parryActive)
            bot.Damaged += Parried;
        else
            bot.Damaged -= Parried;
    }

    // take damage event listener
    void Parried(float damage)
    {
        // set task as completed
        taskCompleted = true;
        // stun self
        bot.SetStun(true);
        // override current animation and play idle
        bot.anim.Play("Idle");
    }
}
