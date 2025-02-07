using System.Reflection;
using UnityEngine;

public class UIUnitShopMainViewBinder
{
    public GameObject gameObject { get; private set; }

    public UIUnitShopMainViewBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject unitGroup => _unitGroup ?? (_unitGroup = this.gameObject.transform.Find("unitGroup").gameObject);
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
    public GameObject btn_confirm => _btn_confirm ?? (_btn_confirm = this.gameObject.transform.Find("btn_confirm").gameObject);
    private GameObject _btn_confirm;
    public GameObject btn_confirm_txt => _btn_confirm_txt ?? (_btn_confirm_txt = this.gameObject.transform.Find("btn_confirm/txt").gameObject);
    private GameObject _btn_confirm_txt;
    public GameObject btn_refresh => _btn_refresh ?? (_btn_refresh = this.gameObject.transform.Find("btn_refresh").gameObject);
    private GameObject _btn_refresh;
    public GameObject btn_refresh_txt => _btn_refresh_txt ?? (_btn_refresh_txt = this.gameObject.transform.Find("btn_refresh/txt").gameObject);
    private GameObject _btn_refresh_txt;
    public GameObject txt_gold => _txt_gold ?? (_txt_gold = this.gameObject.transform.Find("txt_gold").gameObject);
    private GameObject _txt_gold;
}
