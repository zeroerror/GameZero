using UnityEngine;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Render;
using GamePlay.Bussiness.UI;
using GamePlay.Bussiness.Core;

public class GameEntry : MonoBehaviour
{
    public GameApp gameApp { get; private set; }
    public GameAppR gameAppR { get; private set; }
    public UIApp gameUIApp { get; private set; }

    void Start()
    {
        GameRandomService.DefaultInitSeeds();
        gameApp = new GameApp();
        gameAppR = new GameAppR();
        gameUIApp = new UIApp();

        // GameLogger.logLevel = LogLevel.Error;
        var logicApi = gameApp.directorDomain.context.domainApi;
        var rendererApi = gameAppR.directorDomain.context.domainApi;
        var uiApi = gameUIApp.directorDomain.context.uiApi;
        var sceneRoot = this.gameObject;
        var uiRoot = GameObject.Find("UIRoot");

        gameAppR.Inject(sceneRoot, logicApi, uiApi);
        gameUIApp.Inject(uiRoot, logicApi, rendererApi);

        gameApp.BindEvents();
        gameAppR.BindEvents();
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
