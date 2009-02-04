using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    class ResourcesManager
    {
        //--------------------------------------------
        // Class members
        //--------------------------------------------
        public static ResourcesManager m_ResourcesManager = null;

        private ContentManager m_ContentManager;
        private GraphicsDeviceManager m_GraphicsDevice;

        //--------------------------------------------
        // Class methods
        //--------------------------------------------
        public ResourcesManager() { }
        ~ResourcesManager(){}

        public static ResourcesManager GetInstance()
        {
            if (m_ResourcesManager == null)
                m_ResourcesManager = new ResourcesManager();

            return m_ResourcesManager;
        }

        public ContentManager GetContentManager()
        {
            return m_ContentManager;
        }

        public GraphicsDeviceManager GetGraphicsDeviceManager()
        {
            return m_GraphicsDevice;
        }

        public bool Init(ContentManager content, GraphicsDeviceManager graphics)
        {
            bool bInitOk = content != null && graphics != null;

            if (bInitOk)
            {
                m_ContentManager = content;
                m_GraphicsDevice = graphics;
            }

            return bInitOk;
        }
    }
}
