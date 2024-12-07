using UnityEngine;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using GamePlay.Core;

public class GameEntry : MonoBehaviour
{
    public GameApp gameApp { get; private set; }
    public GameAppR gameAppR { get; private set; }
    void Start()
    {
        this.gameApp = new GameApp();
        this.gameAppR = new GameAppR(gameApp.directDomain.context, this.gameObject);
        GameLogger.logLevel = LogLevel.Error;
    }

    void Update()
    {
        this.gameApp.Update(Time.deltaTime);
        this.gameAppR.Update(Time.deltaTime);
    }

    void LateUpdate()
    {
        this.gameApp.LateUpdate(Time.deltaTime);
        this.gameAppR.LateUpdate(Time.deltaTime);
    }
}
