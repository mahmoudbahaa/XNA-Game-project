using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Helper
{
    /// <summary>
    /// Class extend DifficulityConstants and contains constant of the extreme Difficulity.
    /// </summary>
    class XtremeConstants:DifficultyConstants
    {
        public XtremeConstants()
        {
            INCREASED_HEALTH_BY_MEDKIT = 25;
            NUM_MEDKITS_IN_FIELD = 3;
            NUM_MONSTERS_IN_FIELD = 50;
        }
    }
}
