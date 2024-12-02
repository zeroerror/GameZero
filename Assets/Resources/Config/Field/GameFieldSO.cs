using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_field_", menuName = "游戏玩法/配置/场景模板")]
    public class GameFieldSO : GameSOBase
    {
        [Header("场景预制体")]
        public GameObject fieldPrefab;
    }
}