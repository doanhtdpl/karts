using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Karts.Code.SceneManager.Components;

namespace Karts.Code.SceneManager
{
    class Gui : DrawableGameComponent
    {
        # region Singleton 

        private static Gui sceneManager;

        public static Gui Init(Game game)
        {
            if (sceneManager == null)
                sceneManager = new Gui(game);

            return sceneManager;
        }

        public static Gui GetInstance()
        {
            return sceneManager;
        }

        # endregion

        private Gui(Game game)
            : base(game)
        {
            screens = new List<Screen>();
            spriteBatch = new SpriteBatch(ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice);
        }

        private List<Screen> screens;
        private SpriteBatch spriteBatch;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (Screen screen in screens)
                screen.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void AddComponent(Screen comp)
        {
            screens.Add(comp);
        }

        public void RemoveComponent(Screen comp)
        {
            screens.Remove(comp);
        }

        public SpriteBatch GetSpriteBatch() { return spriteBatch; }
    }
}
