using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class Driver
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private Mesh m_Mesh;

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Driver() { }
        ~Driver() { }

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

        public Mesh GetMesh()
        {
            return m_Mesh;
        }

        public void Update(GameTime GameTime)
        {
        }

        public void Draw(GameTime gameTime, Matrix camProjMatrix, Matrix camViewMatrix)
        {
            m_Mesh.Draw(camProjMatrix, camViewMatrix);
        }
    }
}
