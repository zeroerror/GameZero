using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.Logic
{
    public static class GameDrawRCCollection
    {
        public static readonly string RC_DRAW_COLLIDER = "RC_DRAW_COLLIDER";
        public static readonly string RC_DRAW_COLLIDER_MODEL = "RC_DRAW_COLLIDER_MODEL";
    }

    public struct GameRCArgs_DrawCollider
    {
        public GameColliderBase collider;
        public Color color;
        public float maintainTime;
    }


    public struct GameRCArgs_DrawColliderModel
    {
        public GameColliderModelBase colliderModel;
        public GameTransformArgs transformArgs;
        public Color color;
        public float maintainTime;
    }
}