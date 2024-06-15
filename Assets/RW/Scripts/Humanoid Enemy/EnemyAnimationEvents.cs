using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] EnemyCharacter character;

    // methods to activate and deactivate parry window
    public void ActivateParry()
    {
        character.ParryIndicator.SetActive(true);
    }

    public void DeactivateParry()
    {
        character.ParryIndicator.SetActive(false);
    }

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
