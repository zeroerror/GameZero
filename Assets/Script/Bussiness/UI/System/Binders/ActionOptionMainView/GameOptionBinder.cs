using UnityEngine;

public class GameOptionBinder
{
    public GameObject gameObject{ get; private set; }

    public GameOptionBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject text => _text ?? (_text = this.gameObject.transform.Find("text").gameObject);
    private GameObject _text;
}
