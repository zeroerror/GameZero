using System.Collections.Generic;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameEntitySelectDomain : GameEntitySelectDomainApi
    {
        GameContext _context;

        public GameEntitySelectDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Dispose()
        {
        }

        public void Tick(float dt)
        {
        }

        public List<GameEntityBase> SelectEntities(GameEntitySelector selector, GameEntityBase actorEntity, bool ignoreRepeatCollision = true)
        {
            var physicsApi = this._context.domainApi.physicsApi;
            var selColliderModel = selector.colliderModel;
            var isSingleSelect = selColliderModel == null;
            var targetEntity = actorEntity.actionTargeterCom.targetEntity;
            var selectAnchorType = selector.selectAnchorType;

            // 单体选择
            if (isSingleSelect)
            {
                var selectedEntity = this._GetSingleSelectedEntity(selector, actorEntity, targetEntity, selectAnchorType);
                if (selectedEntity == null) return null;
                return new List<GameEntityBase> { selectedEntity };
            }

            // 范围选择
            GameTransformArgs anchorTransformArgs;
            switch (selectAnchorType)
            {
                case GameEntitySelectAnchorType.Self:
                    anchorTransformArgs = actorEntity.transformCom.ToArgs();
                    break;
                case GameEntitySelectAnchorType.Target:
                    anchorTransformArgs = targetEntity.transformCom.ToArgs();
                    break;
                default:
                    GameLogger.LogError($"选择器锚点类型未知: {selectAnchorType}");
                    return null;
            }
            var list = physicsApi.GetOverlapEntities(selColliderModel, anchorTransformArgs);
            list = list?.Filter((entity) =>
            {
                var checkCollided = actorEntity.physicsCom.CheckCollided(entity.idCom.ToArgs());
                if (checkCollided) return false;
                var checkSelect = selector.CheckSelect(actorEntity, entity);
                if (ignoreRepeatCollision && checkSelect) actorEntity.physicsCom.AddCollided(entity.idCom.ToArgs());
                return checkSelect;
            });
            return list;
        }

        private GameEntityBase _GetSingleSelectedEntity(GameEntitySelector selector, GameEntityBase actorEntity, GameEntityBase targetEntity, GameEntitySelectAnchorType selectAnchorType)
        {
            if (selectAnchorType == GameEntitySelectAnchorType.Self) return actorEntity;
            if (selectAnchorType == GameEntitySelectAnchorType.Target)
            {
                if (selector.CheckSelect(actorEntity, targetEntity)) return targetEntity;
                return null;
            }
            GameLogger.LogError($"单体选择 未处理的选择器锚点类型: {selectAnchorType}");
            return null;
        }
    }
}