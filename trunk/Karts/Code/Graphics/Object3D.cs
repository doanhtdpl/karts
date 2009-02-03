using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Karts.Code
{
    class Object3D
    {
        //--------------------------------------------
        // Class members
        //--------------------------------------------
        private Model m_Model;
        private float m_fScale;
        private Vector3 m_vPosition;
        private Vector3 m_vRotation;


        //--------------------------------------------
        // Class methods
        //--------------------------------------------
        public Object3D() 
        {
            m_fScale = 1.0f;
            m_Model = null;
            m_vPosition = Vector3.Zero;
            m_vRotation = Vector3.Zero;
        }

        ~Object3D() { }

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

        public Vector3 GetPosition()
        {
            return m_vPosition;
        }

        public void SetPosition(float x, float y, float z)
        {
            m_vPosition.X = x;
            m_vPosition.Y = y;
            m_vPosition.Z = z;
        }

        public void SetPosition(Vector3 pos)
        {
            m_vPosition = pos;
        }

        public Vector3 GetForward()
        {
            Matrix m = Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z);
            
            Vector3 vDir = m.Forward;
            vDir.Normalize();
            return vDir;
        }

        public Vector3 GetRight()
        {
            Matrix m = Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z);
            Vector3 vRight = m.Right;
            vRight.Normalize();
            return vRight;
        }

        public Vector3 GetUp()
        {
            Matrix m = Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z);
            Vector3 vUp = m.Up;
            vUp.Normalize();
            return vUp;
        }

        public void SetRotation(float fYaw, float fPitch, float fRoll)
        {
            m_vRotation.X = fYaw;
            m_vRotation.X = fPitch;
            m_vRotation.X = fRoll;
        }

        public void Draw(Matrix camProjMatrix, Matrix camViewMatrix)
        {
            if (m_Model != null)
            {
                foreach (ModelMesh mesh in m_Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.World = Matrix.CreateFromYawPitchRoll(m_vRotation.Y, m_vRotation.X, m_vRotation.Z) *
                                       Matrix.CreateScale(m_fScale) * Matrix.CreateTranslation(m_vPosition);

                        effect.Projection = camProjMatrix;
                        effect.View = camViewMatrix;
                    }

                    mesh.Draw();
                }
            }
        }
    }
}
