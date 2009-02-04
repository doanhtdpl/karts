using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code.SceneManager
{
    class SceneManager : DrawableGameComponent
    {
        private static SceneManager sceneManager;

        public static SceneManager Init(Game game)
        {
            if (sceneManager == null)
                sceneManager = new SceneManager(game);
            return sceneManager;
        }

        public static SceneManager GetInstance()
        {
            return sceneManager;
        }

        private SceneManager(Game game) : base(game)
        {
            this.DrawOrder = 100;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
