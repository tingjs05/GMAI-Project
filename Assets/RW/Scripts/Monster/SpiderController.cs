using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar.Pathfinding;

[RequireComponent(typeof(SpiderData), typeof(Agent))]
public class SpiderController : MonoBehaviour, IDamagable
{
    // public properties
    public float Health { get; private set; }
    public bool Died { get; private set; }
    public bool Stunned { get; private set; }

    // components
    public SpiderData data { get; private set; }
    public Agent agent { get; private set; }
    public Animator anim { get; private set; }

    // children objects
    GameObject hitbox;
    GameObject parryIndicator;

    // coroutine
    private Coroutine coroutine;
    

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

        // set health
        Health = data.MaxHealth;
        // set booleans
        Died = false;
        Stunned = false;

        // hide children objects
        hitbox.SetActive(false);
        parryIndicator.SetActive(false);
    }

    // interface method - damage enemy
    public void Damage(float damage)
    {
        Health -= damage;
        // check if enemy has been killed
        if (Health > 0) return;
        Died = true;
    }

    // method to set stun
    public void SetStun(bool newStun)
    {
        Stunned = newStun;
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

    public bool CounterRunning()
    {
        return coroutine != null;
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
