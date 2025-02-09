using UnityEngine;

public class UIUnitDetailMainViewBinder
{
    public GameObject gameObject { get; private set; }

    public UIUnitDetailMainViewBinder(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public UIPanelComBinder panelCom => _panelCom ?? (_panelCom = new UIPanelComBinder(this.gameObject.transform.Find("panelCom").gameObject));
    private UIPanelComBinder _panelCom;
}
