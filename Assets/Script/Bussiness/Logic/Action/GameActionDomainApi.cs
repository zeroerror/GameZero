namespace GamePlay.Bussiness.Logic
{
    public interface GameActionDomainApi
    {

        /// <summary>
        /// 执行行为 
        /// actionId: 行为ID 
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction(int actionId, GameEntityBase actorEntity);

        /// <summary>
        /// 执行伤害行为
        /// dmgAction: 伤害行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_Dmg(GameAction_Dmg dmgAction, GameActionBase actorEntity);

        /// <summary>
        /// 执行治疗行为
        /// healAction: 治疗行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_Heal(GameAction_Heal healAction, GameActionBase actorEntity);

        /// <summary>
        /// 执行发射子弹行为
        /// launchBulletAction: 发射子弹行为
        /// actorEntity: 执行者
        /// </summary>
        public void DoAction_LaunchBullet(GameAction_LaunchBullet launchBulletAction, GameActionBase actorEntity);
    }
}