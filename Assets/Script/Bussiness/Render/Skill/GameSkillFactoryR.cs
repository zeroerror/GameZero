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

        public GameSkillEntityR Load(int typeId)
        {
            if (!template.TryGet(typeId, out var model))
            {
                GameLogger.LogError("GameSkillFactoryR.Load: typeId not found: " + typeId);
                return null;
            }
            var clip = GameResourceService.Load<AnimationClip>(model.clipUrl);
            this._ProcessAnimationTransform(clip, model.movementModel);
            var skill = new GameSkillEntityR(model);
            return skill;
        }

        /// <summary> 加工动画文件的Transform信息 </summary>
        private void _ProcessAnimationTransform(AnimationClip clip, GameSkillMovementModel movementModel)
        {
            var movementType = movementModel.movementType;
            if (movementType != GameSkillMovementType.FixedTimeDash && movementType != GameSkillMovementType.FixedSpeedDash) return;
            clip.SetCurve("Body", typeof(Transform), "m_localPosition", null);
            clip.SetCurve("Right", typeof(Transform), "m_localPosition", null);
            var curve = new AnimationCurve();
            var dashModels = movementModel.dashModels;
            var count = dashModels?.Length ?? 0;
            if (count == 0) return;
            for (var i = 0; i < count - 1; i++)
            {
                var model = dashModels[i];
                var nextModel = dashModels[i + 1];
                var fromFrame = model.frame;
                var toFrame = nextModel.frame;
                var betweenFrame = toFrame - fromFrame;
                for (var frame = fromFrame + 1; frame <= toFrame; frame++)
                {
                    var time = frame.ToTime();
                    var y = Mathf.Lerp(model.y, nextModel.y, (float)(frame - fromFrame) / betweenFrame);
                    curve.AddKey(new Keyframe(time, y));
                }
            }
            clip.SetCurve("Body", typeof(Transform), "m_localPosition.y", curve);
            clip.SetCurve("Right", typeof(Transform), "m_localPosition.y", curve);
        }
    }
}