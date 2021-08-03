using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This character movement input class is an example of how to get input from a gamepad/joystick to control the character;
    //It comes with a dead zone threshold setting to bypass any unwanted joystick "jitter";
    public class CharacterJoystickInput : CharacterInput
    {

        public string horizontalInputAxis;
        public string verticalInputAxis;
        public KeyCode jumpKey;
        public KeyCode fireKey;

        //If this is enabled, Unity's internal input smoothing is bypassed;
        public bool useRawInput = true;

        //If any input falls below this value, it is set to '0';
        //Use this to prevent any unwanted small movements of the joysticks ("jitter");
        public float deadZoneThreshold = 0.2f;

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
            float _horizontalInput;

            if (useRawInput)
                _horizontalInput = Input.GetAxisRaw(horizontalInputAxis);
            else
                _horizontalInput = Input.GetAxis(horizontalInputAxis);

            //Set any input values below threshold to '0';
            if (Mathf.Abs(_horizontalInput) < deadZoneThreshold)
                _horizontalInput = 0f;

            return _horizontalInput;
        }

        public override float GetVerticalMovementInput()
        {
            float _verticalInput;

            if (useRawInput)
                _verticalInput = Input.GetAxisRaw(verticalInputAxis);
            else
                _verticalInput = Input.GetAxis(verticalInputAxis);

            //Set any input values below threshold to '0';
            if (Mathf.Abs(_verticalInput) < deadZoneThreshold)
                _verticalInput = 0f;

            return _verticalInput;
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
                jumpKey = KeyCode.Joystick1Button0,
                fireKey = KeyCode.Joystick1Button2,
            }},
            {Player.Player2, new Controls {
                horizontalAxis = "p2_Horizontal",
                verticalAxis = "p2_Vertical",
                jumpKey = KeyCode.Joystick2Button0,
                fireKey = KeyCode.Joystick2Button2,
            }}
        };
    }
}
