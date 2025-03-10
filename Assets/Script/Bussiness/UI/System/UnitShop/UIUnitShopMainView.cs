using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Render;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;
namespace GamePlay.Bussiness.UI
{
    public struct UIUnitShopMainViewInput
    {
        public GameItemUnitModel[] buyableUnits;
    }

    public class UIUnitShopMainView : UIBase
    {
        public override UILayerType layerType => UILayerType.Main;
        public override string uiPkgUrl => "UI/System/UnitShop";
        public override string uiName => "UIUnitShopMainView";
        public UIUnitShopMainViewBinder uiBinder;

        private UIUnitShopMainViewInput _viewInput;
        private GameItemUnitModel[] _unitModels => this._viewInput.buyableUnits;

        protected override void _OnInit()
        {
            this.uiBinder = new UIUnitShopMainViewBinder(this.go);
            this._viewInput = (UIUnitShopMainViewInput)this._uiInput.customData;
        }

        protected override void _BindEvents()
        {
            base._BindEvents();
            this._uiApi.directorApi.BindKeyAction(KeyCode.G, this._OnClickItemDown0);
            this._uiApi.directorApi.BindKeyAction(KeyCode.H, this._OnClickItemDown1);
            this._uiApi.directorApi.BindKeyAction(KeyCode.J, this._OnClickItemDown2);
            this._uiApi.directorApi.BindKeyAction(KeyCode.K, this._OnClickItemDown3);
            this._uiApi.directorApi.BindKeyAction(KeyCode.L, this._OnClickItemDown4);
            this._uiApi.directorApi.BindKeyAction(KeyCode.Escape, this._OnClickBtnCancel);
            this._uiApi.directorApi.BindKeyAction(KeyCode.Return, this._OnClickBtnConfirm);
            this._uiApi.directorApi.BindEvent(UIPlayerEventCollection.UI_PLAYER_COINS_CHANGE, this._OnPlayerCoinsChange);
            this._uiApi.logicApi.directorApi.BindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_EXIT, this._OnDirectorStateExit);

            this.SetClick(this.uiBinder.btn_confirm, this._OnClickBtnConfirm);
            this.SetClick(this.uiBinder.btn_refresh, this._OnClickBtnRefresh);
            this.SetClick(this.uiBinder.btn_camp, this._OnClickBtnCamp);
            this.SetClick(this.uiBinder.btn_cancel, this._OnClickBtnCancel);
            this.SetClick(this.uiBinder.mask, this._OnClickMask);
        }

        protected override void _UnbindEvents()
        {
            base._UnbindEvents();
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.G, this._OnClickItemDown0);
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.H, this._OnClickItemDown1);
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.J, this._OnClickItemDown2);
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.K, this._OnClickItemDown3);
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.L, this._OnClickItemDown4);
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.Escape, this._OnClickBtnCancel);
            this._uiApi.directorApi.UnbindKeyAction(KeyCode.Return, this._OnClickBtnConfirm);
            this._uiApi.directorApi.UnbindEvent(UIPlayerEventCollection.UI_PLAYER_COINS_CHANGE, this._OnPlayerCoinsChange);
            this._uiApi.logicApi.directorApi.UnbindRC(GameDirectorRCCollection.RC_GAME_DIRECTOR_STATE_EXIT, this._OnDirectorStateExit);

        }

        private void _OnPlayerCoinsChange(object args)
        {
            this._RefreshGold();
        }

        private void _OnDirectorStateExit(object args)
        {
            var rcArgs = (GameDirectorRCArgs_StateExit)args;
            if (rcArgs.exitStateType == GameDirectorStateType.FightPreparing)
            {
                this.uiBinder.btn_confirm.gameObject.SetActive(false);
            }
        }

        protected override void _OnShow()
        {
            // 刷新金币显示
            this._RefreshGold();
            // 刷新取消按钮显示
            this._OnClickBtnCancel();
            // 刷新确认按钮显示
            var isFightPreparing = this._uiApi.logicApi.directorApi.GetDirectorState() == GameDirectorStateType.FightPreparing;
            this.uiBinder.btn_confirm.gameObject.SetActive(isFightPreparing);
            // 刷新购买单位列表
            var itemCount = this._unitModels.Length;
            for (var i = 0; i < 5; i++)
            {
                var unitBinder = this.uiBinder.GetField($"unitGroup_unit{i + 1}") as UIUnitItemBinder;
                var isActive = i < itemCount;
                unitBinder.gameObject.SetActive(isActive);
                if (isActive)
                {
                    var text = unitBinder.txt_name;
                    text.text = this._GetItemName(this._unitModels[i]).ToDevStr();
                    var idx = i;
                    this.SetClick(unitBinder.gameObject, () => this._OnClickItemDown(idx));
                    GameLogger.DebugLog($"单位{i + 1}: {this._GetItemName(this._unitModels[i])}");
                }
            }

        }

        private void _RefreshGold()
        {
            var curGold = this._uiApi.playerApi.curGold;
            this.uiBinder.txt_gold.text = curGold.ToDevStr();
        }

        private void _OnClickItemDown(int index)
        {
            if (!GameMath.IsInRange(index, 0, this._unitModels.Length - 1)) return;
            // 预览单位相同则不重新生成
            var previewIdCom = this._previewUnit?.idCom;
            var unitModel = this._unitModels[index];
            if (previewIdCom?.entityType == unitModel.entityType && previewIdCom?.typeId == unitModel.typeId)
            {
                return;
            }

            // 销毁之前的预览单位
            if (this._previewUnit)
            {
                this._uiApi.rendererApi.directorApi.DestroyPreviewUnit(this._previewUnit);
                this.RemoveTimer(this._timerId);
                this._timerId = 0;
                this._previewUnit = null;
            }

            // 生成新的虚拟单位
            this._previewUnit = this._uiApi.rendererApi.directorApi.CreatePreviewUnit(this._unitModels[index]);
            this._uiApi.rendererApi.shaderEffectApi.PlayShaderEffect((int)GameShaderEffectType.PreviewFlash, this._previewUnit);
            this._timerId = this.SetInterval(0.02f, () =>
            {
                if (!this._previewUnit)
                {
                    this.RemoveTimer(this._timerId);
                    return;
                }
                // 根据鼠标位置更新单位位置
                var pos = this._uiApi.directorApi.GetPointerPosition();
                var worldPos = this._uiApi.rendererApi.directorApi.ScreenToWorldPos(pos);
                this._previewUnit.SetPosition(worldPos);
            });
            this._selectedItemIndex = index;

            // 显示取消按钮
            this.uiBinder.btn_cancel.gameObject.SetActive(true);
        }
        private GameEntityBase _previewUnit;
        private int _timerId;
        private int _selectedItemIndex;

        private void _OnClickItemDown0() => this._OnClickItemDown(0);
        private void _OnClickItemDown1() => this._OnClickItemDown(1);
        private void _OnClickItemDown2() => this._OnClickItemDown(2);
        private void _OnClickItemDown3() => this._OnClickItemDown(3);
        private void _OnClickItemDown4() => this._OnClickItemDown(4);

        private void _OnClickBtnConfirm()
        {
            if (this._uiApi.logicApi.directorApi.GetDirectorState() != GameDirectorStateType.FightPreparing) return;
            // 提交LC, 通知逻辑层确认开始
            this._uiApi.logicApi.directorApi.SubmitEvent(GameLCCollection.LC_GAME_PREPARING_CONFIRM_FIGHT, new GameLCArgs_PreparingConfirmFight());
        }

        private void _OnClickBtnRefresh()
        {
            if (!this._uiApi.logicApi.directorApi.ShuffleBuyableUnits(false)) return;
            this._viewInput.buyableUnits = this._uiApi.logicApi.directorApi.GetBuyableUnits();
            this._OnShow();
        }

        private void _OnClickBtnCamp()
        {
            if (this.campType == GameCampType.Ally)
            {
                this.campType = GameCampType.Enemy;
                this.uiBinder.btn_camp_txt.text = "召唤敌方";
            }
            else
            {
                this.campType = GameCampType.Ally;
                this.uiBinder.btn_camp_txt.text = "召唤友军";
            }
        }
        private GameCampType campType = GameCampType.Ally;

        private void _OnClickMask()
        {
            if (this._previewUnit)
            {
                this._uiApi.logicApi.directorApi.BuyUnit(this._selectedItemIndex, this._previewUnit.GetPosition(), this.campType);
            }
        }

        private void _OnClickBtnCancel()
        {
            if (this._timerId != 0)
            {
                this.RemoveTimer(this._timerId);
                this._timerId = 0;
            }
            if (this._previewUnit)
            {
                this._uiApi.rendererApi.shaderEffectApi.StopShaderEffects(this._previewUnit);
                this._uiApi.rendererApi.directorApi.DestroyPreviewUnit(this._previewUnit);
                this._previewUnit = null;
            }
            this.uiBinder.btn_cancel.gameObject.SetActive(false);
        }

        private string _GetItemName(GameItemUnitModel unitModel)
        {
            switch (unitModel.entityType)
            {
                case GameEntityType.Role:
                    this._uiApi.rendererApi.roleApi.GetRoleTemplate().TryGet(unitModel.typeId, out var roleModel);
                    return roleModel.roleName;
                case GameEntityType.Skill:
                    return unitModel.typeId.ToString();
                case GameEntityType.Buff:
                    return unitModel.typeId.ToString();
                default:
                    return "未知";
            }
        }

    }
}