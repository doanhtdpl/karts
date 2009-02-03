using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace Karts.Code
{
    class MainMenu : GameState
    {
        private String[] OPTIONS = { "CREATE LOCAL GAME", "CREATE MULTIPLAYER GAME", "FIND MULTIPLAYER GAME", "EXIT GAME" };
        private EGameStateType[] ENTER = { EGameStateType.EGM_GAME, EGameStateType.EGM_CREATE_MULTIPLAYER_GAME, EGameStateType.EGM_FIND_MULTIPLAYER_GAME, EGameStateType.EGM_INVALID};

        private int selected = 0;

        public override EGameStateType GetStateType() { return EGameStateType.EGM_MAIN_MENU; }

        public override void Enter()
        {
            Guide.ShowSignIn(1, false);

            base.Enter();
        }

        public override void Update(GameTime GameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down)){
                selected = (selected + OPTIONS.Length + 1) % OPTIONS.Length;
            }else if (state.IsKeyDown(Keys.Up)){
                selected = (selected + OPTIONS.Length - 1) % OPTIONS.Length;
            }else if (state.IsKeyDown(Keys.Enter)){
                EGameStateType nextState = ENTER[selected];
                if(nextState == EGameStateType.EGM_INVALID){
                    Karts.karts.Exit();
                }else{
                    GameStateManager.GetInstance().ChangeState(nextState);
                }
            }

            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            Karts.spriteBatch.Begin();
            for (int i = 0; i < OPTIONS.Length; ++i)
            {
                Karts.spriteBatch.DrawString(Karts.spriteFont, OPTIONS[i], new Vector2(100, 100 + i * 100), i == selected?Color.Red:Color.Black);
            }
            Karts.spriteBatch.End();

            base.Draw(GameTime);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
