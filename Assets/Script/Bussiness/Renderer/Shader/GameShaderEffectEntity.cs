using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameShaderEffectEntity
    {
        public GameShaderEffectModel model { get; private set; }
        public readonly Material material;
        private MaterialPropertyBlock propertyBlock;

        public float elapsedTime { get; private set; }
        public bool isPlaying { get; private set; }

        public Renderer renderer { get; private set; }
        private Material _cachedMaterial;

        public GameShaderEffectEntity(GameShaderEffectModel model, Material mat)
        {
            this.model = model;
            this.material = mat;
            this.propertyBlock = new MaterialPropertyBlock();
        }

        public void Play(Renderer renderer, float startTime = 0)
        {
            if (!renderer) return;
            this.Stop();
            this._cachedMaterial = renderer.sharedMaterial;

            // .material = xxx 会创建新的材质实例导致内存泄漏, 因此采取sharedMaterial和MaterialPropertyBlock搭配使用
            renderer.sharedMaterial = this.material;

            this.renderer = renderer;
            this.elapsedTime = startTime;
            this.isPlaying = true;
        }

        public void Stop()
        {
            if (!this.isPlaying) return;
            this.isPlaying = false;
            this.renderer.sharedMaterial = this._cachedMaterial;
            this._cachedMaterial = null;
            this.renderer = null;
            this.elapsedTime = 0;
        }

        public void Tick(float dt)
        {
            if (!this.isPlaying) return;
            this.elapsedTime += dt;

            var length = this.model.length;
            var loopCount = this.model.loopCount;
            var loopLength = length * loopCount;
            if (this.elapsedTime >= loopLength)
            {
                this.elapsedTime = loopLength;
            }

            var propTimeLines = this.model.propTimeLines;
            propTimeLines?.Foreach((timeline) =>
            {
                var time = this.elapsedTime != loopLength ? this.elapsedTime % length : length;// 保证循环结束时，时间点在最后一帧
                if (time >= timeline.startTime && time <= timeline.endTime)
                {
                    var t = (time - timeline.startTime) / (timeline.endTime - timeline.startTime);
                    var curve_float = timeline.curve_float;
                    if (curve_float != null)
                    {
                        var ratio = curve_float.Evaluate(t);
                        var value = timeline.fromValue_float + ratio * (timeline.toValue_float - timeline.fromValue_float);

                        // 使用 MaterialPropertyBlock 来设置参数，避免内存泄漏
                        propertyBlock.SetFloat(timeline.propName, value);
                        renderer.SetPropertyBlock(propertyBlock); // 将属性块应用到渲染器
                        GameLogger.DebugLog($"ShaderEffectEntity.Tick: {timeline.propName} = {value}");
                        return;
                    }

                    GameLogger.LogError($"ShaderEffectEntity.Tick: 未处理的值类型 {timeline.propName}");
                }
            });

            if (this.elapsedTime >= loopLength)
            {
                this.Stop();
            }
        }
    }
}
