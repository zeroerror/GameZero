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

        public List<GameEntityBase> GetSelectdeEntities(GameEntitySelector selector, GameEntityBase actorEntity)
        {
            var physicsApi = this._context.domainApi.physicsApi;
            var selColliderModel = selector.colliderModel;
            var isSingleSelect = selColliderModel == null;
            var targetEntity = actorEntity.actionTargeterCom.targetEntity;
            var list = new List<GameEntityBase>();
            // 单体选择
            if (isSingleSelect)
            {
                if (targetEntity != null && selector.CheckSelect(actorEntity, targetEntity)) list.Add(targetEntity);
                return list;
            }
            // 范围选择
            GameTransformArgs anchorTransformArgs;
            var selectAnchorType = selector.selectAnchorType;
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
                    return list;
            }
            list = physicsApi.GetOverlapEntities(selColliderModel, anchorTransformArgs);
            list = list.Filter((entity) =>
            {
                return selector.CheckSelect(actorEntity, entity);
            });
            return list;
        }
    }
}