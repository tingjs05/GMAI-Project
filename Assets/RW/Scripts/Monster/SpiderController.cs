using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar.Pathfinding;

[RequireComponent(typeof(SpiderData), typeof(Agent))]
public class SpiderController : MonoBehaviour
{
    // componenets
    public SpiderData data { get; private set;}
    public Agent agent { get; private set; }
    public Animator anim { get; private set; }

    // children objects
    GameObject hitbox;
    GameObject parryIndicator;

    // Start is called before the first frame update
    void Start()
    {
        // get components
        data = GetComponent<SpiderData>();
        agent = GetComponent<Agent>();
        anim = GetComponentInChildren<Animator>();
        
        // get children objects
        hitbox = transform.GetChild(1).gameObject;
        parryIndicator = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
