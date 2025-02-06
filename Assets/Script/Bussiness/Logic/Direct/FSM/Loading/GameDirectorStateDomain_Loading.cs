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
            if (!curField || curField.model.typeId != loadFieldId)
            {
                this._context.domainApi.fieldApi.DestroyField(curField);
                curField = this._context.domainApi.fieldApi.LoadField(loadFieldId);
            }

            // 记录加载下一场景前玩家棋子属性
            var unitEntitys = director.unitEntitys;
            unitEntitys?.ForEach((unitEntity) =>
            {
                var entityId = unitEntity.entityId;
                if (entityId == 0) return;
                var role = this._context.domainApi.roleApi.FindByEntityId(entityId);
                if (role == null) return;
                unitEntity.position = role.transformCom.position;
                unitEntity.attributeArgs = role.attributeCom.ToArgs();
                unitEntity.baseAttributeArgs = role.baseAttributeCom.ToArgs();
            });

            // 清理当前场景
            this._context.domainApi.fieldApi.ClearField(curField);
            fsmCom.EnterLoading(loadFieldId);

            // 生成玩家棋子
            unitEntitys?.ForEach((unitEntity) =>
            {
                this._context.domainApi.directApi.CreateUnit(unitEntity);
            });
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