using System.Collections.Generic;

namespace GamePlay.Core
{
    /** 游戏摄像机剔除标记 */
    public enum GameCameraCullingMask
    {
        /** 默认 */
        Normal = 1,
        /** 场景 */
        Scene = 1 << 1,
        /** 电视 */
        TV = 1 << 2,
    }

    /** 摄像机剔除标记扩展 */
    public static class GameCameraCullingMaskExtension
    {
        private static readonly Dictionary<GameCameraCullingMask, string> MaskNames = new Dictionary<GameCameraCullingMask, string>
        {
            { GameCameraCullingMask.Normal, "normal" },
            { GameCameraCullingMask.Scene, "scene" },
            { GameCameraCullingMask.TV, "tv" },
        };

        public static GameCameraCullingMask AddMask(GameCameraCullingMask mask, params GameCameraCullingMask[] masks)
        {
            var result = mask;
            foreach (var m in masks)
            {
                result |= m;
            }
            return result;
        }

        public static GameCameraCullingMask RemoveMask(GameCameraCullingMask mask, params GameCameraCullingMask[] masks)
        {
            var result = mask;
            foreach (var m in masks)
            {
                result &= ~m;
            }
            return result;
        }

        public static string GetMaskName(GameCameraCullingMask mask)
        {
            return MaskNames.ContainsKey(mask) ? MaskNames[mask] : string.Empty;
        }
    }

}