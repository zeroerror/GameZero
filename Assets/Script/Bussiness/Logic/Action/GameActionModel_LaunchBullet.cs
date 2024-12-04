namespace GamePlay.Bussiness.Logic
{
    [System.Serializable]
    public class GameActionModel_LaunchProjectile : GameActionModelBase
    {
        public int projectileId;

        public override string ToString()
        {
            return $"发射投射物行为: 投射物Id:{this.projectileId}";
        }

    }
}