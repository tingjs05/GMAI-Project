using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class BaseState
    {
        protected StateMachine stateMachine;

        protected void DisplayOnUI(UIManager.Alignment alignment)
        {
            UIManager.Instance.Display(this, alignment);
        }

        // virtual methods to be overridden by children
        public virtual void Enter() {}
        public virtual void HandleInput() {}
        public virtual void LogicUpdate() {}
        public virtual void PhysicsUpdate() {}
        public virtual void Exit() {}
    }
}
