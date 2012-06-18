using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Helper
{
    public class Constants
    {
        public const int RIGHT_HAND = 0;
        public const int LEFT_HAND = 1;

        public const int TERRAIN_TEXTURE_TILING = 10;
        public const int TERRAIN_CELL_SIZE = 3;
        public const int TERRAIN_HEIGHT = 480;

        public const int WATER_HEIGHT = 80;


        public const int NUM_OF_TERRAINS = 4;
        public const int FIELD_MAX_X_Z = 2 * (int)(250 * TERRAIN_CELL_SIZE * 506 / 512);

        public static Vector3 CAMERA_POSITION = new Vector3(0, 15, 60);
        public static Vector3 CAMERA_TARGET = new Vector3(0, 20, 0);

        public const    float PLAYER_SPEED = .05f;//.1f;
        public static Vector3 PLAYER_SCALE = new Vector3(.75f);//new Vector3(2f);

        public static Vector3 MONSTER_SCALE = new Vector3(.185f);//new Vector3(.5f);

        public static Vector3 HP_OFFSET = new Vector3(0, 20, 0);//new Vector3(0, 60, 0);
        public static Vector2 HP_SIZE = new Vector2(50, 10);//new Vector2(100, 20);

        public static Vector3 MEDKIT_SCALE = new Vector3(.25f);//new Vector3(.5f);
        public static Vector3 MEDKIT_OFFSET = new Vector3(0,20,0);//new Vector3(0,30,0);

        public const    float BULLET_SPEED = 80f;
        public static Vector3 BULLET_SCALE = new Vector3(.5f);//new Vector3(10f);
        public static Vector3 BULLET_OFFSET = new Vector3(0, 20, 0);//new Vector3(0, 40, 0);

        public static Vector2 TREE_SIZE = new Vector2(50);
        public static Vector2 GRASS_SIZE = new Vector2(8);
        //public const int SLEEP_TIME = 100;
        public static String[] DifficultiesString = { "Novice" ,"Advanced" ,"Xtreme"};
        public enum Difficulties {
            Novice = 0,
            Advanced,
            Xtreme
        };
    }
}
