namespace GamePlay.Bussiness.UI
{
    public class GameUIDomainApi
    {
        public GameUIJumpTextDomainApi jumpTextApi { get; private set; }
        public void SetJumpTextDomainApi(GameUIJumpTextDomainApi jumpTextDomainApi) => this.jumpTextApi = jumpTextDomainApi;

        public GameUILayerDomainApi layerApi { get; private set; }
        public void SetLayerApi(GameUILayerDomainApi layerDomainApi) => this.layerApi = layerDomainApi;

        public GameUIDomainApi()
        {
        }
    }
}