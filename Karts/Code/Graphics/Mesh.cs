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

            return true;
        }

        public void SetScale(float fScale)
        {
            m_fScale = fScale;
        }

        public new void Draw(Matrix camProjMatrix, Matrix camViewMatrix)
        {
            base.Draw(camProjMatrix, camViewMatrix);

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
    }
}
