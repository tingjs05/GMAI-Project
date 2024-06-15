using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class DeathState : EnemyState
    {
        public DeathState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // set movement to 0
            character.agent.speed = 0f;
            // play death animation
            character.anim.SetTrigger("Die");
            // count death animation duration
            character.CountDuration(character.data.DeathAnimDuration, () => character.Die());
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // ensure is grounded
            character.transform.position = new Vector3(character.position.x, 0f, character.position.z);
        }
    }
}
