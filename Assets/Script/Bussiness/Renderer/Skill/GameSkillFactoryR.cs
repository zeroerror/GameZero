using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameSkillFactoryR
    {
        public GameSkillTemplateR template { get; private set; }
        public GameSkillFactoryR()
        {
            this.template = new GameSkillTemplateR();
        }

        public GameSkillEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameSkillFactoryR.Load: typeId not found: " + typeId);
                return null;
            }
            var skill = new GameSkillEntityR(model);
            return skill;
        }
    }
}