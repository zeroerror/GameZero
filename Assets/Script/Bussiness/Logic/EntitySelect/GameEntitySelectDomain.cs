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

        public List<GameEntityBase> SelectEntities(GameEntitySelector selector, GameEntityBase actEntity, bool avoidRepeatRangeSelect = true)
        {
            var physicsApi = this._context.domainApi.physicsApi;
            var selColliderModel = selector.rangeSelectModel;
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

            // 范围选取锚点
            GameTransformArgs anchorTransformArgs = this._GetRangeSelectAnchorTrans(actEntity, targetEntity, selectAnchorType);
            // 范围选取
            var list = physicsApi.GetOverlapEntities(selColliderModel, anchorTransformArgs);
            list = list?.Filter((entity) =>
            {
                // 死亡单位过滤
                if (!selector.onlySelectDead && !entity.IsAlive()) return false;
                if (selector.onlySelectDead && entity.IsAlive()) return false;

                // 被重复选取的需要过滤
                var checkRepeatSelect = actEntity.physicsCom.CheckCollided(entity.idCom.ToArgs());
                if (checkRepeatSelect) return false;

                // 选中时记录
                var checkSelect = selector.CheckSelect(actEntity, entity);
                if (avoidRepeatRangeSelect && checkSelect) actEntity.physicsCom.AddCollided(entity.idCom.ToArgs());

                return checkSelect;
            });
            // 单位列表排序
            switch (selector.rangeSelectSortType)
            {
                case GameEntitySelectSortType.None:
                    break;
                case GameEntitySelectSortType.HPAsc:
                    list.Sort((a, b) =>
                    {
                        var va = a.attributeCom.GetValue(GameAttributeType.HP);
                        var vb = b.attributeCom.GetValue(GameAttributeType.HP);
                        return va.CompareTo(vb);
                    });
                    break;
                case GameEntitySelectSortType.HPDesc:
                    list.Sort((a, b) =>
                    {
                        var va = a.attributeCom.GetValue(GameAttributeType.HP);
                        var vb = b.attributeCom.GetValue(GameAttributeType.HP);
                        return vb.CompareTo(va);
                    });
                    break;
                default:
                    GameLogger.LogError($"未处理的选择器排序类型: {selector.rangeSelectSortType}");
                    break;
            }
            // 单位数量限制
            var rangeSelectLimitCount = selector.rangeSelectLimitCount;
            if (rangeSelectLimitCount != 0 && (list?.Count ?? 0) > rangeSelectLimitCount)
            {
                list = list.GetRange(0, rangeSelectLimitCount);
            }
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
            var isSingleSelect = selector.rangeSelectModel == null;
            var target = actor.actionTargeterCom.targetEntity;
            if (isSingleSelect)
            {
                switch (selectAnchorType)
                {
                    case GameEntitySelectAnchorType.Actor:
                        return actor.transformCom.position;
                    case GameEntitySelectAnchorType.ActorRole:
                        var actorRole = actor.GetLinkParent<GameRoleEntity>();
                        if (!actorRole)
                        {
                            GameLogger.LogError($"选择器锚点类型为ActorRole时, 未找到角色实体: {actor.idCom}");
                            return default;
                        }
                        return actorRole.transformCom.position;
                    case GameEntitySelectAnchorType.ActTarget:
                        return target.transformCom.position;
                    case GameEntitySelectAnchorType.NearestEnemy:
                        var enemy = this._context.roleContext.repo.GetNearestEnemy(actor.GetLinkParent<GameRoleEntity>());
                        if (!enemy)
                        {
                            GameLogger.LogError($"选择器锚点类型为NearestEnemy时, 未找到敌方实体: {actor.idCom}");
                            return default;
                        }
                        return enemy.transformCom.position;
                    default:
                        GameLogger.LogError($"未处理的选择器锚点类型: {selectAnchorType}");
                        return default;
                }
            }
            var anchorTrans = this._GetRangeSelectAnchorTrans(actor, target, selectAnchorType);
            return anchorTrans.position;
        }

        public bool CheckSelectorAnchor(GameEntityBase actor, GameEntitySelector selector)
        {
            if (actor == null) return false;
            if (selector == null) return false;

            var selectAnchorType = selector.selectAnchorType;
            var isSingleSelect = selector.rangeSelectModel == null;
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