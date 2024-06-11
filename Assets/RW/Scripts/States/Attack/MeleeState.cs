using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class MeleeState : State
    {
        protected bool triggerAttack;
        protected bool triggerSheath;

        public MeleeState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            triggerAttack = false;
            triggerSheath = false;
            character.SetAnimationBool(character.isMelee, true);
        }

        public override void HandleInput()
        {
            base.HandleInput();
            triggerAttack = Input.GetButtonDown("Fire1");
            triggerSheath = Input.GetKeyDown(KeyCode.Q) || (triggerAttack && character.isSheathed);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void Exit()
        {
            base.Exit();
            character.SetAnimationBool(character.isMelee, false);
        }
    }
}
