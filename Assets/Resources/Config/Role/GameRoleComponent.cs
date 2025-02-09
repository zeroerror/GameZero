using System.IO;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    public class GameRoleComponent : MonoBehaviour
    {
        public GameRoleSO roleSO;
        public void Save()
        {
            Debug.Log($"保存角色数据 - {roleSO?.typeId} ✔");
            roleSO?.skills.Foreach(skillSO => skillSO.Save());
            Debug.Log("保存技能列表数据 ✔");
        }
    }
}