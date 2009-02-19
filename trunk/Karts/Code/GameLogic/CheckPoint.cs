using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Karts.Code
{
    // ----------------------------------------------------------------
    // ----------------------------------------------------------------
    // Class CheckPoint
    // 
    // The CheckPoint will define the direction of the circuit and the
    // start/end point of it. In case we have a time attack mode the
    // checkpoints will manage the timing. Also the checkpoints can be 
    // visible or not. We have to remember that the circuits will be 
    // able to be raced in reverse mode.
    // ----------------------------------------------------------------
    // ----------------------------------------------------------------
    class CheckPoint : Mesh, IObserver
    {
        //----------------------------------
        // Class members
        //----------------------------------
        private Area m_Area = null;
        private Circuit m_Circuit = null;
        private List<Player> m_PlayersRanking = new List<Player>(); // Ranking of the players
        private int m_iIndex;
        private Vector3 m_vDirection; // This will be used to determine the direction of the player.

        //----------------------------------
        // Class methods
        //----------------------------------
        public CheckPoint () : base()
        {
        }

        ~CheckPoint()
        {
            m_Area.UnregisterObserver(this);
        }

        public bool Init(Circuit c, string model_name, Vector3 pos, Vector3 rot, bool reversed, int iIndex)
        {
            bool bInitOk = true;

            if (model_name != null)
            {
                bInitOk = Load(model_name);
            }

            if (bInitOk)
            {
                m_vPosition = pos;
                m_vRotation = rot;
                m_iIndex = iIndex;
                m_Circuit = c;

                m_vDirection = reversed ? -1 * GetForward() : GetForward();

                m_Area = CollisionManager.GetInstance().CreateArea(pos, rot, 10000, 1000, 100, -1, m_iIndex == 0? Color.Green : Color.Red);

                bInitOk = m_Area != null;

                if (bInitOk)
                    m_Area.RegisterObserver(this);
            }

            return bInitOk;
        }

        public Vector3 GetDirection()
        {
            return m_vDirection;
        }

        public void AddPlayer(Player p)
        {
            if (!m_PlayersRanking.Contains(p))
                m_PlayersRanking.Add(p);
        }

        public void RemovePlayer(Player p)
        {
            m_PlayersRanking.Remove(p);
        }

        public List<Player> GetRankings()
        {
            return m_PlayersRanking;
        }

        public int GetIndex()
        {
            return m_iIndex;
        }

        public void OnEnter(Mesh m)
        {
            // we detect the player going forwards
            Player p = PlayerManager.GetInstance().GetPlayerByVehicleMesh(m);

            if (p != null)
            {
                // we first check the direction of the player movement and the direction of the checpoint
                Vector3 vel = p.GetVelocity();
                vel.Normalize();
                float dot = Vector3.Dot(vel, m_vDirection);
                
                if (float.IsNaN(dot) || dot >= 0.0)
                    m_Circuit.OnPlayerForwardCheckpoint(p, this);
            }
        }

        public void OnExit(Mesh m)
        {
            // we detect that the player is going backwards
            Player p = PlayerManager.GetInstance().GetPlayerByVehicleMesh(m);

            if (p != null)
            {
                // we first check the direction of the player movement and the direction of the checpoint
                Vector3 vel = p.GetVelocity();
                vel.Normalize();
                float dot = Vector3.Dot(vel, m_vDirection);

                if (dot < 0.0)
                    m_Circuit.OnPlayerBackwardCheckpoint(p, this);
            }
        }

        public void Update(float dt, float t) { }

        public void Draw()
        {
            base.Draw();

            Vector3 p = m_vPosition + new Vector3(0, 1000, 0);
            DrawDebugManager.GetInstance().DrawLine(p , p + GetForward() * 1000, Color.Yellow);

            m_Area.Draw();
        }
    }
}
