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
    class Vehicle : Mesh
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private float m_fMaxWheelRotation;

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Vehicle() : base()
        {
            m_fMaxWheelRotation = 0.1f;
        }

        ~Vehicle() { }

        public bool Init(Vector3 position, Vector3 rotation, float fScale, string resource_name)
        {
            bool bInitOk = false;

            bInitOk = Load(resource_name);

            if (bInitOk)
            {
                SetPosition(position);
                SetRotation(rotation);
                SetScale(fScale);
            }

            return bInitOk;
        }

        public void SetRotationSoft(Vector3 rot)
        {
            // we apply the wheel rotation limits
            Vector3 new_rot = Vector3.Lerp(GetRotation(), rot, 0.1f);

            //rot.Y = MathHelper.Clamp(rot.Y, -m_fMaxWheelRotation, m_fMaxWheelRotation);
            SetRotation(new_rot);
        }

        public void Update(GameTime GameTime)
        {
            // We correct the wheel rotation
            /*
            Vector3 rot = m_Mesh.GetRotation();
            rot.Y -= 0.01f;
            rot.Y = MathHelper.Clamp(rot.Y, 0.0f, rot.Y);
            m_Mesh.SetRotation(rot);*/
        }

        public void Draw(GameTime gameTime, Matrix camProjMatrix, Matrix camViewMatrix)
        {
            Draw(camProjMatrix, camViewMatrix);
        }
    }
}
