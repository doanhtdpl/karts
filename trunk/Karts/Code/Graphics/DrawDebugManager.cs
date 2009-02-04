using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class DrawDebugManager
    {
        //------------------------------------------
        // Class members
        //------------------------------------------
        private static DrawDebugManager m_DrawDebugManager = null;


        //------------------------------------------
        // Class methods
        //------------------------------------------
        public DrawDebugManager() { }
        ~DrawDebugManager() { }

        public static DrawDebugManager GetInstance()
        {
            if (m_DrawDebugManager == null)
            {
                m_DrawDebugManager = new DrawDebugManager();
            }

            return m_DrawDebugManager;
        }


        public void DrawLine(Vector3 or, Vector3 end, Color c)
        {
            BasicEffect effect;
            GraphicsDeviceManager gdm = ResourcesManager.GetInstance().GetGraphicsDeviceManager();

            VertexPositionColor[] v = new VertexPositionColor[2];
            v[0] = new VertexPositionColor(or, c);
            v[1] = new VertexPositionColor(end, c);

            effect = new BasicEffect(gdm.GraphicsDevice, null);

            Camera cam = CameraManager.GetInstance().GetActiveCamera();
            effect.View = cam.GetViewMatrix();
            effect.Projection = cam.GetProjectionMatrix();
            effect.VertexColorEnabled = true;

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                gdm.GraphicsDevice.VertexDeclaration = new VertexDeclaration(gdm.GraphicsDevice, VertexPositionColor.VertexElements);
                gdm.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, v, 0, 1);
                pass.End();
            }
            effect.End();
        }

        public void DrawPoint(Vector3 or, float fSize, Color c)
        {
            BasicEffect effect;
            GraphicsDeviceManager gdm = ResourcesManager.GetInstance().GetGraphicsDeviceManager();

            VertexPositionColor[] v = new VertexPositionColor[1];
            v[0] = new VertexPositionColor(or, c);

            gdm.GraphicsDevice.RenderState.PointSize = fSize;

            effect = new BasicEffect(gdm.GraphicsDevice, null);
            //effect.View = Matrix.CreateLookAt(new Vector3(0, 0, 3), Vector3.Zero, Vector3.Up);
            //effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)gdm.GraphicsDevice.Viewport.Width / (float)gdm.GraphicsDevice.Viewport.Height, 1.0f, 100.0f);
            effect.VertexColorEnabled = true;

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                gdm.GraphicsDevice.VertexDeclaration = new VertexDeclaration(gdm.GraphicsDevice, VertexPositionColor.VertexElements);
                gdm.GraphicsDevice.DrawUserPrimitives(PrimitiveType.PointList, v, 0, 1);
                pass.End();
            }
            effect.End();
        }

        public void DrawSphere(Vector3 center, float fRad, Color c)
        {
        }
    }
}
