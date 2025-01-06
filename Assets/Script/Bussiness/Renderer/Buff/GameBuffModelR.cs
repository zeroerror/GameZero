using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameBuffModelR
    {
        public readonly int typeId;
        public readonly string desc;
        public readonly GameBuffRefreshFlag refreshFlag;
        public readonly int maxLayer;
        public readonly int[] actionIds;
        public readonly GameBuffConditionSetModel conditionSetModel_action;
        public readonly GameBuffConditionSetModel conditionSetModel_remove;
        public readonly string vfxUrl;
        public readonly GameFieldLayerType vfxLayerType;
        public Vector2 vfxScale;
        public Vector2 vfxOffset;
        public int vfxOrderOffset;

        public GameBuffModelR(
            int typeId,
            string desc,
            GameBuffRefreshFlag refreshFlag,
            int maxLayer,
            int[] actionIds,
            GameBuffConditionSetModel conditionSetModel_action,
            GameBuffConditionSetModel conditionSetModel_remove,
            string vfxUrl,
            GameFieldLayerType vfxLayerType,
            in Vector2 vfxScale,
            in Vector2 vfxOffset,
            int vfxOrderOffset
        )
        {
            this.typeId = typeId;
            this.desc = desc;
            this.refreshFlag = refreshFlag;
            this.maxLayer = maxLayer;
            this.actionIds = actionIds;
            this.conditionSetModel_action = conditionSetModel_action;
            this.conditionSetModel_remove = conditionSetModel_remove;
            this.vfxUrl = vfxUrl;
            this.vfxLayerType = vfxLayerType;
            this.vfxScale = vfxScale;
            this.vfxOffset = vfxOffset;
            this.vfxOrderOffset = vfxOrderOffset;
        }
    }
}