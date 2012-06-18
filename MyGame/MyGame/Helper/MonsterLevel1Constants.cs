using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class MonsterLevel1Constants : MonsterConstants
    {
        public MonsterLevel1Constants()
        {
            MONSTER_SPEED = Constants.PLAYER_SPEED * 20;
            MONSTER_HEALTH_PER_BULLET = 30;
            PLAYER_HEALTH_DECREASE = 5;
        }
    }
}
