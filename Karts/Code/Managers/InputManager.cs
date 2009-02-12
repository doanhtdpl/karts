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
        private GamePadState[] gamePadStates;
        private GamePadState[] previousGamePadStates;

        //Keyboard
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;

        private InputManager(Game game) : base(game)
        {
            //Gamepad
            gamePadStates = new GamePadState[4];
            previousGamePadStates = new GamePadState[4];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //GamePads
            for (int i = 0; i < 4; ++i)
            {
                previousGamePadStates[i] = gamePadStates[i];
                gamePadStates[i] = GamePad.GetState((PlayerIndex)i);
            }

            //Keyboard
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        //Keyboard
        public bool isKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public bool isKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        //Gamepads
        public bool isButtonPressed(int index, Buttons btn)
        {
            return gamePadStates[index].IsButtonDown(btn) && previousGamePadStates[index].IsButtonUp(btn);
        }

        public bool isButtonDown(int index, Buttons btn)
        {
            return gamePadStates[index].IsButtonDown(btn);
        }

        public float getAxis(int index, bool left, bool x)
        {
            GamePadState gamePadState = getGamePadState(index);
            return getAxis(gamePadState, left, x);
        }

        public float getAxis(GamePadState gamePadState, bool left, bool x)
        {
            if (x)
            {
                if (left)
                {
                    return gamePadState.ThumbSticks.Left.X;
                }
                else
                {
                    return gamePadState.ThumbSticks.Right.X;
                }
            }
            else
            {
                if (left)
                {
                    return gamePadState.ThumbSticks.Left.Y;
                }
                else
                {
                    return gamePadState.ThumbSticks.Right.Y;
                }
            }
        }

        public GamePadState getGamePadState(int index) { return gamePadStates[index]; }
        public GamePadState getPreviousGamePadState(int index) { return previousGamePadStates[index]; }

    }
}
