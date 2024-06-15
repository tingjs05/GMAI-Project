using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class WeaponIdleState : MeleeState
    {
        public WeaponIdleState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        
        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (triggerSheath)
            {
                stateMachine.ChangeState(character.isSheathed ? character.draw : character.sheath);
                return;
            }

            if (triggerAttack)
            {
                stateMachine.ChangeState(character.attack);
            }
        }
    }
}
