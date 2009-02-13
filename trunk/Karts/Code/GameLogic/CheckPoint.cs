using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    class CheckPoint : Mesh
    {
        //----------------------------------
        // Class members
        //----------------------------------


        //----------------------------------
        // Class methods
        //----------------------------------
        public CheckPoint () : base()
        {
        }

        ~CheckPoint()
        {
        }



    }
}
