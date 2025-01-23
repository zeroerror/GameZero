using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;

namespace GamePlay.Bussiness.UI
{
    public class UIDomainApi
    {
        public UIDirectDomainApi directApi { get; private set; }
        public void SetDirectDomainApi(UIDirectDomainApi directDomainApi) => this.directApi = directDomainApi;

        public UIJumpTextDomainApi jumpTextApi { get; private set; }
        public void SetJumpTextDomainApi(UIJumpTextDomainApi jumpTextDomainApi) => this.jumpTextApi = jumpTextDomainApi;

        public UILayerDomainApi layerApi { get; private set; }
        public void SetLayerApi(UILayerDomainApi layerDomainApi) => this.layerApi = layerDomainApi;

        public GameDomainApiR rendererApi { get; private set; }
        public void SetRendererApi(GameDomainApiR rendererApi) => this.rendererApi = rendererApi;

        public GameDomainApi logicApi { get; private set; }
        public void SetLogicApi(GameDomainApi logicApi) => this.logicApi = logicApi;

        public UIDomainApi()
        {
        }
    }
}