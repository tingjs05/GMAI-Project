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

    // coroutine
    Coroutine coroutine;

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

    // coroutines to count duration of an action
    public void CountDuration(float duration, System.Action callback = null)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(CountDurationCoroutine(duration, callback));
    }

    IEnumerator CountDurationCoroutine(float duration, System.Action callback = null, bool resetCoroutine = true)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
        if (resetCoroutine) coroutine = null;
    }

    // check if player is nearby within a certain range around the enemy
    public bool PlayerNearby(float range, out Transform player)
    {
        // use sphere cast all, check all nearby objects
        Collider[] hits = Physics.OverlapSphere(transform.position, range, data.PlayerMask);
        // check if anything is hit
        if (hits.Length > 0)
        {
            player = hits[0].transform;
            return true;
        }
        player = null;
        return false;
    }

    // get a random point around a center point (usually self)
    public Vector3 RandomPoint(Vector3 center, float range)
    {
        return center + Random.insideUnitSphere * range;
    }

    // draw gizmos
    void OnDrawGizmosSelected() 
    {
        // check if need to draw gizmos
        if (!data.ShowGizmos) return;
        // draw patrol range
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, data.PatrolRange);
        // draw detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.DetectionRange);
    }
}
