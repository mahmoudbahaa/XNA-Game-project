using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class MonsterLevel4Constants : MonsterConstants
    {
        public MonsterLevel4Constants()
        {
            SCORE = 6;
            MONSTER_SPEED = Constants.PLAYER_SPEED * 40;
            MONSTER_HEALTH_PER_BULLET = 10;
            PLAYER_HEALTH_DECREASE = 20;
        }
    }
}
