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

        public void Destroy()
        {
        }

        public void Tick(float dt)
        {
        }

        public List<GameEntityBase> SelectEntities(GameEntitySelector selector, GameEntityBase actEntity, bool ignoreRepeatCollision = true)
        {
            var physicsApi = this._context.domainApi.physicsApi;
            var selColliderModel = selector.colliderModel;
            var isSingleSelect = selColliderModel == null;
            var targetEntity = actEntity.actionTargeterCom.targetEntity;
            var selectAnchorType = selector.selectAnchorType;

            // 单体选择
            if (isSingleSelect)
            {
                var selectedEntity = this._GetSingleSelectedEntity(actEntity, targetEntity, selectAnchorType);
                if (selectedEntity == null) return null;
                if (!selector.onlySelectDead && !selectedEntity.IsAlive()) return null;
                return new List<GameEntityBase> { selectedEntity };
            }

            // 范围选择 - 锚点
            GameTransformArgs anchorTransformArgs = this._GetRangeSelectAnchorTrans(actEntity, targetEntity, selectAnchorType);

            var list = physicsApi.GetOverlapEntities(selColliderModel, anchorTransformArgs);
            list = list?.Filter((entity) =>
            {
                if (entity is GameRoleEntity role)
                {
                    var isDead = role.fsmCom.stateType == GameRoleStateType.Dead;
                    if (isDead) return false;
                }
                var checkCollided = actEntity.physicsCom.CheckCollided(entity.idCom.ToArgs());
                if (checkCollided) return false;
                var checkSelect = selector.CheckSelect(actEntity, entity);
                if (ignoreRepeatCollision && checkSelect) actEntity.physicsCom.AddCollided(entity.idCom.ToArgs());
                return checkSelect;
            });
            return list;
        }

        private GameEntityBase _GetSingleSelectedEntity(GameEntityBase actorEntity, GameEntityBase targetEntity, GameEntitySelectAnchorType selectAnchorType)
        {
            switch (selectAnchorType)
            {
                case GameEntitySelectAnchorType.Actor:
                    return actorEntity;
                case GameEntitySelectAnchorType.ActorRole:
                    return actorEntity.GetLinkParent<GameRoleEntity>();
                case GameEntitySelectAnchorType.ActTarget:
                    return targetEntity;
                default:
                    GameLogger.LogError($"单体选择 未处理的选择器锚点类型: {selectAnchorType}");
                    return null;
            }
        }

        private GameTransformArgs _GetRangeSelectAnchorTrans(GameEntityBase actorEntity, GameEntityBase targetEntity, GameEntitySelectAnchorType selectAnchorType)
        {
            GameTransformArgs anchorTransformArgs;
            switch (selectAnchorType)
            {
                case GameEntitySelectAnchorType.Actor:
                    anchorTransformArgs = actorEntity.transformCom.ToArgs();
                    break;
                case GameEntitySelectAnchorType.ActTarget:
                    if (!targetEntity) return default;
                    anchorTransformArgs = targetEntity.transformCom.ToArgs();
                    break;
                default:
                    GameLogger.LogError($"选择器锚点类型未知: {selectAnchorType}");
                    return default;
            }
            return anchorTransformArgs;
        }

        public GameVec2 GetSelectorAnchorPosition(GameEntityBase actor, GameEntitySelector selector)
        {
            var selectAnchorType = selector.selectAnchorType;
            var isSingleSelect = selector.colliderModel == null;
            var target = actor.actionTargeterCom.targetEntity;
            if (isSingleSelect)
            {
                return selectAnchorType == GameEntitySelectAnchorType.Actor ? actor.transformCom.position : target.transformCom.position;
            }
            var anchorTrans = this._GetRangeSelectAnchorTrans(actor, target, selectAnchorType);
            return anchorTrans.position;
        }

        public bool CheckSelectorAnchor(GameEntityBase actor, GameEntitySelector selector)
        {
            if (actor == null) return false;
            if (selector == null) return false;

            var selectAnchorType = selector.selectAnchorType;
            var isSingleSelect = selector.colliderModel == null;
            var target = actor.actionTargeterCom.targetEntity;
            if (isSingleSelect)
            {
                switch (selectAnchorType)
                {
                    case GameEntitySelectAnchorType.Actor:
                        return actor.IsAlive();
                    case GameEntitySelectAnchorType.ActorRole:
                        var role = actor.GetLinkParent<GameRoleEntity>();
                        return role.IsAlive();
                    case GameEntitySelectAnchorType.ActTarget:
                        return target.IsAlive();
                    default:
                        GameLogger.LogError($"未处理的选择器锚点类型: {selectAnchorType}");
                        return false;
                }
            }

            return actor.IsAlive();
        }
    }
}