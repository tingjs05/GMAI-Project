using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayWenderlich.Unity.StatePatternInUnity;

public class EnemyCharacter : MonoBehaviour
{
    [Header("Equipment")]
    [SerializeField] GameObject sword;
    [SerializeField] GameObject bow;
    [SerializeField] Transform leftEquip, rightEquip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Equip(GameObject weapon)
    {
        // ensure weapon is sword or bow
        if (weapon != sword || weapon != bow) return;
        // destroy all child objects in hand
        foreach(Transform child in leftEquip)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in rightEquip)
        {
            Destroy(child.gameObject);
        }
        // equip weapons
        Instantiate(weapon, rightEquip);
        // only equip on left hand if its the sword
        if (weapon == bow) return;
        Instantiate(weapon, leftEquip);
    }
}
