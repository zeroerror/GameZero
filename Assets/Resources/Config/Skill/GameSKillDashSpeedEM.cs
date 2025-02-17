using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameSKillDashSpeedEM
    {
        public float time;
        public int frame;
        public float x;
        public float distanceRatio;
    }

    public static class GameSKillDashSpeedEMExt
    {
        public static GameSKillDashSpeedModel ToModel(this GameSKillDashSpeedEM em)
        {
            return new GameSKillDashSpeedModel(em.frame, em.distanceRatio);
        }

        public static GameSKillDashSpeedModel[] ToModels(this GameSKillDashSpeedEM[] ems)
        {
            var models = new GameSKillDashSpeedModel[ems.Length];
            for (int i = 0; i < ems.Length; i++)
            {
                models[i] = ems[i].ToModel();
            }
            return models;
        }
    }
}