using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyAttack3State : EnemyAttackState
    {
        public EnemyAttack3State(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // start coroutine to count animation duration, then move onto next attack
            character.CountDuration(character.data.Attak3Duration * 0.95f, () => stateMachine.ChangeState(character.attack4));
        }
    }
}
