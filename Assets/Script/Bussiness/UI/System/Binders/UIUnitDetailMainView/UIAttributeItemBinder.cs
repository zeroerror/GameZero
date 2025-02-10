using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAttributeItemBinder
{
    public GameObject gameObject{ get; private set; }

    public UIAttributeItemBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Image bg => _bg ?? (_bg = this.gameObject.transform.Find("bg").GetComponent<Image>());
    private Image _bg;
    public GameObject group => _group ?? (_group = this.gameObject.transform.Find("group").GetComponent<GameObject>());
    private GameObject _group;
    public Image group_img_icon => _group_img_icon ?? (_group_img_icon = this.gameObject.transform.Find("group/img_icon").GetComponent<Image>());
    private Image _group_img_icon;
    public TextMeshProUGUI group_txt_value => _group_txt_value ?? (_group_txt_value = this.gameObject.transform.Find("group/txt_value").GetComponent<TextMeshProUGUI>());
    private TextMeshProUGUI _group_txt_value;
    public TextMeshProUGUI group_txt_addition => _group_txt_addition ?? (_group_txt_addition = this.gameObject.transform.Find("group/txt_addition").GetComponent<TextMeshProUGUI>());
    private TextMeshProUGUI _group_txt_addition;
}
