using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class Driver : Mesh
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Driver() { }
        ~Driver() { }

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

        public void Update(GameTime GameTime)
        {
        }

        public void Draw(GameTime gameTime, Matrix camProjMatrix, Matrix camViewMatrix)
        {
            Draw(camProjMatrix, camViewMatrix);
        }
    }
}
