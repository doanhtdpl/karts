using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace Karts.Code
{
    class GameplayState : GameState
    {
        Area a;

        public override void Enter()
        {
            //Create Players
            foreach(Player player in PlayerManager.GetInstance().GetPlayers()){
                player.Init(new Vector3(100.0f + 100f * player.LocalPlayerIndex, 200.0f, 10000.0f), new Vector3(0.0f, 0.0f, 0.0f), 0.5f, "Ship", "Ship", player.Local);
            }

            CircuitManager.GetInstance().CreateCircuit(new Vector3(0.0f, 0.0f, 1000.0f), new Vector3(0.0f, 0.0f, 0.0f), "Ground");

            a = new Area();
            a.Init(Vector3.Zero, new Vector3(0.0f, 0.0f, 0.0f), 10000, 10000, 10000);

            base.Enter();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GetInstance().isButtonPressed(0, Buttons.B) || InputManager.GetInstance().isKeyPressed(Keys.Back))
            {
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }
            else
            {

                PlayerManager.GetInstance().Update(gameTime);
                CircuitManager.GetInstance().Update(gameTime);

                Player p = PlayerManager.GetInstance().GetPlayers()[0];

                bool bCollides = a.IntersectsWith(p.GetVehicle());
                Debug.Print("The player is in the area? " + bCollides);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Player p in PlayerManager.GetInstance().GetPlayers())
            {
                ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice.Viewport = p.Viewport;
                CameraManager.GetInstance().SetActiveCamera(p.m_IDCamera);
                PlayerManager.GetInstance().Draw(gameTime);
                CircuitManager.GetInstance().Draw(gameTime);
            }

            a.Draw();
        }

        public override void Exit()
        {
            PlayerManager.GetInstance().RemovePlayers();
            base.Exit();
        }
    }
}
