using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class EnemyState : BaseState
    {
        protected EnemyCharacter character;

        public EnemyState(EnemyCharacter character, StateMachine stateMachine)
        {
            this.character = character;
            base.stateMachine = stateMachine;
        }
    }
}
