using UnityEngine;

public class UISkillItemBinder
{
    public GameObject gameObject{ get; private set; }

    public UISkillItemBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject img_icon => _img_icon ?? (_img_icon = this.gameObject.transform.Find("img_icon").gameObject);
    private GameObject _img_icon;
    public GameObject cd => _cd ?? (_cd = this.gameObject.transform.Find("cd").gameObject);
    private GameObject _cd;
    public GameObject cd_txt => _cd_txt ?? (_cd_txt = this.gameObject.transform.Find("cd/txt").gameObject);
    private GameObject _cd_txt;
}
