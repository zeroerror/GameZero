using GamePlay.Config;

namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public struct GameTimelineEventModel
    {
        public float time;
        public int frame;
        public int actionId;

        public static GameTimelineEventModel ConvertToModel(in GameTimelineEventEditModel em)
        {
            GameTimelineEventModel m;
            m.time = em.time;
            m.frame = em.frame;
            m.actionId = em.action.typeId;
            return m;
        }

        public static GameTimelineEventModel[] ConvertToModels(GameTimelineEventEditModel[] ems)
        {
            if (ems == null) return null;
            GameTimelineEventModel[] ms = new GameTimelineEventModel[ems.Length];
            for (int i = 0; i < ems.Length; i++)
            {
                ms[i] = ConvertToModel(ems[i]);
            }
            return ms;
        }
    }
}