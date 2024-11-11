using UnityEngine;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;

public class GameEntry : MonoBehaviour
{
    public GameCore gameCore { get; private set; }
    public GameCoreR gameCoreR { get; private set; }
    void Start()
    {
        this.gameCore = new GameCore();
        this.gameCoreR = new GameCoreR();
    }

    void Update()
    {
        this.gameCore.Update(Time.deltaTime);
        this.gameCoreR.Update(Time.deltaTime);
    }

    void LateUpdate()
    {
        this.gameCore.LateUpdate(Time.deltaTime);
        this.gameCoreR.LateUpdate(Time.deltaTime);
    }
}
