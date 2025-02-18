using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public class GameSKillDashEM
    {
        public float time;
        public int frame;
        public float distanceRatio;
        public float x;
        public float y;
    }

    public static class GameSKillDashEMExt
    {
        public static GameSKillDashModel ToModel(this GameSKillDashEM em)
        {
            return new GameSKillDashModel(em.frame, em.distanceRatio, em.x, em.y);
        }

        public static GameSKillDashModel[] ToModels(this GameSKillDashEM[] ems)
        {
            var models = new GameSKillDashModel[ems.Length];
            for (int i = 0; i < ems.Length; i++)
            {
                models[i] = ems[i].ToModel();
            }
            return models;
        }
    }
}