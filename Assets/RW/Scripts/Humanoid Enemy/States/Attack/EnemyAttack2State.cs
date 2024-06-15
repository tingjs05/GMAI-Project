using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyAttack2State : EnemyAttackState
    {
        public EnemyAttack2State(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // start coroutine to count animation duration, then move onto next attack
            character.CountDuration(character.data.Attak2Duration * 0.95f, () => stateMachine.ChangeState(character.attack3));
        }
    }
}
