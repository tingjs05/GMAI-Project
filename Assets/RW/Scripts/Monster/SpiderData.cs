using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderData : MonoBehaviour
{
    #region Private Variables

    [Header("Stats")]
    [SerializeField] float maxHealth = 15f;
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] float runSpeed = 12f;

    #endregion

    #region Public Variables

    #endregion

    #region Properties

    public float MaxHealth => maxHealth;

    #endregion
}
