using GamePlay.Bussiness.Logic;

namespace GamePlay.Config
{
    /** buff条件：当执行目标行为时 */
    [System.Serializable]
    public class GameBuffConditionEM_WhenDoAction
    {
        public bool isEnable;
        public GameActionSO targetAction;

        public GameBuffConditionModel_WhenDoAction ToModel()
        {
            if (!this.isEnable || this.targetAction == null)
            {
                return null;
            }
            return new GameBuffConditionModel_WhenDoAction(this.targetAction.typeId);
        }
    }
}