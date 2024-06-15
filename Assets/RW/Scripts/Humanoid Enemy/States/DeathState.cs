using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class DeathState : EnemyState
    {
        Rigidbody rb;
        Collider collider;

        public DeathState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
            // get components
            rb = character.GetComponent<Rigidbody>();
            collider = character.GetComponent<Collider>();
        }

        public override void Enter()
        {
            base.Enter();
            // unequip weapons
            character.Unequip();
            // set movement to 0
            character.agent.speed = 0f;
            // set to iskinematic
            rb.isKinematic = true;
            // disable collider
            collider.enabled = false;
            // play death animation
            character.anim.SetTrigger("Die");
            // play voiceline
            character.voiceManager.PlaySound(character.voiceManager.death);
            // count death animation duration
            character.StartCoroutine(character.Die());
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // ensure is grounded
            character.transform.position = new Vector3(character.position.x, -1.5f, character.position.z);
            character.transform.rotation = Quaternion.Euler(0f, character.transform.eulerAngles.y, 0f);
        }
    }
}
