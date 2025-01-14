using GamePlay.Core;
using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameEntitySelectorEM
    {
        [Header("选择锚点类型")]
        public GameEntitySelectAnchorType selectAnchorType;
        [Header("阵营类型")]
        public GameCampType campType;
        [Header("实体类型")]
        public GameEntityType entityType;

        [Header("范围选择类型")]
        public GameColliderType selColliderType;
        public GameBoxColliderModel boxColliderModel;
        public GameCircleColliderModel circleColliderModel;
        public GameFanColliderModel fanColliderModel;

        public GameEntitySelector ToModel()
        {
            GameEntitySelector selector = new GameEntitySelector();
            selector.selectAnchorType = this.selectAnchorType;
            selector.campType = this.campType;
            selector.entityType = this.entityType;
            switch (this.selColliderType)
            {
                case GameColliderType.None:
                    selector.colliderModel = null;
                    break;
                case GameColliderType.Box:
                    selector.colliderModel = this.boxColliderModel;
                    break;
                case GameColliderType.Circle:
                    selector.colliderModel = this.circleColliderModel;
                    break;
                case GameColliderType.Fan:
                    selector.colliderModel = this.fanColliderModel;
                    break;
                default:
                    selector.colliderModel = null;
                    GameLogger.LogError("编辑时实体选择模型，未处理的碰撞模型类型: " + this.selColliderType);
                    break;
            }
            return selector;
        }
    }
}