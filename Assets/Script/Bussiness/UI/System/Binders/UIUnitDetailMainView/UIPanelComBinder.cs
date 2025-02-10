using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPanelComBinder
{
    public GameObject gameObject{ get; private set; }

    public UIPanelComBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Image img_bg => _img_bg ?? (_img_bg = this.gameObject.transform.Find("img_bg").GetComponent<Image>());
    private Image _img_bg;
    public Image skillGroup => _skillGroup ?? (_skillGroup = this.gameObject.transform.Find("skillGroup").GetComponent<Image>());
    private Image _skillGroup;
    public UISkillItemBinder skillGroup_item1 => _skillGroup_item1 ?? (_skillGroup_item1 = new UISkillItemBinder(this.gameObject.transform.Find("skillGroup/item1").gameObject));
    private UISkillItemBinder _skillGroup_item1;
    public UISkillItemBinder skillGroup_item2 => _skillGroup_item2 ?? (_skillGroup_item2 = new UISkillItemBinder(this.gameObject.transform.Find("skillGroup/item2").gameObject));
    private UISkillItemBinder _skillGroup_item2;
    public UISkillItemBinder skillGroup_item3 => _skillGroup_item3 ?? (_skillGroup_item3 = new UISkillItemBinder(this.gameObject.transform.Find("skillGroup/item3").gameObject));
    private UISkillItemBinder _skillGroup_item3;
    public Image attributeList => _attributeList ?? (_attributeList = this.gameObject.transform.Find("attributeList").GetComponent<Image>());
    private Image _attributeList;
    public Image attributeList_Viewport => _attributeList_Viewport ?? (_attributeList_Viewport = this.gameObject.transform.Find("attributeList/Viewport").GetComponent<Image>());
    private Image _attributeList_Viewport;
    public GameObject attributeList_Viewport_Content => _attributeList_Viewport_Content ?? (_attributeList_Viewport_Content = this.gameObject.transform.Find("attributeList/Viewport/Content").GetComponent<GameObject>());
    private GameObject _attributeList_Viewport_Content;
    public UIAttributeItemBinder attributeList_Viewport_Content_item1 => _attributeList_Viewport_Content_item1 ?? (_attributeList_Viewport_Content_item1 = new UIAttributeItemBinder(this.gameObject.transform.Find("attributeList/Viewport/Content/item1").gameObject));
    private UIAttributeItemBinder _attributeList_Viewport_Content_item1;
    public UIAttributeItemBinder attributeList_Viewport_Content_item2 => _attributeList_Viewport_Content_item2 ?? (_attributeList_Viewport_Content_item2 = new UIAttributeItemBinder(this.gameObject.transform.Find("attributeList/Viewport/Content/item2").gameObject));
    private UIAttributeItemBinder _attributeList_Viewport_Content_item2;
    public UIAttributeItemBinder attributeList_Viewport_Content_item3 => _attributeList_Viewport_Content_item3 ?? (_attributeList_Viewport_Content_item3 = new UIAttributeItemBinder(this.gameObject.transform.Find("attributeList/Viewport/Content/item3").gameObject));
    private UIAttributeItemBinder _attributeList_Viewport_Content_item3;
    public UIAttributeItemBinder attributeList_Viewport_Content_item4 => _attributeList_Viewport_Content_item4 ?? (_attributeList_Viewport_Content_item4 = new UIAttributeItemBinder(this.gameObject.transform.Find("attributeList/Viewport/Content/item4").gameObject));
    private UIAttributeItemBinder _attributeList_Viewport_Content_item4;
    public UIAttributeItemBinder attributeList_Viewport_Content_item5 => _attributeList_Viewport_Content_item5 ?? (_attributeList_Viewport_Content_item5 = new UIAttributeItemBinder(this.gameObject.transform.Find("attributeList/Viewport/Content/item5").gameObject));
    private UIAttributeItemBinder _attributeList_Viewport_Content_item5;
    public UIAttributeItemBinder attributeList_Viewport_Content_item6 => _attributeList_Viewport_Content_item6 ?? (_attributeList_Viewport_Content_item6 = new UIAttributeItemBinder(this.gameObject.transform.Find("attributeList/Viewport/Content/item6").gameObject));
    private UIAttributeItemBinder _attributeList_Viewport_Content_item6;
    public UIAttributeItemBinder attributeList_Viewport_Content_item7 => _attributeList_Viewport_Content_item7 ?? (_attributeList_Viewport_Content_item7 = new UIAttributeItemBinder(this.gameObject.transform.Find("attributeList/Viewport/Content/item7").gameObject));
    private UIAttributeItemBinder _attributeList_Viewport_Content_item7;
    public Image img_head => _img_head ?? (_img_head = this.gameObject.transform.Find("img_head").GetComponent<Image>());
    private Image _img_head;
    public Image img_label => _img_label ?? (_img_label = this.gameObject.transform.Find("img_label").GetComponent<Image>());
    private Image _img_label;
    public Text txt_name => _txt_name ?? (_txt_name = this.gameObject.transform.Find("txt_name").GetComponent<Text>());
    private Text _txt_name;
}
