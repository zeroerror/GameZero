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

            this._context.domainApi.fieldApi.ClearField(curField);
            fsmCom.EnterLoading(loadFieldId);

            this._context.domainApi.roleApi.CreatePlayerRole(101, new GameTransformArgs
            {
                position = new GameVec2(0, -5),
                scale = GameVec2.one,
                forward = GameVec2.right
            }, true);
            GameLogger.DebugLog("导演 - 进入加载状态 " + loadFieldId);
        }

        protected override void _Tick(GameDirectorEntity director, float frameTime)
        {
            this._directorDomain.TickDomain(frameTime);
        }

        protected override (GameDirectorStateType, object) _CheckExit(GameDirectorEntity director)
        {
            var curField = this._context.fieldContext.curField;
            if (curField == null) return (GameDirectorStateType.None, null);
            if (curField.model.typeId == director.fsmCom.loadingState.loadFieldId)
            {
                return (GameDirectorStateType.FightPreparing, null);
            }
            return (GameDirectorStateType.None, null);
        }
    }
}