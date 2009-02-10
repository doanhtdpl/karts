﻿using System;
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
        private Object3D m_Object3D;

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Driver() { }
        ~Driver() { }

        public bool Init(Vector3 position, Vector3 rotation, float fScale, string resource_name)
        {
            bool bInitOk = false;

            m_Object3D = new Object3D();
            bInitOk = m_Object3D.Load(resource_name);

            if (bInitOk)
            {
                m_Object3D.SetPosition(position);
                m_Object3D.SetRotation(rotation);
                m_Object3D.SetScale(fScale);
            }

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
