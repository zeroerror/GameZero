using GamePlay.Bussiness.Config;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Render;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Config
{
    [CreateAssetMenu(fileName = "template_buff_", menuName = "游戏玩法/配置/buff模板")]
    public class GameBuffSO : GameSOBase
    {
        public string buffName;
        public string desc;
        public GameBuffRefreshFlag refreshFlag;
        public int maxLayer;
        public int actionParam;
        public GameActionSO[] actionSOs;
        public float actionCD;
        public GameBuffConditionSetEM conditionSetEM_action;
        public GameBuffConditionSetEM conditionSetEM_remove;

        public bool layerSelectorEnable;
        public GameEntitySelectorEM layerSelectorEM;

        public bool layerValueRefEnable;
        public GameActionValueRefEM layerValueRefEM;

        public GameObject vfxPrefab;
        public string vfxPrefabUrl;
        public GameFieldLayerType vfxLayerType;
        public Vector2 vfxScale;
        public Vector2 vfxOffset;
        public int vfxOrderOffset;

        public GameBuffAttributeEM[] attributeEMs;

        public GameBuffModel ToModel()
        {
            var model = new GameBuffModel(
                this.typeId,
                this.desc,
                this.refreshFlag,
                this.maxLayer,
                this.actionParam,
                this.actionSOs?.Map(e => e.typeId),
                this.actionCD,
                this.conditionSetEM_action.ToModel(),
                this.conditionSetEM_remove.ToModel(),
                this.attributeEMs?.Map(e => e.ToModel()),
                this.layerSelectorEnable ? this.layerSelectorEM?.ToModel() : null,
                this.layerValueRefEnable ? this.layerValueRefEM?.ToModel() : null
            );
            return model;
        }

        public GameBuffModelR ToModelR()
        {
            var model = new GameBuffModelR(
                this.typeId,
                this.desc,
                this.refreshFlag,
                this.maxLayer,
                this.actionSOs.Map(e => e.typeId),
                this.conditionSetEM_action.ToModel(),
                this.conditionSetEM_remove.ToModel(),
                this.vfxPrefabUrl,
                this.vfxLayerType,
                this.vfxScale,
                this.vfxOffset,
                this.vfxOrderOffset
            );
            return model;
        }
    }
}
