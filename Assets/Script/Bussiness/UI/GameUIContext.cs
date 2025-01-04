using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class GameUIContext
    {
        public GameUIDirector director { get; private set; }
        public GameObject uiRoot { get; private set; }
        public Camera uiCamera { get; private set; }
        public GameUIDomainApi domainApi { get; private set; }
        public GameCmdBufferService cmdBufferService { get; private set; }

        public GameDomainApi logicApi { get; private set; }
        public GameDomainApiR rendererApi { get; private set; }

        public GameUIContext()
        {
            this.director = new GameUIDirector();
            this.domainApi = new GameUIDomainApi();
            this.cmdBufferService = new GameCmdBufferService();
        }

        public void Inject(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this.uiRoot = uiRoot;
            this.uiCamera = GameObject.Find("UICamera")?.GetComponent<Camera>();
            Debug.Assert(this.uiCamera != null, "UICamera not found");
            this.logicApi = logicApi;
            this.rendererApi = rendererApi;
        }
    }
}