using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionUtil_Stealth
    {
        public static GameActionRecord_Stealth CalcStealth(GameEntityBase actor, GameEntityBase target, GameActionModel_Stealth model)
        {
            var record = new GameActionRecord_Stealth(
                model.typeId,
                actor.GetLinkParent<GameRoleEntity>()?.idCom.ToArgs() ?? default,
                actor.idCom.ToArgs(),
                target.idCom.ToArgs(),
                actor.actionTargeterCom.getCurTargeterAsRecord(),
                model.duraiton
            );
            return record;
        }

        public static void DoStealth(GameEntityBase target, GameActionRecord_Stealth record, GameDomainApi domainApi)
        {
            if (target is GameRoleEntity role)
            {
                domainApi.roleApi.fsmApi.TryEnter(role, GameRoleStateType.Stealth, record.duraiton);
                return;
            }
            GameLogger.LogError($"DoStealth: 执行隐身行为实体类型{target.idCom.entityType}不支持");
        }
    }
}