using UnityEngine;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using GamePlay.Bussiness.UI;

public class GameEntry : MonoBehaviour
{
    public GameApp gameApp { get; private set; }
    public GameAppR gameAppR { get; private set; }
    public UIApp gameUIApp { get; private set; }
    void Start()
    {
        gameApp = new GameApp();
        gameAppR = new GameAppR();
        gameUIApp = new UIApp();

        // GameLogger.logLevel = LogLevel.Error;
        var logicApi = gameApp.directDomain.context.domainApi;
        var rendererApi = gameAppR.directDomain.context.domainApi;
        var uiApi = gameUIApp.directDomain.context.domainApi;
        var sceneRoot = this.gameObject;
        var uiRoot = GameObject.Find("UIRoot");

        gameAppR.Inject(sceneRoot, logicApi, uiApi);
        gameUIApp.Inject(uiRoot, logicApi, rendererApi);
    }

    void Update()
    {
        gameApp.Update(Time.deltaTime);
        gameAppR.Update(Time.deltaTime);
        gameUIApp.Update(Time.deltaTime);
    }

    void LateUpdate()
    {
        gameAppR.LateUpdate(Time.deltaTime);
        gameUIApp.LateUpdate(Time.deltaTime);
    }
}
