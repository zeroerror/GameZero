namespace GamePlay.Bussiness.Logic
{
    public class GameActionOptionRepo : GameEntityRepoBase<GameActionOptionEntity>
    {
        public GameActionOptionEntity FindOption(int campId, int optionId)
        {
            return this._list.Find(x => x.idCom.campId == campId && x.model.typeId == optionId);
        }
    }
}