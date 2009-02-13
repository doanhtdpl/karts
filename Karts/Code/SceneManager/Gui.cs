using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Karts.Code.SceneManager.Components;
using Karts.Code.SceneManager.Effects;

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
            effects = new List<GuiEffect>();
            screens = new List<Component>();
            spriteBatch = new SpriteBatch(ResourcesManager.GetInstance().GetGraphicsDeviceManager().GraphicsDevice);
        }

        private List<GuiEffect> effects;
        private List<Component> screens;
        private SpriteBatch spriteBatch;

        public override void Update(GameTime gameTime)
        {
            foreach (GuiEffect effect in effects)
                effect.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

            foreach (Component screen in screens)
                screen.Draw(Vector2.Zero, Vector2.One);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void AddComponent(Component comp)
        {
            screens.Add(comp);
        }

        public void RemoveComponent(Component comp)
        {
            screens.Remove(comp);
        }

        public void AddEffect(GuiEffect effect)
        {
            effects.Add(effect);
        }

        public void RemoveEffect(GuiEffect effect)
        {
            effects.Remove(effect);
        }

        public SpriteBatch GetSpriteBatch() { return spriteBatch; }
    }
}
