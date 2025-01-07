using UnityEngine;

public class GameActionOptionMainViewBinder
{
    public GameObject gameObject{ get; private set; }

    public GameActionOptionMainViewBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject optionGroup => _optionGroup ?? (_optionGroup = this.gameObject.transform.Find("optionGroup").gameObject);
    private GameObject _optionGroup;
    public GameOptionBinder option1 => _option1 ?? (_option1 = new GameOptionBinder(GameObject.Find("option1")));
    private GameOptionBinder _option1;
    public GameOptionBinder option2 => _option2 ?? (_option2 = new GameOptionBinder(GameObject.Find("option2")));
    private GameOptionBinder _option2;
    public GameOptionBinder option3 => _option3 ?? (_option3 = new GameOptionBinder(GameObject.Find("option3")));
    private GameOptionBinder _option3;
}
