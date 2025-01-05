using System;
using System.Collections.Generic;
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

        public Dictionary<GameUILayerType, GameObject> layerDict { get; private set; }

        public GameUIContext()
        {
            this.director = new GameUIDirector();
            this.domainApi = new GameUIDomainApi();
            this.cmdBufferService = new GameCmdBufferService();
        }

        public void Inject(GameObject uiRoot, GameDomainApi logicApi, GameDomainApiR rendererApi)
        {
            this.uiRoot = uiRoot;
            this._createLayerDict(uiRoot);
            this.uiCamera = GameObject.Find("UICamera")?.GetComponent<Camera>();
            Debug.Assert(this.uiCamera != null, "UICamera not found");
            this.logicApi = logicApi;
            this.rendererApi = rendererApi;
        }

        private void _createLayerDict(GameObject uiRoot)
        {
            if (this.layerDict != null)
            {
                foreach (var layer in this.layerDict)
                {
                    GameObject.Destroy(layer.Value);
                }
            }

            this.layerDict = new Dictionary<GameUILayerType, GameObject>();
            var enumvalus = Enum.GetValues(typeof(GameUILayerType));
            foreach (GameUILayerType type in enumvalus)
            {
                var layerGO = new GameObject(type.ToString());
                layerGO.transform.SetParent(uiRoot.transform, false);
                this.layerDict[type] = layerGO;
            }
        }
    }
}