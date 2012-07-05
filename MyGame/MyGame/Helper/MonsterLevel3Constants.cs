using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Class that extends MonsterConstants Class, contains the constants for monster type 3.
    /// </summary>
    public class MonsterLevel3Constants : MonsterConstants
    {
        public MonsterLevel3Constants()
        {
            SCORE = 4;
            MONSTER_SPEED = Constants.PLAYER_SPEED * 35;
            MONSTER_HEALTH_PER_BULLET = 15;
            PLAYER_HEALTH_DECREASE = 15;
        }
    }
}
