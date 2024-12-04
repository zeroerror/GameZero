using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_Attach : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity projectile)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity projectile)
        {
            var fsmCom = projectile.fsmCom;
            var targeter = projectile.actionTargeterCom.getCurTargeter();
            fsmCom.EnterAttach(targeter);
            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_ATTACH, new GameProjectileRCArgs_StateEnterAttach
            {
                fromStateType = fsmCom.stateType,
                idArgs = projectile.idCom.ToArgs(),
                pos = targeter.targetPosition,
                targetIdArgs = targeter.targetEntity.idCom.ToArgs()
            });
        }

        protected override void _Tick(GameProjectileEntity projectile, float frameTime)
        {
            var targeter = projectile.actionTargeterCom.getCurTargeter();
            var attachPos = projectile.transformCom.position;
            var fsmCom = projectile.fsmCom;
            var stateModel = fsmCom.attachState.model;
            switch (stateModel.attachType)
            {
                case GameProjectileTargeterType.Actor:
                    var actor = projectile.idCom.parent;
                    if (actor.attributeCom.GetValue(GameAttributeType.Hp) <= 0) break;
                    attachPos = actor.transformCom.position;
                    break;
                case GameProjectileTargeterType.Target:
                    var targetEntity = targeter.targetEntity;
                    if (targetEntity.attributeCom.GetValue(GameAttributeType.Hp) <= 0) break;
                    attachPos = targetEntity.transformCom.position;
                    break;
                case GameProjectileTargeterType.Position:
                    attachPos = targeter.targetPosition;
                    break;
                default:
                    GameLogger.LogError($"投射物附着类型未处理：{stateModel.attachType}");
                    break;
            }
            projectile.transformCom.position = attachPos;
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity projectile)
        {
            var fsmCom = projectile.fsmCom;
            var stateModel = fsmCom.attachState.model;
            if (stateModel.attachType == GameProjectileTargeterType.Actor)
            {
                var actor = projectile.idCom.parent;
                if (actor.attributeCom.GetValue(GameAttributeType.Hp) <= 0) return GameProjectileStateType.Destroyed;
            }
            else if (stateModel.attachType == GameProjectileTargeterType.Target)
            {
                var targetEntity = projectile.actionTargeterCom.getCurTargeter().targetEntity;
                if (targetEntity.attributeCom.GetValue(GameAttributeType.Hp) <= 0) return GameProjectileStateType.Destroyed;
            }
            return base._CheckExit(projectile);
        }
    }

}