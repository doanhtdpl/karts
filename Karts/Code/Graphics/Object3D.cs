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
        private Model m_Model = null;


        //--------------------------------------------
        // Class methods
        //--------------------------------------------
        public Object3D() { }
        ~Object3D() { }

        public bool Load(string resource_name)
        {
            ContentManager content = ResourcesManager.GetInstance().GetContentManager();
            m_Model = content.Load<Model>(resource_name);

            return true;
        }

        public void Draw()
        {
            if (m_Model != null)
            {
                foreach (ModelMesh mm in m_Model.Meshes)
                {
                    mm.Draw();
                }
            }
        }
    }
}
