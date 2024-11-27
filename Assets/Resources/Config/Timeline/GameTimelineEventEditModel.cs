namespace GamePlay.Config
{
    [System.Serializable]
    public struct GameTimelineEventEditModel
    {
        public float time;
        public int frame;
        public GameActionSO action;
    }
}