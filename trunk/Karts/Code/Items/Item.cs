using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Karts.Code
{
    class Item : Mesh
    {
        // ------------------------------------------------
        // Class types
        // ------------------------------------------------
        public enum EItemTypes
        {
            E_ITEM_TYPE_INVALID     = 0,
            E_ITEM_TYPE_TURBO       = 1,
            E_ITEM_TYPE_SHIELD      = 2,
            E_ITEM_TYPE_BOMB        = 3
        };

        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private ItemArea m_IteamArea;

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Item() { }
        ~Item() { }

        public bool Init(Vector3 position, Vector3 rotation)
        {
            m_vPosition = position;
            m_vRotation = rotation;

            bool bLoadOk = Load("duck");

            SetScale(1000.0f);

            return bLoadOk;
        }

        public virtual EItemTypes GetItemType() { return EItemTypes.E_ITEM_TYPE_INVALID; }

        public void Update(float dt, float t)
        {
            // Update item movement and effects?
        }

        public void Draw(Matrix ProjMatrix, Matrix ViewMatrix)
        {
            base.Draw(ProjMatrix, ViewMatrix);
        }
    }
}
