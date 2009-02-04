using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Karts.Code
{
    class InputManager : GameComponent
    {
        private static InputManager inputManager;

        public static InputManager GetInstance()
        {
            return inputManager;
        }

        public static InputManager Init(Game game)
        {
            if (inputManager == null)
                inputManager = new InputManager(game);

            return inputManager;
        }

        //Gamepads
        private Hashtable[] buttonPressed;
        private Hashtable[] buttonReleased;
        private GamePadState[] gamePadStates;

        //Keyboard
        private Hashtable keyPressed;
        private Hashtable keyReleased;
        private KeyboardState keyboardState;

        private InputManager(Game game) : base(game)
        {
            //Gamepad
            buttonPressed = new Hashtable[4];
            buttonReleased = new Hashtable[4];
            for (int i = 0; i < 4; ++i)
            {
                buttonPressed[i] = new Hashtable();
                buttonReleased[i] = new Hashtable();
            }
            gamePadStates = new GamePadState[4];

            //Keyboard
            keyPressed = new Hashtable();
            keyReleased = new Hashtable();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //GamePads
            for (int i = 0; i < 4; ++i)
            {
                gamePadStates[i] = GamePad.GetState((PlayerIndex)i);
                buttonPressed[i].Clear();

                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    if (gamePadStates[i].IsButtonUp(button) && !buttonReleased[i].ContainsKey(button))
                    {
                        buttonReleased[i].Add(button, true);
                    }
                }

                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    if (gamePadStates[i].IsButtonDown(button) && buttonReleased[i].ContainsKey(button))
                    {
                        buttonPressed[i].Add(button, true);
                        buttonReleased[i].Remove(button);
                    }
                }
            }

            //Keyboard
            keyboardState = Keyboard.GetState();
            keyPressed.Clear();

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (keyboardState.IsKeyUp(key) && !keyReleased.ContainsKey(key))
                {
                    keyReleased.Add(key, true);
                }
            }

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (keyboardState.IsKeyDown(key) && keyReleased.ContainsKey(key))
                {
                    keyPressed.Add(key, true);
                    keyReleased.Remove(key);
                }
            }
        }

        //Keyboard
        public bool isKeyPressed(Keys key)
        {
            return keyPressed.ContainsKey(key);
        }

        public bool isKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        //Gamepads
        public bool isButtonPressed(int index, Buttons btn)
        {
            return buttonPressed[index].ContainsKey(btn);
        }

        public bool isButtonDown(int index, Buttons btn)
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
        }
    }
}
