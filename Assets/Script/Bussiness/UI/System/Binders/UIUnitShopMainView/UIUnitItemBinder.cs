using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUnitItemBinder
{
    public GameObject gameObject{ get; private set; }

    public UIUnitItemBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Text txt_name => _txt_name ?? (_txt_name = this.gameObject.transform.Find("txt_name").GetComponent<Text>());
    private Text _txt_name;
}
