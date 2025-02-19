using GamePlay.Bussiness.Logic;
using GamePlay.Core;
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
            var url = skillModel.clipUrl;
            var clip = GameResourceService.LoadAnimationClip(url);
            var movementType = movementModel.movementType;
            if (movementType != GameSkillMovementType.FixedTimeDash && movementType != GameSkillMovementType.FixedSpeedDash) return;

            // 分别创建X、Y、Z轴的动画曲线
            var curveX = new AnimationCurve();
            var curveY = new AnimationCurve();
            var curveZ = new AnimationCurve();
            var dashModels = movementModel.dashModels;
            var count = dashModels?.Length ?? 0;
            if (count == 0) return;
            for (var i = 0; i < count - 1; i++)
            {
                var dashModel = dashModels[i];
                var nextModel = dashModels[i + 1];
                var fromFrame = dashModel.frame;
                var toFrame = nextModel.frame;
                var betweenFrame = toFrame - fromFrame;
                for (var frame = fromFrame + 1; frame <= toFrame; frame++)
                {
                    var time = frame.ToTime();
                    var y = Mathf.Lerp(dashModel.y, nextModel.y, (float)(frame - fromFrame) / betweenFrame);
                    // 仅保留Y轴的位移
                    curveX.AddKey(new Keyframe(time, 0));
                    curveY.AddKey(new Keyframe(time, y));
                    curveZ.AddKey(new Keyframe(time, 0));
                }
            }
            // 设置新的动画曲线
            var path = isMultyAnimationLayer ? "Right" : "Body";
            clip.SetCurve(path, typeof(Transform), "m_LocalPosition.x", curveX);
            clip.SetCurve(path, typeof(Transform), "m_LocalPosition.y", curveY);
            clip.SetCurve(path, typeof(Transform), "m_LocalPosition.z", curveZ);
        }
    }
}