using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class MonsterLevel2Constants : MonsterConstants
    {
        public MonsterLevel2Constants()
        {
            MONSTER_SPEED = Constants.PLAYER_SPEED * 30;
            MONSTER_HEALTH_PER_BULLET = 20;
            PLAYER_HEALTH_DECREASE = 10;
            
        }
    }
}
