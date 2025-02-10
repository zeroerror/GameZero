using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillItemBinder
{
    public GameObject gameObject{ get; private set; }

    public UISkillItemBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Image img_icon => _img_icon ?? (_img_icon = this.gameObject.transform.Find("img_icon").GetComponent<Image>());
    private Image _img_icon;
    public Image cd => _cd ?? (_cd = this.gameObject.transform.Find("cd").GetComponent<Image>());
    private Image _cd;
    public TextMeshProUGUI cd_txt => _cd_txt ?? (_cd_txt = this.gameObject.transform.Find("cd/txt").GetComponent<TextMeshProUGUI>());
    private TextMeshProUGUI _cd_txt;
}
