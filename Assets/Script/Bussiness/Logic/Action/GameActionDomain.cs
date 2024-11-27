using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameActionDomain : GameActionDomainApi
    {

        GameContext _context;

        public GameActionDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void DoAction(int actionId, GameEntityBase actorEntity)
        {
            GameLogger.Log($"执行行为：{actionId} 执行者：{actorEntity.idCom}");
        }

        public void DoAction_Dmg(GameAction_Dmg dmgAction, GameActionBase actorEntity)
        {
            throw new System.NotImplementedException();
        }

        public void DoAction_Heal(GameAction_Heal healAction, GameActionBase actorEntity)
        {
            throw new System.NotImplementedException();
        }

        public void DoAction_LaunchBullet(GameAction_LaunchBullet launchBulletAction, GameActionBase actorEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}