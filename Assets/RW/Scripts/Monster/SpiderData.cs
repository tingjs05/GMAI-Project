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
    [SerializeField] float damage = 5f;
    [SerializeField] float strongDamage = 15f;

    [Header("Ranges")]
    [SerializeField] float patrolRange = 8f;
    [SerializeField] float detectionRange = 3f;

    [Header("Durations")]
    [SerializeField] float stunDuration = 5f;

    [Header("Parry")]
    [SerializeField, Range(0f, 1f)] float normalizedStartParryWindow = 0.3f;
    [SerializeField, Range(0f, 1f)] float normalizedParryWindow = 0.25f;


    [Header("Others")]
    [SerializeField] LayerMask playerMask;
    [SerializeField] bool showGizmos = true;

    #endregion

    #region Properties

    // stats
    public float MaxHealth => maxHealth;
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float Damage => damage;
    public float StrongDamage => strongDamage;

    // ranges
    public float PatrolRange => patrolRange;
    public float DetectionRange => detectionRange;

    // durations
    public float StunDuration => stunDuration;

    // others
    public LayerMask PlayerMask => playerMask;
    public bool ShowGizmos => showGizmos;

    #endregion
}
