using UnityEngine;

namespace GamePlay.Bussiness.Renderer
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
            this.elapsedTime += dt;
            if (this.elapsedTime >= this.duration)
            {
                this.Reset();
                return;
            }
            Vector2 shake = this._shakeDir * Mathf.Sin(this.elapsedTime * this.frequency) * this.amplitude;
            Vector2 offset = shake - this._lastShake;
            this.transform.position += new Vector3(offset.x, offset.y, 0);
            this._lastShake = shake;
        }
    }
}