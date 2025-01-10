namespace GamePlay.Bussiness.Logic
{
    public class GameCharacterTransformCom
    {
        private GameRoleEntity _role;

        /// <summary> 模型 </summary>
        public GameRoleModel model { get; private set; }
        /// <summary> 技能组件 </summary>
        public GameSkillCom skillCom { get; private set; }

        public GameCharacterTransformCom(GameRoleEntity role)
        {
            this._role = role;
            this.skillCom = new GameSkillCom(_role);
        }

        public void StartTransform(GameRoleModel model)
        {
            this.model = model;
        }

        public void EndTransform()
        {
            this.model = null;
            this.skillCom.Clear();
        }
    }
}