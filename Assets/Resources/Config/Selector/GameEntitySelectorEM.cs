using GamePlay.Core;
using GamePlay.Bussiness.Logic;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameEntitySelectorEM
    {
        public GameEntitySelectAnchorType selectAnchorType;
        public GameCampType campType;
        public GameEntityType entityType;
        public bool onlySelectDead;

        public GameColliderType selColliderType;
        public GameBoxColliderModel boxColliderModel;
        public GameCircleColliderModel circleColliderModel;
        public GameFanColliderModel fanColliderModel;
        public int rangeSelectLimitCount;
        public GameEntitySelectSortType rangeSelectSortType;

        public GameEntitySelector ToModel()
        {
            GameEntitySelector selector = new GameEntitySelector();
            selector.selectAnchorType = this.selectAnchorType;
            selector.campType = this.campType;
            selector.entityType = this.entityType;
            selector.onlySelectDead = this.onlySelectDead;

            // 范围选取相关
            switch (this.selColliderType)
            {
                case GameColliderType.None:
                    selector.rangeSelectModel = null;
                    break;
                case GameColliderType.Box:
                    selector.rangeSelectModel = this.boxColliderModel;
                    break;
                case GameColliderType.Circle:
                    selector.rangeSelectModel = this.circleColliderModel;
                    break;
                case GameColliderType.Fan:
                    selector.rangeSelectModel = this.fanColliderModel;
                    break;
                default:
                    selector.rangeSelectModel = null;
                    GameLogger.LogError("编辑时实体选择模型，未处理的碰撞模型类型: " + this.selColliderType);
                    break;
            }
            selector.rangeSelectLimitCount = this.rangeSelectLimitCount;
            selector.rangeSelectSortType = this.rangeSelectSortType;

            return selector;
        }
    }
}