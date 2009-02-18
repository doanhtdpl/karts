using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

namespace Karts.Code
{
    class DrawDebugManager
    {
        //------------------------------------------
        // Class members
        //------------------------------------------
        private static DrawDebugManager m_DrawDebugManager = null;
        private Mesh m_Sphere = null;

        private const int NUM_SPHERE_AXIS = 3;
        private const int BSPHERE_RESOLUTION = 30;
        private const int MAX_BSPHERE_COUNT = 2500;
        private const int NUM_BSPHERE_VERT = (BSPHERE_RESOLUTION + 1) * NUM_SPHERE_AXIS;
        private const float BSPHERE_STEP = MathHelper.TwoPi / (float)BSPHERE_RESOLUTION;


        VertexPositionColor[] BSphereVerts = new VertexPositionColor[NUM_BSPHERE_VERT];


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

        public void DrawSphere(Vector3 c, float rad, Color color)
        {
#if DEBUG
            Matrix sphereMatrix = Matrix.CreateScale(rad) * Matrix.CreateTranslation(c);
            int iCountVerts = 0;

            //create the loop on the XY plane first
            for (float a = 0f; a <= MathHelper.TwoPi; a += BSPHERE_STEP)
            {
                Vector3 position =
                    new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f);
                position = Vector3.Transform(position, sphereMatrix);

                BSphereVerts[iCountVerts++] = new VertexPositionColor(position, color);
            }

            //next on the XZ plane
            for (float a = 0f; a <= MathHelper.TwoPi; a += BSPHERE_STEP)
            {
                Vector3 position =
                    new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a));
                position = Vector3.Transform(position, sphereMatrix);

                BSphereVerts[iCountVerts++] = new VertexPositionColor(position, color);
            }

            //finally on the YZ plane
            for (float a = 0f; a <= MathHelper.TwoPi; a += BSPHERE_STEP)
            {
                Vector3 position =
                    new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a));
                position = Vector3.Transform(position, sphereMatrix);

                BSphereVerts[iCountVerts++] = new VertexPositionColor(position, color);
            }

            BasicEffect effect;
            GraphicsDeviceManager gdm = ResourcesManager.GetInstance().GetGraphicsDeviceManager();
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
                gdm.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, BSphereVerts, 0, iCountVerts / 2);
                pass.End();
            }
            effect.End();
#endif
        }

        public void DrawLine(Vector3 or, Vector3 end, Color c)
        {
#if DEBUG
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
#endif
        }

        public void DrawPoint(Vector3 or, float fSize, Color c)
        {
#if DEBUG
            BasicEffect effect;
            GraphicsDeviceManager gdm = ResourcesManager.GetInstance().GetGraphicsDeviceManager();

            VertexPositionColor[] v = new VertexPositionColor[1];
            v[0] = new VertexPositionColor(or, c);

            gdm.GraphicsDevice.RenderState.PointSize = fSize;

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
                gdm.GraphicsDevice.DrawUserPrimitives(PrimitiveType.PointList, v, 0, 1);
                pass.End();
            }
            effect.End();
#endif
        }
    }
}
