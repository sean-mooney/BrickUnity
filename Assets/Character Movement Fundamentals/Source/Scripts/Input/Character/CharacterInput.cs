using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This abstract character input class serves as a base for all other character input classes;
    //The 'Controller' component will access this script at runtime to get input for the character's movement (and jumping);
    //By extending this class, it is possible to implement custom character input;
    public abstract class CharacterInput : MonoBehaviour
    {


        public Player currentPlayer = Player.Player1;

        public void Start()
        {
            AssignPlayerControls();
        }

        public abstract void AssignPlayerControls();
        public abstract float GetHorizontalMovementInput();
        public abstract float GetVerticalMovementInput();

        public abstract bool IsJumpKeyPressed();
        public abstract bool IsFireKeyPressed();


    }

    class Controls
    {
        public string horizontalAxis;
        public string verticalAxis;
        public KeyCode jumpKey;
        public KeyCode fireKey;
    }

    public enum Player
    {
        Player1,
        Player2
    }
}
