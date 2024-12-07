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
                case GameProjectileTargeterType.RoleActor:
                    var roleActor = projectile.TryGetLinkEntity<GameRoleEntity>();
                    if (roleActor.attributeCom.GetValue(GameAttributeType.HP) <= 0) break;
                    attachPos = roleActor.transformCom.position;
                    projectile.transformCom.forward = roleActor.transformCom.forward;
                    projectile.actionTargeterCom.SetTargeter(new GameActionTargeterArgs
                    {
                        targetDirection = roleActor.transformCom.forward,
                    });
                    break;
                case GameProjectileTargeterType.Target:
                    var targetEntity = targeter.targetEntity;
                    if (targetEntity.attributeCom.GetValue(GameAttributeType.HP) <= 0) break;
                    attachPos = targetEntity.transformCom.position;
                    projectile.transformCom.forward = targetEntity.transformCom.forward;
                    break;
                case GameProjectileTargeterType.Position:
                    attachPos = targeter.targetPosition;
                    break;
                default:
                    GameLogger.LogError($"投射物附着类型未处理：{stateModel.attachType}");
                    break;
            }
            attachPos += stateModel.attachOffset;
            projectile.transformCom.position = attachPos;
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity projectile)
        {
            var fsmCom = projectile.fsmCom;
            var stateModel = fsmCom.attachState.model;
            if (stateModel.attachType == GameProjectileTargeterType.RoleActor)
            {
                var actor = projectile.TryGetLinkEntity<GameRoleEntity>();
                if (actor.attributeCom.GetValue(GameAttributeType.HP) <= 0) return GameProjectileStateType.Destroyed;
            }
            else if (stateModel.attachType == GameProjectileTargeterType.Target)
            {
                var targetEntity = projectile.actionTargeterCom.getCurTargeter().targetEntity;
                if (targetEntity.attributeCom.GetValue(GameAttributeType.HP) <= 0) return GameProjectileStateType.Destroyed;
            }
            return base._CheckExit(projectile);
        }
    }

}