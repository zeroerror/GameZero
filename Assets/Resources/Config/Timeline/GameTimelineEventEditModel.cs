using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameTimelineEventEditModel
    {
        public float time;
        public int frame;
        public GameActionSO action;
    }

    /// ext
    public static class GameTimelineEventEditModelExt
    {
        public static GameTimelineEventModel ToModel(this GameTimelineEventEditModel em)
        {
            GameTimelineEventModel m;
            m.time = em.time;
            m.frame = em.frame;
            m.actionId = em.action.typeId;
            return m;
        }

        public static GameTimelineEventModel[] ToModels(this GameTimelineEventEditModel[] ems)
        {
            if (ems == null) return null;
            GameTimelineEventModel[] ms = new GameTimelineEventModel[ems.Length];
            for (int i = 0; i < ems.Length; i++)
            {
                ms[i] = ems[i].ToModel();
            }
            return ms;
        }
    }
}