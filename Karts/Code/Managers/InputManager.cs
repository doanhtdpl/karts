using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class InputManager
    {
        private static InputManager inputManager;

        public static InputManager GetInstance()
        {
            if (inputManager == null)
            {
                inputManager = new InputManager();
            }
            return inputManager;
        }

        private Hashtable[] inputPressed;
        private Hashtable[] inputReleased;
        private GamePadState[] gamePadStates;

        private InputManager()
        {
            inputPressed = new Hashtable[4];
            inputReleased = new Hashtable[4];
            for (int i = 0; i < 4; ++i)
            {
                inputPressed[i] = new Hashtable();
                inputReleased[i] = new Hashtable();
            }
            gamePadStates = new GamePadState[4];
        }

        public void Update()
        {
            for (int i = 0; i < 4; ++i)
            {
                gamePadStates[i] = GamePad.GetState((PlayerIndex)i);
                inputPressed[i].Clear();

                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    if (gamePadStates[i].IsButtonUp(button) && !inputReleased[i].ContainsKey(button))
                    {
                        inputReleased[i].Add(button, true);
                    }
                }

                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    if (gamePadStates[i].IsButtonDown(button) && inputReleased[i].ContainsKey(button))
                    {
                        inputPressed[i].Add(button, true);
                        inputReleased[i].Remove(button);
                    }
                }
            }
        }

        public bool isInputPressed(int index, Buttons btn)
        {
            return inputPressed[index].ContainsKey(btn);
        }

        public bool isInputDown(int index, Buttons btn)
        {
            return gamePadStates[index].IsButtonDown(btn);
        }

        public float getAxisX(int index, bool left)
        {
            if(left){
                return gamePadStates[index].ThumbSticks.Left.X;
            }else{
                return gamePadStates[index].ThumbSticks.Right.X;
            }
            //return 0;
        }

        public float getAxisY(int index, bool left)
        {
            if (left)
            {
                return gamePadStates[index].ThumbSticks.Left.Y;
            }
            else
            {
                return gamePadStates[index].ThumbSticks.Right.Y;
            }
            //return 0;
        }
    }
}
