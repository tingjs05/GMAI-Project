using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Astar.Pathfinding;
using RayWenderlich.Unity.StatePatternInUnity;
using UnityEngine.UIElements;

[RequireComponent(typeof(EnemyData), typeof(Agent))]
public class EnemyCharacter : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] float height = 1.7f;
    #endregion

    #region Properties
    // components
    public EnemyData data { get; private set; }
    public Agent agent { get; private set; }
    public Animator anim { get; private set; }

    // position relative to height
    public Vector3 position 
    {
        get { return transform.position + (Vector3.up * height); }
        set { transform.position = value - (Vector3.up * height); }
    }

    #endregion

    #region Duration Management
    // manage shooting cooldown
    public bool CanShoot { get; private set; } = true;
    [HideInInspector] public int shotsFired = 0;
    [HideInInspector] public Coroutine shootCoroutine;

    // manage animation durations
    Coroutine coroutine;
    #endregion

    #region FSM
    // state machine
    StateMachine fsm;
    // states
    public IdleState idle { get; private set; }
    public AlertState alert { get; private set; }
    public CircleState circle { get; private set; }
    public ShootState shoot { get; private set; }
    #endregion

    // private variables
    GameObject[] weapons = new GameObject[3];

    #region MonoBehaviour Callbacks
    void Awake()
    {
        // initialize fsm
        fsm = new StateMachine();
        // initialize states
        idle = new IdleState(this, fsm);
        alert = new AlertState(this, fsm);
        circle = new CircleState(this, fsm);
        shoot = new ShootState(this, fsm);
    }

    // Start is called before the first frame update
    void Start()
    {
        // get components
        data = GetComponent<EnemyData>();
        agent = GetComponent<Agent>();
        anim = GetComponentInChildren<Animator>();
        // create weapons
        CreateWeapons();
        // initialize fsm
        fsm.Initialize(idle);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }

    void FixedUpdate()
    {
        fsm.FixedUpdate();
    }
    #endregion

    #region Handle Weapon
    void CreateWeapons()
    {
        // instatiate weapons
        weapons[0] = Instantiate(data.Bow, data.LeftEquip);
        weapons[1] = Instantiate(data.Sword, data.LeftEquip);
        weapons[2] = Instantiate(data.Sword, data.RightEquip);
        // unequip weapons first
        Unequip();
    }

    public void Unequip()
    {
        foreach (GameObject obj in weapons)
        {
            obj.SetActive(false);
        }
    }

    public void Equip(int weapon)
    {
        // unequip all weapons first
        Unequip();
        // equip weapon
        switch (weapon)
        {
            case 0:
                weapons[1].SetActive(true);
                weapons[2].SetActive(true);
                break;
            case 1:
                weapons[0].SetActive(true);
                break;
            default:
                Debug.LogWarning("Weapon " + weapon + " cannot be found! ");
                break;
        }
    }

    public void ShootArrow()
    {
        // create arrow object
        GameObject arrow = Instantiate(
                data.Arrow, 
                position + (transform.forward * 2.5f), 
                Quaternion.Euler(transform.rotation.y, transform.rotation.x, transform.rotation.z)
            );
        // set arrow damage
        arrow.GetComponent<HitBox>()?.SetDamage(data.ArrowDamage);
    }
    #endregion

    #region Other Public Methods
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

    // coroutines to count duration of an action
    public void CountDuration(float duration, System.Action callback = null)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(CountDurationCoroutine(duration, callback));
    }

    IEnumerator CountDurationCoroutine(float duration, System.Action callback = null)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
        coroutine = null;
    }
    #endregion

    #region Gizmos
    void OnDrawGizmosSelected()
    {
        // ensure data is not null
        if (data == null) data = GetComponent<EnemyData>();
        // check if need to show gizmos
        if (!data.showGizmos) return;
        // show attack ranges
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(position, data.DetectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, data.RangedAttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, data.MeleeAttackRange);
    }
    #endregion
}
