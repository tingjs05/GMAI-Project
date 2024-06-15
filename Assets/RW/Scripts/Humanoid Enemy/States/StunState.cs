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

        public override void Enter()
        {
            base.Enter();
            // unequip weapons
            character.Unequip();
            // trigger hit animation
            character.anim.SetTrigger("Hit");
            // play voiceline
            character.voiceManager.PlaySound(character.voiceManager.stun);
            // play stun sound effect
            SoundManager.Instance?.PlaySound(SoundManager.Instance.shieldBreak);
            // start stun duration count
            character.CountDuration(character.data.StunDuration, () => stateMachine.ChangeState(character.idle));
        }

        public override void Exit()
        {
            base.Exit();
            // return to idle animation
            character.anim.Play("Idle");
        }
    }
}
