using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameDirectorStateDomain_Loading : GameDirectorStateDomainBase
    {
        public GameDirectorStateDomain_Loading(GameDirectorDomain directorDomain) : base(directorDomain)
        {
        }

        public override bool CheckEnter(GameDirectorEntity director, object args = null)
        {
            return true;
        }

        public override void Enter(GameDirectorEntity director, object args = null)
        {
            var loadFieldId = (int)args;
            var curField = this._context.fieldContext.curField;
            var fsmCom = director.fsmCom;
            var loadingState = fsmCom.loadingState;
            loadingState.loadFieldId = loadFieldId;
            var needLoad = !curField || curField.model.typeId != loadFieldId;
            if (needLoad)
            {
                this._context.domainApi.fieldApi.DestroyField(curField);
                curField = this._context.domainApi.fieldApi.LoadField(loadFieldId);
            }

            // 记录加载下一场景前玩家棋子属性
            var unitEntitys = director.unitItemEntitys;
            unitEntitys?.ForEach((unitEntity) =>
            {
                var unit = this._context.domainApi.directorApi.FindUnitEntity(unitEntity);
                if (unit == null) return;
                unitEntity.attributeArgs = unit.attributeCom.ToArgs();
                unitEntity.baseAttributeArgs = unit.baseAttributeCom.ToArgs();
            });

            if (needLoad)
            {
                // 清理当前战场
                this._context.domainApi.directorApi.CleanBattleField();
            }
            else
            {
                this._context.domainApi.roleApi.ForeachAllRoles((role) =>
                {
                    var isBoughtRole = unitEntitys.Exists((unitEntity) => unitEntity.itemModel.entityType == GameEntityType.Role && unitEntity.entityId == role.idCom.entityId);
                    if (isBoughtRole)
                    {
                        // 移除购买的存活角色身上的buff
                        this._context.domainApi.buffApi.DetachAllBuffs(role);
                    }
                    else
                    {
                        // 摧毁非购买的角色
                        this._context.domainApi.roleApi.fsmApi.TryEnter(role, GameRoleStateType.Dead);
                    }
                });
                this._context.domainApi.projectileApi.ForeachAllProjectiles((projectile) =>
                {
                    var isBoughtProjectile = unitEntitys.Exists((unitEntity) => unitEntity.itemModel.entityType == GameEntityType.Projectile && unitEntity.entityId == projectile.idCom.entityId);
                    if (!isBoughtProjectile)
                    {
                        // 摧毁非购买的投射物, 不包括比如购买的陷阱等
                        this._context.domainApi.projectileApi.fsmApi.TryEnter(projectile, GameProjectileStateType.Destroyed);
                    }
                });
                // 重置当前场景的怪物生成状态
                curField.ResetMonsterSpawned();
            }

            // 生成玩家棋子
            var unitCount = unitEntitys?.Count ?? 0;
            for (int i = 0; i < unitCount; i++)
            {
                var unitEntity = unitEntitys[i];
                var directorApi = this._context.domainApi.directorApi;
                var unit = needLoad ? directorApi.CreateUnit(unitEntity) : directorApi.FindUnitEntity(unitEntity);
                if (!unit)
                {
                    // 移除已经被销毁的棋子
                    unitEntitys.RemoveAt(i);
                    i--;
                    continue;
                }
                unit.attributeCom.SetByArgs(unitEntity.attributeArgs);
                unit.baseAttributeCom.SetByArgs(unitEntity.baseAttributeArgs);
            }

            // 切换状态机组件状态
            fsmCom.EnterLoading(loadFieldId);
            GameLogger.DebugLog("导演 - 进入加载状态 " + loadFieldId);

            // 提交RC
            this._context.SubmitRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_LOADING, new GameDirectorRCArgs_StateEnterLoading { fieldId = loadFieldId });
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
            this._directorDomain.TickDomain(frameTime);
        }

        protected override GameDirectorExitStateArgs _CheckExit(GameDirectorEntity director)
        {
            var curField = this._context.fieldContext.curField;
            if (curField == null) return new GameDirectorExitStateArgs(GameDirectorStateType.None);
            if (curField.model.typeId == director.fsmCom.loadingState.loadFieldId)
            {
                return new GameDirectorExitStateArgs(GameDirectorStateType.FightPreparing);
            }
            return new GameDirectorExitStateArgs(GameDirectorStateType.None);
        }

        public override void ExitTo(GameDirectorEntity director, GameDirectorStateType toState)
        {
            base.ExitTo(director, toState);
            // 更新回合数
            director.curRound++;
            GameLogger.DebugLog("L 导演 - 进入第 " + director.curRound + " 回合");
        }
    }
}