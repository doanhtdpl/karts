using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using System.Diagnostics;

namespace Karts.Code
{
    class Player : Object3D
    {
        // ------------------------------------------------
        // Class types
        // ------------------------------------------------
        public class CircuitState
        {
            public int iLaps = 0;
            public int iCheckPoint = -1;
            public float fSqCheckpointDist = 0;
        };

        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private UInt32 m_uID;
        private Vehicle m_Vehicle;
        //private Driver m_Driver;
        private string m_sName;
        public int m_IDCamera { get; set; }
        private bool m_bLife;
        public bool Local { get; set; }
        public int LocalPlayerIndex { get; set; }
        public int LocalPlayerIndexCount { get; set; }
        public Viewport Viewport { get; set; }
        private Vector3 m_vVelocity;

        private CircuitState m_CircuitState;

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Player ()
        {
            m_Vehicle = null;
            //m_Driver = null;
            m_sName = null;
            m_vPosition = Vector3.Zero;
            m_vRotation = Vector3.Zero;
            m_vVelocity = Vector3.Zero;
            m_IDCamera = CameraManager.INVALID_CAMERA_ID;

            ResetCircuitState();
        }

        ~Player ()
        {
            m_Vehicle = null;
            //m_Driver = null;
            m_sName = null;
            m_vPosition = Vector3.Zero;
            m_vRotation = Vector3.Zero;
            m_IDCamera = CameraManager.INVALID_CAMERA_ID;

            ResetCircuitState();
        }

        public void ResetCircuitState()
        {
            if (m_CircuitState == null)
            {
                m_CircuitState = new CircuitState();
            }

            m_CircuitState.fSqCheckpointDist = 0.0f;
            m_CircuitState.iCheckPoint = -1;
            m_CircuitState.iLaps = 0;
        }

        public string GetName() { return m_sName; }
        public UInt32 GetID() { return m_uID;  }

        public CircuitState GetCircuitState()
        {
            return m_CircuitState;
        }

        public void Init(string Name, bool local, bool life, int playerIndex, int playerIndexCount)
        {
            m_sName = Name;
            Local = local;
            m_bLife = life;
            LocalPlayerIndex = playerIndex;
            LocalPlayerIndexCount = playerIndexCount;
        }

        public bool Init(Vector3 position, Vector3 rotation, float fScale, string vehicle_name, string driver_name, bool bCamera)
        {
            bool bInitOk = false;

            m_vPosition = position;
            m_vRotation = rotation;

            // Load and init the vehicle model
            m_Vehicle = new Vehicle();
            bInitOk = m_Vehicle.Init(position, rotation, fScale, vehicle_name);

            if (bInitOk)
            {
                /*
                // Load and init the driver model
                m_Driver = new Driver();
                bInitOk = m_Driver.Init(driver_name);
                */
                if (bInitOk)
                {
                    if (bCamera)
                    {
                        Object3D target = m_Vehicle;
                        m_IDCamera = CameraManager.GetInstance().CreateCamera(Camera.ECamType.ECAMERA_TYPE_TARGET, true, target, target.GetPosition(), target.GetRotation());
                    }
                }
            }

            if (!bInitOk)
            {
                // Print a message error
            }

            CreateViewport();

            return bInitOk;
        }

        private void CreateViewport()
        {
            int numPlayers = PlayerManager.GetInstance().GetNumLocalPlayers();

            int width = numPlayers > 2 ? 400 : 800;
            int height = numPlayers > 1 ? 300 : 600;

            Viewport v = new Viewport();
            if (numPlayers == 1)
            {
                v.X = 0;
                v.Y = 0;
            }
            else if (numPlayers == 2)
            {
                v.X = 0;
                v.Y = LocalPlayerIndexCount == 0 ? 0 : 300;
            }
            else
            {
                v.X = LocalPlayerIndexCount % 2 == 0 ? 0 : 400;
                v.Y = LocalPlayerIndexCount < 2 ? 0 : 300;
            }

            v.Width = width;
            v.Height = height;
            Viewport = v;

            CameraManager.GetInstance().GetCamera(m_IDCamera).SetAspectRatio((float) ((float)width/(float)height) );
        }

        public void Update(GameTime gameTime)
        {
           if (CameraManager.GetInstance().IsActiveCameraFree()) 
                return;

            Vector3 newPos = new Vector3(0, 0, 0);
            float fMove = 00f;
            ControllerManager cm = ControllerManager.GetInstance();

           if (cm.isDown(LocalPlayerIndex, "accelerate"))
           {
                fMove = 100.0f;
            }

           if (cm.isDown(LocalPlayerIndex, "brake"))
           {
                fMove = -100.0f;
            }

           if (cm.isDown(LocalPlayerIndex, "turn_left"))
           {
                m_vRotation.Y += 0.03f;
            }

           if (cm.isDown(LocalPlayerIndex, "turn_right"))
           {
                m_vRotation.Y -= 0.03f;
            }

           m_vVelocity = fMove * GetForward();

           m_vPosition = m_vPosition + m_vVelocity;

            m_Vehicle.SetPosition(m_vPosition);
            m_Vehicle.SetRotationSoft(m_vRotation);
            
            m_Vehicle.Update(gameTime);
            //m_Driver.Update(gameTime);

            // We update the distances to the checkpoint
            if (m_CircuitState.iCheckPoint >= 0)
            {
                Circuit c = CircuitManager.GetInstance().GetCurrentCircuit();
                
                if (c != null)
                {
                    Debug.Print("Calculating distance for CP " + m_CircuitState.iCheckPoint);
                    CheckPoint cp = c.GetCheckpoint(m_CircuitState.iCheckPoint);
                    m_CircuitState.fSqCheckpointDist = (cp.GetPosition() - GetPosition()).LengthSquared();
                }
            }
        }

        public Vector3 GetVelocity()
        {
            return m_vVelocity;
        }

        public void Draw(GameTime gameTime)
        {
            Camera cam = CameraManager.GetInstance().GetActiveCamera();

            m_Vehicle.Draw(gameTime, cam.GetProjectionMatrix(), cam.GetViewMatrix());
            //m_Driver.Draw(gameTime, m_Camera.GetProjectionMatrix(), m_Camera.GetViewMatrix());
        }

        public Vehicle GetVehicle()
        {
            return m_Vehicle;
        }
    }
}
