using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class ShootState : EnemyState
    {
        int animationCount;

        public ShootState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // reset animation count
            animationCount = 0;
            // play shoot animation
            character.anim.SetTrigger("Shoot");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // check for end of animation
            if (character.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                animationCount++;
            // after shoot animation, return to circle state
            if (animationCount >= 2)
                stateMachine.ChangeState(character.circle);
        }
    }
}
