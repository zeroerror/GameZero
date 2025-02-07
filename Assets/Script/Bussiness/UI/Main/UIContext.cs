using System;
using System.Collections.Generic;
using GamePlay.Bussiness.Core;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UIContext
    {
        public GameDomainApi logicApi { get; private set; }
        public GameDomainApiR rendererApi { get; private set; }

        public GameEventService eventService { get; private set; }
        public GameEventService delayRCEventService { get; private set; }

        public Dictionary<string, UIBase> uiDict { get; private set; }
        public UIFactory factory { get; private set; }

        public UIDirector director { get; private set; }
        public GameObject uiRoot { get; private set; }
        public Camera uiCamera { get; private set; }
        public UIDomainApi domainApi { get; private set; }
        public GameCmdBufferService cmdBufferService { get; private set; }
        public GameInputService inputService { get; private set; }

        public Dictionary<UILayerType, GameObject> layerDict { get; private set; }

        public UIContext()
        {
            this.eventService = new GameEventService();
            this.delayRCEventService = new GameEventService();
            this.director = new UIDirector();
            this.uiDict = new Dictionary<string, UIBase>();
            this.factory = new UIFactory();
            this.domainApi = new UIDomainApi();
            this.cmdBufferService = new GameCmdBufferService();
            this.inputService = new GameInputService();
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

            this.layerDict = new Dictionary<UILayerType, GameObject>();
            var enumvalus = Enum.GetValues(typeof(UILayerType));
            foreach (UILayerType type in enumvalus)
            {
                var layerGO = new GameObject(type.ToString());
                layerGO.transform.SetParent(uiRoot.transform, false);
                this.layerDict[type] = layerGO;
            }
        }

        public void BindRC(string rcName, Action<object> callback)
        {
            this.logicApi.directorApi.BindRC(rcName, callback);
            this.delayRCEventService.Bind(rcName, callback);
        }

        public void UnbindRC(string rcName, Action<object> callback)
        {
            this.logicApi.directorApi.UnbindRC(rcName, callback);
            this.delayRCEventService.Unbind(rcName, callback);
        }

        public void DelayRC(string rcName, object args)
        {
            this.delayRCEventService.Submit(rcName, args);
        }
    }
}