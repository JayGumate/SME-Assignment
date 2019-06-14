using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys {

    public class Pools
    {
        public const int MISSILE_POOL = 100;
        public const int ENEMIES_POOL = 60;
        public const int ENEMIES_PER_COLUMN = 5;
    }

    public class Times
    {

        public const float TIME_TO_NEXT_PLAYER_SHOOT = 0.7f;
        public const float TIME_TO_NEXT_ENEMY_MOVE = 0.5f;
        public const float TIME_TO_NEXT_LEVEL = 6f;
        public const float TIME_TO_NEXT_ENEMY_SHOOT = 0.8f;

    }

    public class Distances
    {

        public const float ENEMIES_X_MOVE = 0.35f;
        public const float ENEMIES_Y_MOVE = 0.3f;

    }

    public class Strings
    {

        public const string SCORE = "SCORE: ";
        public const string HP = "HP: ";
        public const string NORMAL = "NORMAL";

    }

    public class Events
    {

        public const string SCORE = "SCORE";
        public const string HP = "HP";
        public const string GAME_OVER = "GAME_OVER";
        public const string WIN = "WIN";
        public const string LEVEL_UP = "LEVEL_UP";
        public const string READY = "READY";
        public const string PAUSE = "PAUSE";

    }
    public class InputFiles
    {

        public const string ENEMY_STATS_FILE_PATH = "EnemyStats";

        public enum StatsKeys
        {
        Damage,
        FireRate,
        ReloadTime,
        AmmoPerReload
        }

    }


}
