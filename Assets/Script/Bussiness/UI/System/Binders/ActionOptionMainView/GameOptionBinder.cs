using UnityEngine;

public class GameOptionBinder
{
    private GameObject _gameObject;

    public GameOptionBinder(GameObject gameObject)
    {
        _gameObject = gameObject;
    }

    public GameObject text => _text ?? (_text = _gameObject.transform.Find("text").gameObject);
    private GameObject _text;
}
