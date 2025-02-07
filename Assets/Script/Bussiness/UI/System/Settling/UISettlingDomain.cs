using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.UI
{
    public class UISettlingDomain : UISystemDomainBase, UISettlingDomainApi
    {
        public UISettlingModel model { get; private set; }

        public UISettlingDomain()
        {
        }

        protected override void _BindEvents()
        {
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_SETTLING, this._OnStateEnter_Settling);
            this._context.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_EXIT_SETTLING, this._OnStateExit_Settling);
        }

        protected override void _UnbindEvents()
        {
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_ENTER_SETTLING, this._OnStateEnter_Settling);
            this._context.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_EXIT_SETTLING, this._OnStateExit_Settling);
        }

        private void _OnStateEnter_Settling(object args)
        {
            // TODO 弹出结算界面, 点击结算界面的关闭按钮, 退出结算状态

            // 2s后自定请求退出结算
            this._context.cmdBufferService.AddDelayCmd(2, () =>
            {
                var lcArgs = new GameLCArgs_PreparingConfirmFight { };
                var ldirectorApi = this._context.logicApi.directorApi;
                ldirectorApi.SubmitEvent(GameLCCollection.LC_GAME_SETTLING_CONFIRM_EXIT, lcArgs);
            });
        }

        private void _OnStateExit_Settling(object args)
        {
            // TODO 结算状态正确关闭后, 关闭结算界面
            // this._context.domainApi.directorApi.CloseUI(XXXXXX);
        }
    }
}