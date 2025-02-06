using UnityEngine;

public class UIOptionBinder
{
    public GameObject gameObject{ get; private set; }

    public UIOptionBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject text => _text ?? (_text = this.gameObject.transform.Find("text").gameObject);
    private GameObject _text;
}
