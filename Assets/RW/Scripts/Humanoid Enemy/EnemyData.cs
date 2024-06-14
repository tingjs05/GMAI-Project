using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [field: Header("Stats")]
    [field: SerializeField] public float MaxHealth { get; private set; } = 150f;
    [field: SerializeField] public float WalkSpeed { get; private set; } = 10f;
    [field: SerializeField] public float Damage { get; private set; } = 10f;
    [field: SerializeField] public float ComboDamage { get; private set; } = 20f;

    [field: Header("Ranges")]
    [field: SerializeField] public float DetectionRange { get; private set; } = 20f;
    [field: SerializeField] public float RangedAttackRange { get; private set; } = 10f;
    [field: SerializeField] public float MeleeAttackRange { get; private set; } = 1.5f;

    [field: Header("Equipment")]
    [field: SerializeField] public GameObject Sword { get; private set; }
    [field: SerializeField] public GameObject Bow { get; private set; }
    [field: SerializeField] public Transform LeftEquip { get; private set; }
    [field: SerializeField] public Transform RightEquip { get; private set; }

    [field: Header("Others")]
    [field: SerializeField] public LayerMask PlayerMask { get; private set; }
    public bool showGizmos = true;
}
