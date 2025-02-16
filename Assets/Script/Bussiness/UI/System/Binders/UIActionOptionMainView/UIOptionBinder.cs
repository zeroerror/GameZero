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

    public Image img_title => _img_title ?? (_img_title = this.gameObject.transform.Find("img_title").GetComponent<Image>());
    private Image _img_title;
    public Text txt_title => _txt_title ?? (_txt_title = this.gameObject.transform.Find("txt_title").GetComponent<Text>());
    private Text _txt_title;
    public Text txt_content => _txt_content ?? (_txt_content = this.gameObject.transform.Find("txt_content").GetComponent<Text>());
    private Text _txt_content;
}
