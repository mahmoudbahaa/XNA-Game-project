using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Helper
{
    class Constants
    {
        public const int RIGHT_HAND = 0;
        public const int LEFT_HAND = 1;

        public const float TERRAIN_TEXTURE_TILING = 10;
        public const float TERRAIN_CELL_SIZE = 10;
        public const float TERRAIN_HEIGHT = 500;


        public const int FIELD_MAX_X_Z = (int)(250 * TERRAIN_CELL_SIZE * 480 / 512);

        public static Vector3 CAMERA_POSITION = new Vector3(0, 40, 150);
        public static Vector3 CAMERA_TARGET = new Vector3(0, 50, 0);

        public const    float PLAYER_SPEED = .1f;//.1f;
        public static Vector3 PLAYER_SCALE = new Vector3(2f);//new Vector3(2f);

        public static Vector3 MONSTER_SCALE = new Vector3(.5f);//new Vector3(.5f);
        public const    float MONSTER_SPEED = 3f;//3f;

        public static Vector3 HP_OFFSET = new Vector3(0, 60, 0);//new Vector3(0, 60, 0);
        public static Vector2 HP_SIZE = new Vector2(100, 20);//new Vector2(100, 20);

        public static Vector3 MEDKIT_SCALE = new Vector3(.5f);//new Vector3(.5f);

        public const    float BULLET_SPEED = 20f;
        public static Vector3 BULLET_SCALE = new Vector3(10);//new Vector3(10f);
        public static Vector3 BULLET_OFFSET = new Vector3(0, 40, 0);//new Vector3(0, 40, 0);
        //public const int SLEEP_TIME = 100;
    }
}
