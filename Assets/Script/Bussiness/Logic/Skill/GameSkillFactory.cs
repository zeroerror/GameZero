using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameSkillFactory
    {
        public GameSkillTemplate template { get; private set; }
        public GameSkillFactory()
        {
            template = new GameSkillTemplate();
        }

        public GameSkillEntity Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameSkillFactory.Load: typeId not found: " + typeId);
                return null;
            }
            var skill = new GameSkillEntity(model);
            return skill;
        }
    }
}