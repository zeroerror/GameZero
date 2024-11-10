using UnityEngine;

public class GameEntry : MonoBehaviour
{
    public GameCore gameCore { get; private set; }
    void Start()
    {
        this.gameCore = new GameCore();
    }

    void Update()
    {
        this.gameCore.Update(Time.deltaTime);
    }

    void LateUpdate()
    {
        this.gameCore.LateUpdate(Time.deltaTime);
    }
}
