using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This character movement input class is an example of how to get input from a keyboard to control the character;
    public class CharacterKeyboardInput : CharacterInput
    {
        public string horizontalInputAxis;
        public string verticalInputAxis;
        public KeyCode jumpKey;

        public KeyCode fireKey;

        //If this is enabled, Unity's internal input smoothing is bypassed;
        public bool useRawInput = true;

        public override void AssignPlayerControls()
        {
            Controls controls = PlayerControlMapper[currentPlayer];
            horizontalInputAxis = controls.horizontalAxis;
            verticalInputAxis = controls.verticalAxis;
            jumpKey = controls.jumpKey;
            fireKey = controls.fireKey;
        }


        public override float GetHorizontalMovementInput()
        {
            if (useRawInput)
                return Input.GetAxisRaw(horizontalInputAxis);
            else
                return Input.GetAxis(horizontalInputAxis);
        }

        public override float GetVerticalMovementInput()
        {
            if (useRawInput)
                return Input.GetAxisRaw(verticalInputAxis);
            else
                return Input.GetAxis(verticalInputAxis);
        }

        public override bool IsJumpKeyPressed()
        {
            return Input.GetKey(jumpKey);
        }

        public override bool IsFireKeyPressed()
        {
            return Input.GetKeyDown(fireKey);
        }

        static Dictionary<Player, Controls> PlayerControlMapper = new Dictionary<Player, Controls>{
            {Player.Player1, new Controls {
                horizontalAxis = "p1_Horizontal",
                verticalAxis = "p1_Vertical",
                jumpKey = KeyCode.H,
                fireKey = KeyCode.G,
            }},
            {Player.Player2, new Controls {
                horizontalAxis = "p2_Horizontal",
                verticalAxis = "p2_Vertical",
                jumpKey = KeyCode.RightShift,
                fireKey = KeyCode.RightControl,
            }}
        };
    }
}
