using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Helper
{
    class XtremeConstants:DifficultyConstants
    {
        public XtremeConstants()
        {
            MONSTER_SPEED = Constants.PLAYER_SPEED * 40;
            MONSTER_HEALTH_PER_BULLET = 10;
            PLAYER_HEALTH_DECREASE = 20;
            INCREASED_HEALTH_BY_MEDKIT = 25;
            NUM_MEDKITS_IN_FIELD = 3;
            NUM_MONSTERS_IN_FIELD = 100;
        }
    }
}
