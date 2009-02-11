using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Karts.Code
{
    class Mesh : Object3D
    {
        private BoundingSphere m_BoundingSphere;
        private Model m_Model;
        private float m_fScale;

        public Mesh()
            : base()
        {
            m_fScale = 1.0f;
            m_Model = null;
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

            m_BoundingSphere = m_Model.Meshes[0].BoundingSphere;

            return true;
        }

        public Model GetModel()
        {
            return m_Model;
        }

        public void SetScale(float fScale)
        {
            m_fScale = fScale;
        }

        public BoundingSphere GetBoundingsphere()
        {
            return m_BoundingSphere;
        }

        public void Draw(Matrix camProjMatrix, Matrix camViewMatrix)
        {
            base.Draw();

            if (m_Model != null)
            {
                Matrix[] transforms = new Matrix[m_Model.Bones.Count];
                m_Model.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in m_Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.World = transforms[mesh.ParentBone.Index] *
                                       Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z) * // Rotation matrix
                                       Matrix.CreateScale(m_fScale) * Matrix.CreateTranslation(m_vPosition); // Translation and scale matrix

                        // Use the matrices provided by the chase camera
                        effect.View = camViewMatrix;
                        effect.Projection = camProjMatrix;
                    }
                    mesh.Draw();
                }
            }
        }

        //---------------------------------------
        // Collision methods
        //---------------------------------------
        public bool CollidesWithMesh(Mesh m)
        {
            if (m_Model == null) 
                return false;

            for (int i = 0; i < m_Model.Meshes.Count; i++)
            {
                // Check whether the bounding boxes of the two cubes intersect.
                BoundingSphere c1BoundingSphere = m_Model.Meshes[i].BoundingSphere;
                c1BoundingSphere.Center += GetPosition();

                for (int j = 0; j < m.GetModel().Meshes.Count; j++)
                {
                    BoundingSphere c2BoundingSphere = m.GetModel().Meshes[j].BoundingSphere;
                    c2BoundingSphere.Center += m.GetPosition();

                    if (c1BoundingSphere.Intersects(c2BoundingSphere))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
