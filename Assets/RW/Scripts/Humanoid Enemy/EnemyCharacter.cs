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

    // position relative to height
    public Vector3 position 
    {
        get { return transform.position + (Vector3.up * height); }
        set { transform.position = value - (Vector3.up * height); }
    }

    #endregion

    #region FSM
    StateMachine fsm;
    #endregion

    // private variables
    GameObject[] weapons = new GameObject[3];

    #region MonoBehaviour Callbacks
    // Start is called before the first frame update
    void Start()
    {
        // get components
        data = GetComponent<EnemyData>();
        agent = GetComponent<Agent>();

        // set up weapon
        // CreateWeapons();
        // Equip(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // apply gravity
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
