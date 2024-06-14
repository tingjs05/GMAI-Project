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
    GameObject[] weapons = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        CreateWeapons();
        Equip(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Handle Weapon
    void CreateWeapons()
    {
        // instatiate weapons
        weapons[0] = Instantiate(bow, leftEquip);
        weapons[1] = Instantiate(sword, leftEquip);
        weapons[2] = Instantiate(sword, rightEquip);
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
}
