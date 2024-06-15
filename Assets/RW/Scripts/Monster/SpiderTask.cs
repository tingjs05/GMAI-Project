using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

[RequireComponent(typeof(PandaBehaviour), typeof(SpiderController))] 
public class SpiderTask : MonoBehaviour
{
    // componenets
    protected PandaBehaviour panda;
    protected SpiderController bot;

    // boolean to manage task completion
    protected bool taskCompleted;

    void Awake()
    {
        // get componenets
        panda = GetComponent<PandaBehaviour>();
        bot = GetComponent<SpiderController>();

        // set task completed boolean
        taskCompleted = false;

        // subscribe to cancel task event
        bot.CancelTask += CancelTask;
    }

    // event listener to cancel current task when event is called
    void CancelTask()
    {
        taskCompleted = true;
    }
}
