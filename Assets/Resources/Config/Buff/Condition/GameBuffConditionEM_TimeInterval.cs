using GamePlay.Bussiness.Logic;
using UnityEngine;
namespace GamePlay.Config
{
    [System.Serializable]
    public class GameBuffConditionEM_TimeInterval
    {
        public bool isEnable;
        public float timeInterval;

        public GameBuffConditionModel_TimeInterval ToModel()
        {
            return new GameBuffConditionModel_TimeInterval(this.timeInterval);
        }
    }
}