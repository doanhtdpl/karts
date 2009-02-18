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
        public override void Enter()
        {
            //Create Players
            foreach(Player player in PlayerManager.GetInstance().GetPlayers())
            {
                player.Init(new Vector3(100.0f + 100f * player.LocalPlayerIndex, 200.0f, -1000.0f), new Vector3(0.0f, MathHelper.Pi, 0.0f), 0.5f, "Ship", "Ship", player.Local);
            }

            CircuitManager.GetInstance().CreateCircuit(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), "Ground");

            base.Enter();
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float t = (float)gameTime.TotalGameTime.TotalSeconds;

            UpdateControls();
            
            PlayerManager.GetInstance().Update(gameTime);
            CircuitManager.GetInstance().Update(gameTime);
            CollisionManager.GetInstance().Update(dt, t);
        }

        public override void Draw(GameTime gameTime)
        {
            Viewport v = ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice.Viewport;
            foreach (Player p in PlayerManager.GetInstance().GetPlayers())
            {
                ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice.Viewport = p.Viewport;
                CameraManager.GetInstance().SetActiveCamera(p.m_IDCamera);
                PlayerManager.GetInstance().Draw(gameTime);
                CircuitManager.GetInstance().Draw(gameTime);
            }
            ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice.Viewport = v;
        }

        public void UpdateControls()
        {
            ControllerManager cm = ControllerManager.GetInstance();

            if (cm.isPressed("menu_cancel"))
            {
                GameStateManager.GetInstance().ChangeState(new MainMenu());
            }

            // Switch camera Free/Target
            if (cm.isPressed("toggle_free_camera"))
            {
                Camera cam = CameraManager.GetInstance().GetActiveCamera();
                Camera.ECamType type = cam.GetCameraType() == Camera.ECamType.ECAMERA_TYPE_FREE ? Camera.ECamType.ECAMERA_TYPE_TARGET : Camera.ECamType.ECAMERA_TYPE_FREE;
                cam.SetType(type);
                //CameraManager.GetInstance().ActivateCameraFree(!CameraManager.GetInstance().IsActiveCameraFree());
            }
        }

        public override void Exit()
        {
            PlayerManager.GetInstance().RemovePlayers();
            base.Exit();
        }
    }
}
