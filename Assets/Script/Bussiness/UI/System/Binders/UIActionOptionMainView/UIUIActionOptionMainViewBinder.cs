using UnityEngine;

public class UIUIActionOptionMainViewBinder
{
    public GameObject gameObject{ get; private set; }

    public UIUIActionOptionMainViewBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject optionGroup => _optionGroup ?? (_optionGroup = this.gameObject.transform.Find("optionGroup").gameObject);
    private GameObject _optionGroup;
    public UIUIOptionBinder option1 => _option1 ?? (_option1 = new UIUIOptionBinder(GameObject.Find("option1")));
    private UIUIOptionBinder _option1;
    public UIUIOptionBinder option2 => _option2 ?? (_option2 = new UIUIOptionBinder(GameObject.Find("option2")));
    private UIUIOptionBinder _option2;
    public UIUIOptionBinder option3 => _option3 ?? (_option3 = new UIUIOptionBinder(GameObject.Find("option3")));
    private UIUIOptionBinder _option3;
}
