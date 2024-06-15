using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyAttack1State : EnemyAttackState
    {
        public EnemyAttack1State(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play voiceline
            character.voiceManager.PlaySound(character.voiceManager.attack[0]);
            // start coroutine to count animation duration, then move onto next attack
            character.CountDuration(character.data.Attak1Duration * 0.8f, () => stateMachine.ChangeState(character.attack2));
        }
    }
}
