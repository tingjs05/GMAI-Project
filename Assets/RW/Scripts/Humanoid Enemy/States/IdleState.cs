using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class IdleState : EnemyState
    {
        public IdleState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // disallow movement
            character.agent.speed = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check if player is within detection range
            if (character.PlayerNearby(character.data.DetectionRange, out Transform player))
            {
                // change to alert state if player within range
                stateMachine.ChangeState(character.alert);
            }
        }
    }
}
