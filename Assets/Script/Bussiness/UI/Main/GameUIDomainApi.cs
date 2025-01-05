namespace GamePlay.Bussiness.UI
{
    public class GameUIDomainApi
    {
        public GameUIDirectDomainApi directApi { get; private set; }
        public void SetDirectDomainApi(GameUIDirectDomainApi directDomainApi) => this.directApi = directDomainApi;

        public GameUIJumpTextDomainApi jumpTextApi { get; private set; }
        public void SetJumpTextDomainApi(GameUIJumpTextDomainApi jumpTextDomainApi) => this.jumpTextApi = jumpTextDomainApi;

        public GameUILayerDomainApi layerApi { get; private set; }
        public void SetLayerApi(GameUILayerDomainApi layerDomainApi) => this.layerApi = layerDomainApi;

        public GameUIDomainApi()
        {
        }
    }
}