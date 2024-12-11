namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public struct GameTimelineEventModel
    {
        public float time;
        public int frame;
        public int[] actionIds;
    }
}