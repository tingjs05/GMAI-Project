using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class StunState : RootMotionState
    {
        public StunState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }
    }
}
