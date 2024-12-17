using UnityEditor;
using UnityEngine;

namespace GamePlay.Config
{
    public abstract class GameSOBase : ScriptableObject
    {
        [Header("类型Id")]
        public int typeId;
    }
}