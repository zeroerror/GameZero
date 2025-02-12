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
            roleSO?.Save();
        }
    }
}