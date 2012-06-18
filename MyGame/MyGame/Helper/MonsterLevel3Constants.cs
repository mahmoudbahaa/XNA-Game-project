using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class MonsterLevel3Constants : MonsterConstants
    {
        public MonsterLevel3Constants()
        {
            MONSTER_SPEED = Constants.PLAYER_SPEED * 40;
            MONSTER_HEALTH_PER_BULLET = 10;
            PLAYER_HEALTH_DECREASE = 20;
        }
    }
}
