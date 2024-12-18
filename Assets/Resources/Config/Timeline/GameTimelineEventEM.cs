using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameTimelineEventEM
    {
        public float time;
        public int frame;
        public GameActionSO[] actions;
    }

    public static class GameTimelineEventEMExt
    {
        public static GameTimelineEventModel ToModel(this GameTimelineEventEM em)
        {
            GameTimelineEventModel m;
            m.time = em.time;
            m.frame = em.frame;
            m.actionIds = em.actions.Map((a) => a.typeId);
            return m;
        }

        public static GameTimelineEventModel[] ToModels(this GameTimelineEventEM[] ems)
        {
            if (ems == null) return null;
            ems = ems.Filter(e => e.actions != null);
            GameTimelineEventModel[] ms = new GameTimelineEventModel[ems.Length];
            for (int i = 0; i < ems.Length; i++)
            {
                var em = ems[i];
                ms[i] = em.ToModel();
            }
            return ms;
        }
    }
}