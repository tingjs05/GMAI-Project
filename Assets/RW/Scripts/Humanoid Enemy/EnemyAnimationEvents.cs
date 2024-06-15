using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] EnemyCharacter character;

    // methods to be called by attack animation events
    public void Attack()
    {
        character?.DealDamage(character.data.Damage, character.transform);
    }
    
    public void ComboAttack()
    {
        character?.DealDamage(character.data.ComboDamage, transform);
    }
}
