using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Karts.Code
{
    class ResourcesManager
    {
        //--------------------------------------------
        // Class members
        //--------------------------------------------
        public static ResourcesManager m_ResourcesManager = null;

        private ContentManager m_ContentManager;

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

        public bool Init(ContentManager content)
        {
            bool bInitOk = content != null;

            if (bInitOk)
            {
                m_ContentManager = content;
            }

            return bInitOk;
        }
    }
}
