using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Diagnostics;

namespace Karts.Code
{
    class Mesh : Object3D
    {
        private Model m_Model;
        private float m_fScale;

        public Mesh()
            : base()
        {
            m_fScale = 1.0f;
            m_Model = null;
        }

        ~Mesh()
        {
        }

        public bool Load(string resource_name)
        {
            ContentManager content = ResourcesManager.GetInstance().GetContentManager();

            try
            {
                m_Model = content.Load<Model>(resource_name);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Model GetModel()
        {
            return m_Model;
        }

        public float GetScale()
        {
            return m_fScale;
        }

        public void SetScale(float fScale)
        {
            m_fScale = fScale;
        }

        public BoundingSphere GetBoundingSphere()
        {
            BoundingSphere bs = m_Model.Meshes[0].BoundingSphere;
            bs.Radius *= m_fScale;
            bs.Center = Vector3.Transform(bs.Center, GetRotationMatrix());
            bs.Center += m_vPosition;

            return bs;
        }

        public void Draw(Matrix camProjMatrix, Matrix camViewMatrix)
        {
            base.Draw();

            if (m_Model != null)
            {
                foreach (ModelMesh mesh in m_Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.World = Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z) * // Rotation matrix
                                       Matrix.CreateScale(m_fScale) * Matrix.CreateTranslation(m_vPosition); // Translation and scale matrix

                        // Use the matrices provided by the chase camera
                        effect.View = camViewMatrix;
                        effect.Projection = camProjMatrix;
                    }
                    mesh.Draw();
                }
            }
        }

        public void DrawBoundingSphere()
        {
            BoundingSphere bs = GetBoundingSphere();
            DrawDebugManager.GetInstance().DrawSphere(bs.Center, bs.Radius, Color.Yellow);
        }


        //---------------------------------------
        // Collision methods
        //---------------------------------------
        public bool CollidesWithMesh(Mesh m)
        {
            if (m_Model == null) 
                return false;

            // Check whether the bounding boxes of the two cubes intersect.
            BoundingSphere bs1 = GetBoundingSphere();
            BoundingSphere bs2 = m.GetBoundingSphere();

            if (bs1.Intersects(bs2))
                return true;
            
            return false;
        }
    }
}
