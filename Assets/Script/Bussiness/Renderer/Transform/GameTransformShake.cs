using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public class GameTransformShake
    {
        /// <summary> 变换组件 </summary>
        public Transform transform;
        /// <summary> 角度 </summary>
        public float angle;
        /// <summary> 振幅 </summary>
        public float amplitude;
        /// <summary> 频率 </summary>   
        public float frequency;
        /// <summary> 持续时间 </summary>   
        public float duration;
        /// <summary> 已经过去的时间 </summary>
        public float elapsedTime { get; private set; }

        public bool isShaking { get; private set; }
        private Vector2 _lastShake;
        private Vector2 _shakeDir;

        public void SetShake(Transform transform, float angle, float amplitude, float frequency, float duration)
        {
            this.Reset();
            this.transform = transform;
            this.angle = angle;
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.duration = duration;
            this._shakeDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            this.isShaking = true;
        }

        public void Reset()
        {
            // 还原位置
            if (this.transform)
            {
                this.transform.position -= new Vector3(this._lastShake.x, this._lastShake.y, 0);
            }
            this.isShaking = false;
            this.transform = null;
            this.elapsedTime = 0;
            this._lastShake = Vector2.zero;
            this._shakeDir = Vector2.zero;
        }

        public void Tick(float dt)
        {
            if (this.duration <= 0) return;
            this.elapsedTime += dt;
            var ratio = GameMathF.Min(this.elapsedTime / this.duration, 1);
            var amp = this.amplitude * GameMathF.Sin(this.frequency * ratio * 2 * Mathf.PI);
            Vector2 shake = this._shakeDir * amp * (1 - ratio);
            Vector2 offset = shake - this._lastShake;
            this._lastShake = shake;
            this.transform.position += new Vector3(offset.x, offset.y, 0);

            if (ratio >= 1)
            {
                this.Reset();
            }
        }
    }
}