using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIOptionBinder
{
    public GameObject gameObject{ get; private set; }

    public UIOptionBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Text text => _text ?? (_text = this.gameObject.transform.Find("text").GetComponent<Text>());
    private Text _text;
}
