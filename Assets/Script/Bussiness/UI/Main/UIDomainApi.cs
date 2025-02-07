using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;

namespace GamePlay.Bussiness.UI
{
    public class UIDomainApi
    {
        public GameDomainApi logicApi { get; private set; }
        public GameDomainApiR rendererApi { get; private set; }

        public UIDirectorDomainApi directorApi { get; private set; }

        public UIJumpTextDomainApi jumpTextApi { get; private set; }

        public UILayerDomainApi layerApi { get; private set; }

        public UIPlayerDomainApi playerApi { get; private set; }

        public UIActionOptionDomainApi actionOptionApi { get; private set; }
        public UISettlingDomainApi settlingApi { get; private set; }
        public UIUnitShopDomainApi unitShopApi { get; private set; }

        public void InjectApis(
            GameDomainApi logicApi,
            GameDomainApiR rendererApi,
            UIDirectorDomainApi directorApi,
            UIJumpTextDomainApi jumpTextApi,
            UILayerDomainApi layerApi,
            UIPlayerDomainApi playerApi,
            UIActionOptionDomainApi actionOptionApi,
            UISettlingDomainApi settlingApi,
            UIUnitShopDomainApi unitShopApi
        )
        {
            this.logicApi = logicApi;
            this.rendererApi = rendererApi;
            this.directorApi = directorApi;
            this.jumpTextApi = jumpTextApi;
            this.layerApi = layerApi;
            this.playerApi = playerApi;
            this.actionOptionApi = actionOptionApi;
            this.settlingApi = settlingApi;
            this.unitShopApi = unitShopApi;
        }
    }
}