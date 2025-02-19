using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameRoleModel
    {
        public readonly int typeId;
        public readonly int[] skillIds;
        public readonly GameAttribute[] attributes;
        public readonly GameRoleCareerType careerType;

        public GameRoleModel(int typeId, int[] skillIds, GameAttribute[] attributes, GameRoleCareerType careerType)
        {
            this.typeId = typeId;
            this.skillIds = skillIds;
            this.attributes = attributes;
            this.careerType = careerType;

            int stalkerSkillId = 0;
            switch (careerType)
            {
                case GameRoleCareerType.Ranger:
                    stalkerSkillId = 1000101;
                    break;
                case GameRoleCareerType.Warrior:
                    stalkerSkillId = 2000101;
                    break;
                case GameRoleCareerType.Tank:
                    stalkerSkillId = 3000101;
                    break;
                case GameRoleCareerType.Stalker:
                    stalkerSkillId = 4000101;
                    break;
                case GameRoleCareerType.Priest:
                    stalkerSkillId = 5000101;
                    break;
                default:
                    GameLogger.LogError($"GameRoleModel: careerType {careerType} 未处理 角色Id {typeId}");
                    break;
            }
            if (stalkerSkillId != 0)
            {
                this.skillIds = this.skillIds.Contact(stalkerSkillId);
            }
        }
    }
}