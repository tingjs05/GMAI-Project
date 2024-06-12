using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

[RequireComponent(typeof(PandaBehaviour), typeof(SpiderController))] 
public class SpiderTasks : MonoBehaviour
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
        }
}
