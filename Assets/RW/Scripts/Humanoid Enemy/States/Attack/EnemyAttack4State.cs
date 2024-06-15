using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyAttack4State : EnemyAttackState
    {
        public EnemyAttack4State(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // play voiceline
            character.voiceManager.PlaySound(character.voiceManager.attack[3]);
            // start coroutine to count animation duration, then return to idle state
            character.CountDuration(character.data.Attak4Duration, () => stateMachine.ChangeState(character.idle));
        }
    }
}
