namespace GamePlay.Bussiness.Logic
{
    public class GameSKillDashSpeedModel
    {
        public readonly int frame;
        public readonly float speed;

        /// <summary> 是否为变量点 </summary>
        public readonly bool isVariablePoint;

        public GameSKillDashSpeedModel(int frame, float speed, bool isVariablePoint = false)
        {
            this.frame = frame;
            this.speed = speed;
            this.isVariablePoint = isVariablePoint;
        }
    }
}