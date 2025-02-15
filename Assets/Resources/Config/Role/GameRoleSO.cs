using System.Linq;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Render;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_role_", menuName = "游戏玩法/配置/角色模板")]
    public class GameRoleSO : GameSOBase
    {
        public string roleName;
        public string desc;
        public GameObject prefab;
        public string prefabUrl;
        public GameSkillSO[] skills;
        public GameAttribute[] attributes;
        public GameRoleCareerType careerType;
        public bool isMultyAnimationLayer;

        public GameRoleModel ToModel()
        {
            var skillIds = skills?.Select(s => s.typeId).ToArray();
            var model = new GameRoleModel(typeId, skillIds, attributes, careerType);
            return model;
        }

        public GameRoleModelR ToModelR()
        {
            var model = new GameRoleModelR(this.typeId, roleName, desc, prefabUrl, skills, isMultyAnimationLayer);
            return model;
        }

        public void Save()
        {
            if (!prefab)
            {
                Debug.LogError("预制体为空");
                return;
            }
            if (prefabUrl != prefab.GetPrefabUrl())
            {
                Debug.Log($"预制体更新: {prefabUrl} => {prefab.GetPrefabUrl()}");
                prefabUrl = prefab.GetPrefabUrl();
            }
            Debug.Log($"保存角色数据 - {typeId} ✔");
            this?.skills.Foreach(skillSO => skillSO.Save());
            Debug.Log("保存技能列表数据 ✔");
        }

    }
}
