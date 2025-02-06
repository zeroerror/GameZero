using UnityEngine;

public class UIActionOptionMainViewBinder
{
    public GameObject gameObject{ get; private set; }

    public UIActionOptionMainViewBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject optionGroup => _optionGroup ?? (_optionGroup = this.gameObject.transform.Find("optionGroup").gameObject);
    private GameObject _optionGroup;
    public UIOptionBinder option1 => _option1 ?? (_option1 = new UIOptionBinder(GameObject.Find("option1")));
    private UIOptionBinder _option1;
    public UIOptionBinder option2 => _option2 ?? (_option2 = new UIOptionBinder(GameObject.Find("option2")));
    private UIOptionBinder _option2;
    public UIOptionBinder option3 => _option3 ?? (_option3 = new UIOptionBinder(GameObject.Find("option3")));
    private UIOptionBinder _option3;
}
