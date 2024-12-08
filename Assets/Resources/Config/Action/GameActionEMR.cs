using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameActionEMR
    {
        public int typeId;

        public GameObject prefab;
        public GameVec2 scale = GameVec2.one;
        public GameVec2 offset = GameVec2.zero;

        public GameCameraShakeModel camShakeModel;

        public GameActionModelR ToModel()
        {
            var url = this.prefab ? UnityEditor.AssetDatabase.GetAssetPath(this.prefab) : null;
            if (url != null)
            {
                url = System.Text.RegularExpressions.Regex.Replace(url, @"Assets/Resources/", "");
                url = url.Substring(0, url.LastIndexOf('.'));
            }
            if (scale.x == 0 || scale.y == 0)
            {
                GameLogger.LogWarning("请注意 行为特效缩放值为0");
            }
            return new GameActionModelR(
                this.typeId,
                url,
                this.scale,
                this.offset,
                this.camShakeModel
            );
        }

    }
}