namespace GamePlay.Bussiness.UI
{
    public class GameUIDomainApi
    {

        public GameUIJumpTextDomainApi jumpTextDomainApi { get; private set; }
        public void SetJumpTextDomainApi(GameUIJumpTextDomainApi jumpTextDomainApi) => this.jumpTextDomainApi = jumpTextDomainApi;

        public GameUIDomainApi()
        {
        }
    }
}