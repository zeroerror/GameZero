using UnityEngine;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;
using GamePlay.Bussiness.UI;

public class GameEntry : MonoBehaviour
{
    public GameApp gameApp { get; private set; }
    public GameAppR gameAppR { get; private set; }
    public GameUIApp gameUIApp { get; private set; }
    void Start()
    {
        gameUIApp = new GameUIApp(GameObject.Find("UIRoot"));
        gameApp = new GameApp();
        gameAppR = new GameAppR(gameApp.directDomain.context, gameObject, gameUIApp.directDomain.context);
        // GameLogger.logLevel = LogLevel.Error;
    }

    void Update()
    {
        gameApp.Update(Time.deltaTime);
        gameAppR.Update(Time.deltaTime);
        gameUIApp.Update(Time.deltaTime);
    }

    void LateUpdate()
    {
        gameApp.LateUpdate(Time.deltaTime);
        gameAppR.LateUpdate(Time.deltaTime);
        gameUIApp.LateUpdate(Time.deltaTime);
    }
}
