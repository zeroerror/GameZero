namespace GamePlay.Bussiness.Logic
{
    public class GameActionOptionRepo : GameEntityRepoBase<GameActionOptionEntity>
    {
        public GameActionOptionEntity FindByCampId(int campId)
        {
            return this._list.Find(x => x.idCom.campId == campId);
        }
    }
}