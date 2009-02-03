using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// ----------------------------------------------------------------------------------
// This class specifies the vehicle. The vehicle contains its 3D model.
// ----------------------------------------------------------------------------------

namespace Karts.Code
{
    class Vehicle
    {
        // ------------------------------------------------
        // Class members
        // ------------------------------------------------


        // ------------------------------------------------
        // Class methods
        // ------------------------------------------------
        public Vehicle() { }
        ~Vehicle() { }

        public bool Init(String resource_name)
        {
            
            return true;
        }

        public void Update(GameTime GameTime)
        {
        }

        public void Draw(GameTime GameTime)
        {
        }
    }
}
