using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    //---------------------------------------
    //---------------------------------------
    // Class Area:
    //
    // This class will define an area trigger.
    //---------------------------------------
    //---------------------------------------
    class Area : Object3D
    {
        //---------------------------------------
        // Class members
        //---------------------------------------
        private BoundingBox m_bb;
        private List<UInt32> m_EnteredEntities = new List<UInt32>();
        private Vector3 m_vSize;

        static VertexPositionColor[] verts = new VertexPositionColor[8];
        static int[] indices = new int[]
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            0, 4,
            1, 5,
            2, 6,
            3, 7,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
        };

        static BasicEffect effect;
        static VertexDeclaration vertDecl;

        //---------------------------------------
        // Class methods
        //---------------------------------------
        public bool Init(Vector3 pos, Vector3 rot, float fWidth, float fHeight, float fDepth)
        {
            m_vPosition = pos; // Base position
            m_vRotation = rot;

            m_vSize = new Vector3(fWidth, fHeight, fDepth);

            m_bb = new BoundingBox(m_vPosition - GetRight()*fWidth*0.5f - GetForward()*fDepth*0.5f, 
                                   m_vPosition + GetRight()*fWidth*0.5f + GetForward()*fDepth*0.5f + GetUp()*fHeight);

            return true;
        }

        public bool IntersectsWith(Mesh mesh)
        {
            BoundingSphere bs = mesh.GetBoundingSphere();

            if (m_bb.Intersects(bs))
                return true;

            return false;
        }

        public bool IntersectsWith(BoundingBox bb)
        { 
            return m_bb.Intersects(bb);
        }

        public float? IntersectsWith(Ray r)
        { 
            return m_bb.Intersects(r);
        }

        // Just for Debug
        public void Draw()
        {
            GraphicsDeviceManager gdm = ResourcesManager.GetInstance().GetGraphicsDeviceManager();
            GraphicsDevice graphicsDevice = gdm.GraphicsDevice;

            if (effect == null)
            {
                effect = new BasicEffect(graphicsDevice, null);
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
                vertDecl = new VertexDeclaration(graphicsDevice, VertexPositionColor.VertexElements);
            }

            Vector3[] corners = m_bb.GetCorners();
            for (int i = 0; i < 8; i++)
            {
                verts[i].Position = corners[i];
                verts[i].Color = Color.Red;
            }

            graphicsDevice.VertexDeclaration = vertDecl;

            Camera cam = CameraManager.GetInstance().GetActiveCamera();

            effect.View = cam.GetViewMatrix();
            effect.Projection = cam.GetProjectionMatrix();

            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList,
                    verts,
                    0,
                    8,
                    indices,
                    0,
                    indices.Length / 2);

                pass.End();
            }
            effect.End();
        }
    }
}
