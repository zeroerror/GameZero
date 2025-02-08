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
            var unitEntitys = director.unitEntitys;
            unitEntitys?.ForEach((unitEntity) =>
            {
                var unit = this._context.domainApi.directorApi.FindUnit(unitEntity);
                if (unit == null) return;
                unitEntity.attributeArgs = unit.attributeCom.ToArgs();
                unitEntity.baseAttributeArgs = unit.baseAttributeCom.ToArgs();
            });

            // 清理当前场景 或 清理存活单位身上的buff
            if (needLoad)
            {
                this._context.domainApi.fieldApi.ClearField(curField);
            }
            else
            {
                this._context.domainApi.roleApi.ForeachAllRoles((role) =>
                {
                    this._context.domainApi.buffApi.DetachAllBuffs(role);
                });
            }

            // 生成玩家棋子
            unitEntitys?.ForEach((unitEntity) =>
            {
                var unit = this._context.domainApi.directorApi.CreateUnit(unitEntity);
                unit.attributeCom.SetByArgs(unitEntity.attributeArgs);
                unit.baseAttributeCom.SetByArgs(unitEntity.baseAttributeArgs);
                unit.transformCom.position = unitEntity.standPos;
            });

            // 切换状态机组件状态
            fsmCom.EnterLoading(loadFieldId);
            GameLogger.DebugLog("导演 - 进入加载状态 " + loadFieldId);
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
    }
}