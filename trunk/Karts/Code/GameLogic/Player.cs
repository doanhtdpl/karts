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
        private Camera m_Camera;


        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Player ()
        {
            m_uID = 0;
            m_Vehicle = null;
            m_Driver = null;
            m_sName = null;
            m_Camera = null;
        }

        ~Player ()
        {
            m_uID = 0;
            m_Vehicle = null;
            m_Driver = null;
            m_sName = null;
            m_Camera = null;
        }

        public UInt32 GetID() { return m_uID; }
        public string GetName() { return m_sName; }

        public bool Init(string Name, UInt32 uID, string vehicle_name, string driver_name)
        {
            bool bInitOk = false;

            m_uID = uID;
            m_sName = Name;

            // Load and init the vehicle model
            m_Vehicle = new Vehicle();
            bInitOk = m_Vehicle.Init(vehicle_name);

            if (bInitOk)
            {
                m_Vehicle.GetObject3D().SetPosition(0, 0, 0);

                // Load and init the driver model
                m_Driver = new Driver();
                bInitOk = m_Driver.Init(driver_name);

                if (bInitOk)
                {
                    m_Camera = new Camera();
                    m_Camera.SetTarget(this.m_Vehicle.GetObject3D());
                }
            }

            if (!bInitOk)
            {
                // Print a message error
            }

            return bInitOk;
        }

        public void Update(GameTime gameTime)
        {
            m_Camera.Update(gameTime);
            m_Vehicle.Update(gameTime);
            m_Driver.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            m_Vehicle.Draw(gameTime, m_Camera.GetProjectionMatrix(), m_Camera.GetViewMatrix());
            m_Driver.Draw(gameTime, m_Camera.GetProjectionMatrix(), m_Camera.GetViewMatrix());
        }
    }
}
