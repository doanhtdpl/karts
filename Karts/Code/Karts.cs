using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Karts.Code;
using Karts.Code.SceneManager;
using Karts.Code.SceneManager.Components;

namespace Karts
{
    public class Karts : Game
    {
        GraphicsDeviceManager graphics;
        Viewport defaultViewport;

        TextComponent fps;

        public Karts()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.SynchronizeWithVerticalRetrace = false;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new GamerServicesComponent(this));
            //Initialize components
            Components.Add(InputManager.Init(this));
            Components.Add(GameStateManager.Init(this));
            Components.Add(NetworkManager.Init(this));
            //Components.Add(PlayerManager.Init(this));
            //Components.Add(CircuitManager.Init(this));

            ControllerManager.GetInstance().parse();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            ResourcesManager resources = ResourcesManager.GetInstance();
            resources.Init(this.Content, this.graphics);

            Components.Add(Gui.Init(this));
            Components.Add(CameraManager.Init(this));

            InputManager.GetInstance().UpdateOrder = 0;
            GameStateManager.GetInstance().UpdateOrder = 10;
            NetworkManager.GetInstance().UpdateOrder = 20;
            //PlayerManager.GetInstance().UpdateOrder = 30;
            //CircuitManager.GetInstance().UpdateOrder = 35;
            CameraManager.GetInstance().UpdateOrder = 40;
            Gui.GetInstance().UpdateOrder = 50;

            GameStateManager.GetInstance().ChangeState(new MainMenu());
            //GameStateManager.GetInstance().ChangeState(new GameplayState());

            fps = new TextComponent(10, 10, "FPS:", "kartsFont");
            Gui.GetInstance().AddComponent(fps);

            defaultViewport = GraphicsDevice.Viewport;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            fps.Text = ("FPS: " + Math.Round(1000 / gameTime.ElapsedGameTime.TotalMilliseconds));
            GraphicsDevice.Viewport = defaultViewport;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        // Entry point
        static void Main(string[] args)
        {
            using (Karts game = new Karts())
            {
                game.Run();
            }
        }
    }
}
