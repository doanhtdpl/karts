using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class CircuitManager : DrawableGameComponent
    {
        //---------------------------------------------------
        // Class members
        //---------------------------------------------------
        private static CircuitManager m_CircuitManager = null;

        private List<Circuit> m_CircuitList = new List<Circuit>();
        private Circuit m_CurrentCircuit;

        //---------------------------------------------------
        // Class methods
        //---------------------------------------------------
        public CircuitManager(Game game) : base(game) { }
        ~CircuitManager(){}

        public static CircuitManager GetInstance()
        {
            return m_CircuitManager;
        }

        public static CircuitManager Init(Game game)
        {
            if (m_CircuitManager == null)
            {
                m_CircuitManager = new CircuitManager(game);
            }

            return m_CircuitManager;
        }

        public bool CreateCircuit(Vector3 position, Vector3 rotation, string circuit_name)
        {
            bool bInitOk = false;
            Circuit newCircuit = new Circuit();

            bInitOk = newCircuit.Init(position, rotation, circuit_name);

            if (bInitOk)
            {
                // This is temporary right now
                m_CurrentCircuit = newCircuit;
            }

            return bInitOk;
        }

        public List<Circuit>  GetCircuits()
        {
            return m_CircuitList;
        }

        public int GetNumCircuits()
        {
            return m_CircuitList.Count;
        }

        public Circuit GetCircuitByID(UInt32 uID)
        {
            return m_CircuitList.Find(new FindCircuitID(uID).CompareID); ;
        }

        public Circuit GetCurrentCircuit()
        {
            return m_CurrentCircuit;
        }


        public override void Update(GameTime gameTime)
        {
            if (m_CurrentCircuit != null)
                m_CurrentCircuit.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (m_CurrentCircuit != null)
                m_CurrentCircuit.Draw(gameTime);
        }

        //---------------------------------------------------
        // Class predicates
        //---------------------------------------------------
        struct FindCircuitID
        {
            UInt32 uID;

            public FindCircuitID(UInt32 _uID) { uID = _uID; }
            public bool CompareID(Circuit c)
            {
                return c.GetID() == uID;
            }
        }
    }
}
