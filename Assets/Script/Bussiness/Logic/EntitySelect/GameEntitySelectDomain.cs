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
            var transformArgs = actorEntity.transformCom.ToArgs();
            var selColliderModel = selector.colliderModel;
            var isSingleSelect = selColliderModel == null;
            var targetEntity = actorEntity.actionTargeterCom.targetEntity;
            // 单体选择
            if (isSingleSelect)
            {
                var list = new List<GameEntityBase>();
                if (targetEntity != null && selector.CheckSelect(actorEntity, targetEntity)) list.Add(targetEntity);
                return list;
            }
            // 范围选择
            var overlapEntities = physicsApi.GetOverlapEntities(selColliderModel, transformArgs);
            overlapEntities = overlapEntities.Filter((entity) =>
            {
                return selector.CheckSelect(actorEntity, entity);
            });
            return overlapEntities;
        }
    }
}