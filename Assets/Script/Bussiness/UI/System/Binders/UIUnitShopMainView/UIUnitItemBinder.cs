using UnityEngine;

public class UIUnitItemBinder
{
    public GameObject gameObject{ get; private set; }

    public UIUnitItemBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject txt_name => _txt_name ?? (_txt_name = this.gameObject.transform.Find("txt_name").gameObject);
    private GameObject _txt_name;
}
