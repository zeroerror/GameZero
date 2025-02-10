using UnityEngine;

public class UIPanelComBinder
{
    public GameObject gameObject{ get; private set; }

    public UIPanelComBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject img_bg => _img_bg ?? (_img_bg = this.gameObject.transform.Find("img_bg").gameObject);
    private GameObject _img_bg;
    public GameObject skillGroup => _skillGroup ?? (_skillGroup = this.gameObject.transform.Find("skillGroup").gameObject);
    private GameObject _skillGroup;
    public UISkillItemBinder skillGroup_item1 => _skillGroup_item1 ?? (_skillGroup_item1 = new UISkillItemBinder(this.gameObject.transform.Find("skillGroup/item1").gameObject));
    private UISkillItemBinder _skillGroup_item1;
    public UISkillItemBinder skillGroup_item2 => _skillGroup_item2 ?? (_skillGroup_item2 = new UISkillItemBinder(this.gameObject.transform.Find("skillGroup/item2").gameObject));
    private UISkillItemBinder _skillGroup_item2;
    public UISkillItemBinder skillGroup_item3 => _skillGroup_item3 ?? (_skillGroup_item3 = new UISkillItemBinder(this.gameObject.transform.Find("skillGroup/item3").gameObject));
    private UISkillItemBinder _skillGroup_item3;
    public GameObject attributeList => _attributeList ?? (_attributeList = this.gameObject.transform.Find("attributeList").gameObject);
    private GameObject _attributeList;
    public GameObject attributeList_Viewport => _attributeList_Viewport ?? (_attributeList_Viewport = this.gameObject.transform.Find("attributeList/Viewport").gameObject);
    private GameObject _attributeList_Viewport;
    public GameObject attributeList_Viewport_Content => _attributeList_Viewport_Content ?? (_attributeList_Viewport_Content = this.gameObject.transform.Find("attributeList/Viewport/Content").gameObject);
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
    public GameObject img_head => _img_head ?? (_img_head = this.gameObject.transform.Find("img_head").gameObject);
    private GameObject _img_head;
    public GameObject img_label => _img_label ?? (_img_label = this.gameObject.transform.Find("img_label").gameObject);
    private GameObject _img_label;
    public GameObject txt_name => _txt_name ?? (_txt_name = this.gameObject.transform.Find("txt_name").gameObject);
    private GameObject _txt_name;
}
