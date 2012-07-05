using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Class containing the monster constants.
    /// </summary>
    public class MonsterConstants
    {
        /// <summary> the monster speed</summary>
        public float MONSTER_SPEED;

        /// <summary> HP decreased from monster when it is damaged by a bullet.</summary>
        public int MONSTER_HEALTH_PER_BULLET;

        /// <summary> HP decreased from player when monster attack him.</summary>
        public int PLAYER_HEALTH_DECREASE;

        /// <summary> amount of score added when player kill a monster.</summary>
        public int SCORE;
    }
}
