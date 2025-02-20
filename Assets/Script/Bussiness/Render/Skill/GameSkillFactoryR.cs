using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameSkillFactoryR
    {
        public GameSkillTemplateR template { get; private set; }
        public GameSkillFactoryR()
        {
            this.template = new GameSkillTemplateR();
        }

        public GameSkillEntityR Load(int typeId, bool isMultyAnimationLayer)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameSkillFactoryR.Load: typeId not found: " + typeId);
                return null;
            }
            this._ProcessAnimationTransform(model, model.movementModel, isMultyAnimationLayer);
            var skill = new GameSkillEntityR(model);
            return skill;
        }

        /// <summary> 加工动画文件的Transform信息 </summary>
        private void _ProcessAnimationTransform(GameSkillModelR skillModel, GameSkillMovementModel movementModel, bool isMultyAnimationLayer)
        {
            var isPassive = skillModel.skillType == GameSkillType.Passive;
            if (isPassive) return;
            var movementType = movementModel.movementType;
            if (movementType != GameSkillMovementType.FixedTimeDash && movementType != GameSkillMovementType.FixedSpeedDash) return;
            // 清空除Y轴以外的动画位移
            var path = isMultyAnimationLayer ? "Right" : "Body";
            var clip = GameResourceManager.LoadAnimationClip(skillModel.clipUrl);
            clip.SetCurve(path, typeof(Transform), "m_LocalPosition.x", new AnimationCurve());
            clip.SetCurve(path, typeof(Transform), "m_LocalPosition.z", new AnimationCurve());
        }
    }
}