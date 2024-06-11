using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class MeleeState : State
    {
        protected bool attacked;
        protected bool sheathed;

        public MeleeState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            attacked = false;
            character.Equip(character.MeleeWeapon);
        }
        
        public override void Exit()
        {
            base.Exit();
            character.SheathWeapon();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            attacked = Input.GetButton("Fire1");
            sheathed = attacked || Input.GetKey(KeyCode.Q);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
