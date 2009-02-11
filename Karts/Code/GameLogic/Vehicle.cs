using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// ----------------------------------------------------------------------------------
// This class specifies the vehicle. The vehicle contains its 3D model.
// ----------------------------------------------------------------------------------

namespace Karts.Code
{
    class Vehicle
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private Mesh m_Mesh;
        private float m_fMaxWheelRotation;

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Vehicle() 
        {
            m_fMaxWheelRotation = 0.1f;
        }

        ~Vehicle() { }

        public bool Init(Vector3 position, Vector3 rotation, float fScale, string resource_name)
        {
            bool bInitOk = false;

            m_Mesh = new Mesh();
            bInitOk = m_Mesh.Load(resource_name);

            if (bInitOk)
            {
                m_Mesh.SetPosition(position);
                m_Mesh.SetRotation(rotation);
                m_Mesh.SetScale(fScale);
            }

            return bInitOk;
        }

        public Object3D GetObject3D()
        {
            return m_Mesh;
        }

        public void SetRotation(Vector3 rot)
        {
            // we apply the wheel rotation limits
            rot.Y = MathHelper.Clamp(rot.Y, -m_fMaxWheelRotation, m_fMaxWheelRotation);
            m_Mesh.SetRotation(rot);
        }

        public void Update(GameTime GameTime)
        {
            // We correct the wheel rotation
            Vector3 rot = m_Mesh.GetRotation();
            rot.Y -= 0.01f;
            rot.Y = MathHelper.Clamp(rot.Y, 0.0f, rot.Y);
            m_Mesh.SetRotation(rot);
        }

        public void Draw(GameTime gameTime, Matrix camProjMatrix, Matrix camViewMatrix)
        {
            m_Mesh.Draw(camProjMatrix, camViewMatrix);
        }
    }
}
