using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

// --------------------------------------------------------------
// --------------------------------------------------------------
// ItemArea class
//
// This class represents a virtual area in a circuit that is filled with
// recollectable items. The area will have slots where the items 
// will be respawned during the game. It will so manage the spawn
// time of each item inside.
// ---------------------------------------------------------------
// --------------------------------------------------------------
namespace Karts.Code
{
    // ------------------------------------------------
    // Class structures
    // ------------------------------------------------
    class ItemSlot
    {
        public bool bEmpty;          // Is the slot empty?
        public float fTakenTime;     // Time the item was taken
        public Vector3 vPosition;    // 3D Position of the slot on the ItemArea
        public Item item;            // Current spawned item

        public ItemSlot() 
        {
            bEmpty = false;
            fTakenTime = 0.0f;
            vPosition = Vector3.Zero;
            item = null;
        }
    }

    class ItemArea : Object3D
    {
        

        // ------------------------------------------------
        // Class members
        // ------------------------------------------------
        private List<ItemSlot> m_ItemAreaList = new List<ItemSlot>();   // Item Slots
        private float m_fRespawnTime = 5.0f;                            // Time of items respawn

        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public ItemArea() { }
        ~ItemArea() { }

        public bool Init(Vector3 position, Vector3 rotation)
        {
            m_vPosition = position;
            m_vRotation = rotation;

            for (int i = 0; i < 4; i++)
            {
                ItemSlot itemSlot = new ItemSlot();
                itemSlot.bEmpty = false;
                itemSlot.fTakenTime = 0.0f;
                itemSlot.vPosition = new Vector3(0.0f + i * 1000.0f, 0.0f, 0.0f);

                itemSlot.item = new Item();

                itemSlot.item.Init(m_vPosition + itemSlot.vPosition, Vector3.Zero);

                m_ItemAreaList.Add(itemSlot);
            }
            /*
            ItemSlot itemSlot = new ItemSlot();
            itemSlot.bEmpty = false;
            itemSlot.fTakenTime = 0.0f;
            itemSlot.vPosition = Vector3.Zero;

            itemSlot.item = new Item();

            itemSlot.item.Init(m_vPosition + itemSlot.vPosition, Vector3.Zero);

            m_ItemAreaList.Add(itemSlot);

            ItemSlot itemSlot2 = new ItemSlot();
            itemSlot2.bEmpty = false;
            itemSlot2.fTakenTime = 0.0f;
            itemSlot2.vPosition = new Vector3(1000.0f, 0.0f, 1000.0f);

            itemSlot2.item = new Item();
            
            itemSlot2.item.Init(m_vPosition + itemSlot2.vPosition, Vector3.Zero);

            m_ItemAreaList.Add(itemSlot2);
             * */

            return true;
        }

        public void Update(float dt, float t)
        {
            Player p = PlayerManager.GetInstance().GetPlayers()[0];

            // TODO: Check the collisions??
            for (int i = 0; i < m_ItemAreaList.Count; ++i)
            {
                ItemSlot slot = m_ItemAreaList[i];
                if (!slot.bEmpty && slot.item.CollidesWithMesh(p.GetVehicle()))
                {
                    slot.fTakenTime = t;
                    slot.item = null;
                    slot.bEmpty = true;
                }
            }

            // Update individual items
            for (int i = 0; i < m_ItemAreaList.Count; ++i)
            {
                ItemSlot slot = m_ItemAreaList[i];

                if (slot.bEmpty)
                {
                    // we update the respawn time
                    if ((slot.fTakenTime + m_fRespawnTime) < t)
                    {
                        // TODO: we generate a new item
                        slot.bEmpty = false;
                        slot.item = new Item();
                        slot.fTakenTime = 0.0f;
                        slot.item.Init(m_vPosition + slot.vPosition, Vector3.Zero);
                    }
                }
                else
                {
                    slot.item.Update(dt, t);
                }
            }
        }

        public void Draw(Matrix ProjMatrix, Matrix ViewMatrix)
        {
            base.Draw();

            foreach (ItemSlot itemSlot in m_ItemAreaList)
            {
                if (!itemSlot.bEmpty)
                    itemSlot.item.Draw(ProjMatrix, ViewMatrix);
            }
        }

        // ------------------------------------------------
        // Class predicates
        // ------------------------------------------------
        private struct FindItemType
        {
            Item.EItemTypes eType;

            public FindItemType(Item.EItemTypes _type) { eType = _type; }
            public bool CompareType(ItemSlot i)
            {
                return i.item.GetItemType() == eType;
            }
        }
    }
}
