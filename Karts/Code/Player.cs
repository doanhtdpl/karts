using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;



namespace Karts.Code
{
    class Player
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private Vehicle m_Vehicle;
        private Driver m_Driver;
        private UInt32 m_uID;
        private String m_sName;


        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Player ()
        {
            m_uID = 0;
            m_Vehicle = null;
            m_Driver = null;
            m_sName = null;
        }

        ~Player (){}

        public UInt32 GetID() { return m_uID; }
        public String GetName() { return m_sName; }

        public void Update(GameTime GameTime)
        {
            m_Vehicle.Update(GameTime);
            m_Driver.Update(GameTime);
        }

        public void Draw(GameTime GameTime)
        {
            m_Vehicle.Draw(GameTime);
            m_Driver.Draw(GameTime);
        }
    }
}
