using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIActionOptionMainViewBinder
{
    public GameObject gameObject{ get; private set; }

    public UIActionOptionMainViewBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject optionGroup => _optionGroup ?? (_optionGroup = this.gameObject.transform.Find("optionGroup").GetComponent<GameObject>());
    private GameObject _optionGroup;
    public UIOptionBinder optionGroup_option1 => _optionGroup_option1 ?? (_optionGroup_option1 = new UIOptionBinder(this.gameObject.transform.Find("optionGroup/option1").gameObject));
    private UIOptionBinder _optionGroup_option1;
    public UIOptionBinder optionGroup_option2 => _optionGroup_option2 ?? (_optionGroup_option2 = new UIOptionBinder(this.gameObject.transform.Find("optionGroup/option2").gameObject));
    private UIOptionBinder _optionGroup_option2;
    public UIOptionBinder optionGroup_option3 => _optionGroup_option3 ?? (_optionGroup_option3 = new UIOptionBinder(this.gameObject.transform.Find("optionGroup/option3").gameObject));
    private UIOptionBinder _optionGroup_option3;
}
