using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUnitShopMainViewBinder
{
    public GameObject gameObject{ get; private set; }

    public UIUnitShopMainViewBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Image mask => _mask ?? (_mask = this.gameObject.transform.Find("mask").GetComponent<Image>());
    private Image _mask;
    public GameObject unitGroup => _unitGroup ?? (_unitGroup = this.gameObject.transform.Find("unitGroup").GetComponent<GameObject>());
    private GameObject _unitGroup;
    public UIUnitItemBinder unitGroup_unit1 => _unitGroup_unit1 ?? (_unitGroup_unit1 = new UIUnitItemBinder(this.gameObject.transform.Find("unitGroup/unit1").gameObject));
    private UIUnitItemBinder _unitGroup_unit1;
    public UIUnitItemBinder unitGroup_unit2 => _unitGroup_unit2 ?? (_unitGroup_unit2 = new UIUnitItemBinder(this.gameObject.transform.Find("unitGroup/unit2").gameObject));
    private UIUnitItemBinder _unitGroup_unit2;
    public UIUnitItemBinder unitGroup_unit3 => _unitGroup_unit3 ?? (_unitGroup_unit3 = new UIUnitItemBinder(this.gameObject.transform.Find("unitGroup/unit3").gameObject));
    private UIUnitItemBinder _unitGroup_unit3;
    public UIUnitItemBinder unitGroup_unit4 => _unitGroup_unit4 ?? (_unitGroup_unit4 = new UIUnitItemBinder(this.gameObject.transform.Find("unitGroup/unit4").gameObject));
    private UIUnitItemBinder _unitGroup_unit4;
    public UIUnitItemBinder unitGroup_unit5 => _unitGroup_unit5 ?? (_unitGroup_unit5 = new UIUnitItemBinder(this.gameObject.transform.Find("unitGroup/unit5").gameObject));
    private UIUnitItemBinder _unitGroup_unit5;
    public Image btn_confirm => _btn_confirm ?? (_btn_confirm = this.gameObject.transform.Find("btn_confirm").GetComponent<Image>());
    private Image _btn_confirm;
    public Text btn_confirm_txt => _btn_confirm_txt ?? (_btn_confirm_txt = this.gameObject.transform.Find("btn_confirm/txt").GetComponent<Text>());
    private Text _btn_confirm_txt;
    public Image btn_refresh => _btn_refresh ?? (_btn_refresh = this.gameObject.transform.Find("btn_refresh").GetComponent<Image>());
    private Image _btn_refresh;
    public Text btn_refresh_txt => _btn_refresh_txt ?? (_btn_refresh_txt = this.gameObject.transform.Find("btn_refresh/txt").GetComponent<Text>());
    private Text _btn_refresh_txt;
    public Text txt_gold => _txt_gold ?? (_txt_gold = this.gameObject.transform.Find("txt_gold").GetComponent<Text>());
    private Text _txt_gold;
}
