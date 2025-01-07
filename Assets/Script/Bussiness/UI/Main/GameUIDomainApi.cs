using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;

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

        public GameDomainApiR rendererApi { get; private set; }
        public void SetRendererApi(GameDomainApiR rendererApi) => this.rendererApi = rendererApi;

        public GameDomainApi logicApi { get; private set; }
        public void SetLogicApi(GameDomainApi logicApi) => this.logicApi = logicApi;

        public GameUIDomainApi()
        {
        }
    }
}