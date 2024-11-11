using System;
using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    // 实体类型枚举
    [Flags]  // 使用 Flags 属性，表示该枚举是位标志类型
    public enum GameEntityType
    {
        None = 0,
        Role = 1 << 0,   // 1
        Skill = 1 << 1,  // 2
        Bullet = 1 << 2, // 4
        Buff = 1 << 3    // 8
    }

    // GameEntityType 扩展类
    public static class GameEntityTypeExtension
    {
        // 是否包含实体类型
        public static bool IsContains(GameEntityType entityType, GameEntityType entityTypeToCheck)
        {
            return (entityType & entityTypeToCheck) != 0;
        }

        // 添加实体类型
        public static GameEntityType Add(GameEntityType entityType, GameEntityType entityTypeToAdd)
        {
            return entityType | entityTypeToAdd;
        }

        // 移除实体类型
        public static GameEntityType Remove(GameEntityType entityType, GameEntityType entityTypeToRemove)
        {
            return entityType & ~entityTypeToRemove;
        }

        // 获取拆分后的实体类型列表
        public static List<GameEntityType> GetSplitList(GameEntityType entityType)
        {
            List<GameEntityType> list = new List<GameEntityType>();
            for (int i = 0; i < 32; i++)
            {
                GameEntityType type = (GameEntityType)(1 << i);
                if (IsContains(entityType, type))
                {
                    list.Add(type);
                }
            }
            return list;
        }

        // 转换为字符串
        public static string ToString(GameEntityType entityType)
        {
            switch (entityType)
            {
                case GameEntityType.Role:
                    return "角色";
                case GameEntityType.Skill:
                    return "技能";
                case GameEntityType.Bullet:
                    return "弹体";
                case GameEntityType.Buff:
                    return "buff";
                default:
                    return "未知";
            }
        }
    }
}
