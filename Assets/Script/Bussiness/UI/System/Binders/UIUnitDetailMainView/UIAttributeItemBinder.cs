using UnityEngine;

public class UIAttributeItemBinder
{
    public GameObject gameObject{ get; private set; }

    public UIAttributeItemBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject bg => _bg ?? (_bg = this.gameObject.transform.Find("bg").gameObject);
    private GameObject _bg;
    public GameObject group => _group ?? (_group = this.gameObject.transform.Find("group").gameObject);
    private GameObject _group;
    public GameObject group_img_icon => _group_img_icon ?? (_group_img_icon = this.gameObject.transform.Find("group/img_icon").gameObject);
    private GameObject _group_img_icon;
    public GameObject group_txt_value => _group_txt_value ?? (_group_txt_value = this.gameObject.transform.Find("group/txt_value").gameObject);
    private GameObject _group_txt_value;
    public GameObject group_txt_addition => _group_txt_addition ?? (_group_txt_addition = this.gameObject.transform.Find("group/txt_addition").gameObject);
    private GameObject _group_txt_addition;
}
