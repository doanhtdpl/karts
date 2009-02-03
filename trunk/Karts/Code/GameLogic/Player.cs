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
        private string m_sName;


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

        ~Player ()
        {
            m_uID = 0;
            m_Vehicle = null;
            m_Driver = null;
            m_sName = null;
        }

        public UInt32 GetID() { return m_uID; }
        public string GetName() { return m_sName; }

        public bool Init(string Name, string vehicle_name, string driver_name)
        {
            bool bInitOk = false;

            // Generate the unique ID
            m_uID = 1;
            m_sName = Name;

            // Load and init the vehicle model
            m_Vehicle = new Vehicle();
            bInitOk = m_Vehicle.Init(vehicle_name);

            if (bInitOk)
            {
                // Load and init the driver model
                m_Driver = new Driver();
                bInitOk = m_Driver.Init(driver_name);
            }

            if (!bInitOk)
            {
                // Print a message error
            }

            return bInitOk;
        }

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
