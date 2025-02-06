using UnityEngine;

public class UIUnitShopMainViewBinder
{
    public GameObject gameObject{ get; private set; }

    public UIUnitShopMainViewBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public GameObject unitGroup => _unitGroup ?? (_unitGroup = this.gameObject.transform.Find("unitGroup").gameObject);
    private GameObject _unitGroup;
    public UIUnitItemBinder unit1 => _unit1 ?? (_unit1 = new UIUnitItemBinder(GameObject.Find("unit1")));
    private UIUnitItemBinder _unit1;
    public UIUnitItemBinder unit2 => _unit2 ?? (_unit2 = new UIUnitItemBinder(GameObject.Find("unit2")));
    private UIUnitItemBinder _unit2;
    public UIUnitItemBinder unit3 => _unit3 ?? (_unit3 = new UIUnitItemBinder(GameObject.Find("unit3")));
    private UIUnitItemBinder _unit3;
    public UIUnitItemBinder unit4 => _unit4 ?? (_unit4 = new UIUnitItemBinder(GameObject.Find("unit4")));
    private UIUnitItemBinder _unit4;
    public UIUnitItemBinder unit5 => _unit5 ?? (_unit5 = new UIUnitItemBinder(GameObject.Find("unit5")));
    private UIUnitItemBinder _unit5;
}
