namespace GamePlay.Bussiness.Logic
{
    public class GameProjectileStateDomain_Explode : GameProjectileStateDomainBase
    {
        public override bool CheckEnter(GameProjectileEntity entity)
        {
            return true;
        }

        public override void Enter(GameProjectileEntity entity)
        {
            var fsmCom = entity.fsmCom;
            fsmCom.EnterExplode();
            // 提交RC
            this._context.SubmitRC(GameProjectileRCCollection.RC_GAME_PROJECTILE_STATE_ENTER_EXPLODE, new GameProjectileRCArgs_StateEnterExplode
            {
                fromStateType = fsmCom.stateType,
                idArgs = entity.idCom.ToArgs(),
            });
        }

        protected override void _Tick(GameProjectileEntity entity, float frameTime)
        {
            throw new System.NotImplementedException();
        }
    }

}