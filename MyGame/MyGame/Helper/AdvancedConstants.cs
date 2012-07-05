using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Helper
{
    /// <summary>
    /// Class extend DifficulityConstants and contains constant of the Advanced Difficulity.
    /// </summary>
    class AdvancedConstants:DifficultyConstants
    {
        public AdvancedConstants()
        {
            INCREASED_HEALTH_BY_MEDKIT = 50;
            NUM_MEDKITS_IN_FIELD = 5;
            NUM_MONSTERS_IN_FIELD = 25;
        }
    }
}
