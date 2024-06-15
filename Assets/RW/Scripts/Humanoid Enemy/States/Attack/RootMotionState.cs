using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class RootMotionState : EnemyState
    {
        Transform model;

        public RootMotionState(EnemyCharacter character, StateMachine stateMachine) : base(character, stateMachine)
        {
            model = character.transform.GetChild(0);
        }

        public override void Enter()
        {
            base.Enter();
            // set animation to apply root motion
            character.anim.applyRootMotion = true;
        }


        public override void Exit()
        {
            base.Exit();
            // set animation to NOT apply root motion
            character.anim.applyRootMotion = false;
            // reset position and rotation
            // update position to model position
            character.transform.position += model.localPosition;
            model.localPosition = Vector3.zero;
            // update rotation to model rotation
            character.transform.Rotate(model.localEulerAngles.x, model.localEulerAngles.y, model.localEulerAngles.z);
            model.localRotation = Quaternion.identity;
        }
    }
}
