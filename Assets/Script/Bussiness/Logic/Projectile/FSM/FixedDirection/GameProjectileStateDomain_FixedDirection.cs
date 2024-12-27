using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_FixedDirection : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity projectile)
        {
            var fsmCom = projectile.fsmCom;
            fsmCom.EnterFixedDirection();
            var direction = projectile.actionTargeterCom.targetDirection;
            projectile.FaceTo(direction);
            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_FIXED_DIRECTION, new GameProjectileRCArgs_StateEnterFixedDirection
            {
                fromStateType = fsmCom.stateType,
                idArgs = projectile.idCom.ToArgs(),
                direction = direction,
            });
        }

        protected override void _Tick(GameProjectileEntity projectile, float frameTime)
        {
            var fixedDirectionState = projectile.fsmCom.fixedDirectionState;
            fixedDirectionState.stateTime += frameTime;

            var model = projectile.fsmCom.fixedDirectionState.model;
            var speed = model.speed;
            var direction = projectile.actionTargeterCom.targetDirection;
            GameLogger.LogWarning("direction: " + direction);
            var delta = direction * speed * frameTime;
            projectile.transformCom.position += delta;
            projectile.FaceTo(direction);

            // 反弹检测
            if (model.bounceCount > 0 && fixedDirectionState.bounceCount < model.bounceCount)
            {
                var entitySelectApi = this._context.domainApi.entitySelectApi;
                var checkEntitySelector = model.checkEntitySelector;
                var selectedEntities = entitySelectApi.SelectEntities(checkEntitySelector, projectile, false);
                if (selectedEntities?.Count > 0)
                {
                    var selectedEntity = selectedEntities[0];
                    var normal = GamePhysicsResolvingUtil.GetResolvingMTV(selectedEntity.physicsCom.collider, checkEntitySelector.colliderModel, projectile.transformCom.ToArgs()).normalized;
                    var reflectDirection = GameVectorUtil.GetReflectDirection(direction, normal);

                    projectile.FaceTo(reflectDirection);
                    var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(selectedEntity.physicsCom.collider, checkEntitySelector.colliderModel, projectile.transformCom.ToArgs());
                    projectile.transformCom.position += mtv + mtv.normalized * 0.01f;

                    var curTargeter = projectile.actionTargeterCom.getCurTargeter();
                    curTargeter.targetDirection = reflectDirection;
                    projectile.actionTargeterCom.SetTargeter(curTargeter);
                    projectile.physicsCom.ClearCollided();
                }
            }
        }

        protected override GameProjectileStateType _CheckExit(GameProjectileEntity projectile)
        {
            // 不存在持续一定时间后切换状态的情况, 默认一定时间后销毁
            var triggerSet = projectile.fsmCom.triggerSetEntityDict[GameProjectileStateType.FixedDirection];
            var durationTriggerEntity = triggerSet.durationTriggerEntity;
            if (durationTriggerEntity == null || durationTriggerEntity.model.duration <= 0)
            {
                var stateTime = projectile.fsmCom.fixedDirectionState.stateTime;
                if (stateTime >= 3)
                {
                    return GameProjectileStateType.Destroyed;
                }
            }

            return base._CheckExit(projectile);
        }
    }

}