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
        private Object3D m_Object3D;

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Vehicle() { }
        ~Vehicle() { }

        public bool Init(string resource_name)
        {
            bool bInitOk = false;

            m_Object3D = new Object3D();
            bInitOk = m_Object3D.Load(resource_name);

            return bInitOk;
        }

        public Object3D GetObject3D()
        {
            return m_Object3D;
        }

        public void Update(GameTime GameTime)
        {
        }

        public void Draw(GameTime gameTime, Matrix camProjMatrix, Matrix camViewMatrix)
        {
            m_Object3D.Draw(camProjMatrix, camViewMatrix);
        }
    }
}
