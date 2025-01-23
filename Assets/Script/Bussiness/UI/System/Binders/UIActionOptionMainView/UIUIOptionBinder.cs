using UnityEngine;

public class UIUIOptionBinder
{
    public GameObject gameObject{ get; private set; }

    public UIUIOptionBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject text => _text ?? (_text = this.gameObject.transform.Find("text").gameObject);
    private GameObject _text;
}
