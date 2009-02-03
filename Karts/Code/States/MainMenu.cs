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

        private int selected = 0;

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
                if(selected == 3){
                    Karts.karts.Exit();
                }
                else if (selected == 0)
                {
                    GameStateManager.GetInstance().ChangeState(new CreateMultiplayerGame());
                }
                else if (selected == 1)
                {
                    GameStateManager.GetInstance().ChangeState(new FindMultiplayerGame());
                }
                else if (selected == 2)
                {
                    GameStateManager.GetInstance().ChangeState(new GameplayState());
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
