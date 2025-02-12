using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public static class GameDirectorRCCollection
    {
        /// <summary> 导演 - 时间缩放变更 </summary>
        public static readonly string RC_GAME_DIRECTOR_TIME_SCALE_CHANGE = "RC_GAME_DIRECTOR_TIME_SCALE_CHANGE";
        /// <summary> 导演 - 状态退出 </summary>
        public static readonly string RC_GAME_DIRECTOR_STATE_EXIT = "RC_GAME_DIRECTOR_STATE_EXIT";

        /// <summary> 导演 - 进入 加载 </summary>
        public static readonly string RC_GAME_DIRECTOR_STATE_ENTER_LOADING = "RC_GAME_DIRECTOR_STATE_ENTER_LOADING";
        /// <summary> 导演 - 进入 战斗 </summary>
        public static readonly string RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING = "RC_GAME_DIRECTOR_STATE_ENTER_FIGHTING";
        /// <summary> 导演 - 进入 结算 </summary>
        public static readonly string RC_GAME_DIRECTOR_STATE_ENTER_SETTLING = "RC_GAME_DIRECTOR_STATE_ENTER_SETTLING";
        /// <summary> 导演 - 退出 结算 </summary>
        public static readonly string RC_GAME_DIRECTOR_STATE_EXIT_SETTLING = "RC_GAME_DIRECTOR_STATE_EXIT_SETTLING";
        /// <summary> 导演 - 进入 战斗准备 </summary>
        public static readonly string RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING = "RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING";
        /// <summary> 导演 - 战斗准备全员就位 </summary>
        public static readonly string RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING_POSITIONED = "RC_GAME_DIRECTOR_STATE_ENTER_FIGHT_PREPARING_POSITIONED";
        /// <summary> 导演 - 金币变更 </summary>
        public static readonly string RC_GAME_DIRECTOR_COINS_CHANGE = "RC_GAME_DIRECTOR_COINS_CHANGE";
        /// <summary> 导演 - 购买单位 </summary>
        public static readonly string RC_GAME_DIRECTOR_BUY_UNIT = "RC_GAME_DIRECTOR_BUY_UNIT";
        /// <summary> 导演 - 清空战场 </summary>
        public static readonly string RC_GAME_DIRECTOR_CLEAR_FIELD = "RC_GAME_DIRECTOR_CLEAR_FIELD";
    }

    /// <summary> 参数 - 导演 - 时间缩放变更 </summary>
    public struct GameDirectorRCArgs_StateEnterFighting
    {
        public GameDirectorStateType fromStateType;
    }

    /// <summary> 参数 - 导演 - 状态退出 </summary>
    public struct GameDirectorRCArgs_StateExit
    {
        public GameDirectorStateType exitStateType;
    }

    /// <summary> 参数 - 导演 - 进入 加载 </summary>
    public struct GameDirectorRCArgs_StateEnterLoading
    {
        public int fieldId;
    }

    /// <summary> 参数 - 导演 - 进入 结算 </summary>
    public struct GameDirectorRCArgs_StateEnterSettling
    {
        public GameDirectorStateType fromStateType;
        public int playerCount;
        public int enemyCount;
        public bool isWin;
        public int ganiCoins;
    }

    /// <summary> 参数 - 导演 - 退出 结算 </summary>
    public struct GameDirectorRCArgs_StateExitSettling
    {
        public GameDirectorStateType toStateType;
    }

    /// <summary> 参数 - 导演 - 进入 战斗准备 </summary>
    public struct GameDirectorRCArgs_StateEnterFightPreparing
    {
        public GameDirectorStateType fromStateType;
        public List<GameActionOptionModel> actionOptions;
    }

    /// <summary> 参数 - 导演 - 金币变更 </summary>
    public struct GameDirectorRCArgs_GoldChange
    {
        public int gold;
    }

    /// <summary> 参数 - 导演 - 购买单位 </summary>
    public struct GameDirectorRCArgs_BuyUnit
    {
        public GameItemUnitModel model;
        public int costGold;
    }

    /// <summary> 参数 - 导演 - 清空战场 </summary>
    public struct GameDirectorRCArgs_CleanBattleField
    {
        public int fieldId;
    }
}