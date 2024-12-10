namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public struct GameAttribute
    {
        public GameAttributeType type;
        public float value;

        public GameAttribute(GameAttributeType type, float value)
        {
            this.type = type;
            this.value = value;
        }
    }
}