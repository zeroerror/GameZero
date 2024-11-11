using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Core
{

    public class GameCameraShakeData
    {
        public float angle;
        public float amplitude;
        public float frequency;
        public float duration;

        public GameVec2 direction;
        public GameVec2 value;
        public float time;

        public GameCameraShakeData(float angle, float amplitude, float frequency, float duration)
        {
            this.angle = angle;
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.duration = duration;

            this.direction = GameVectorUtil.RotateOnAxisZ(GameVec2.up, angle);
            this.value = GameVec2.zero;
            this.time = 0;
        }

        public void Clear()
        {
            this.value = GameVec2.zero;
            this.time = 0;
        }
    }


    public class GameCameraShakeCom
    {
        public Camera camera { get; private set; }

        List<GameCameraShakeData> _shakeDataList = new List<GameCameraShakeData>();
        Queue<GameCameraShakeData> _shakeDataPool = new Queue<GameCameraShakeData>();
        GameCameraShakeData _CreateData(float angle, float amplitude, float frequency, float duration)
        {
            if (!this._shakeDataPool.TryDequeue(out var data))
            {
                data = new GameCameraShakeData(angle, amplitude, frequency, duration);
            }
            else
            {
                data.Clear();
            }
            this._shakeDataList.Add(data);
            return data;
        }


        public GameCameraShakeCom(Camera camera)
        {
            this.camera = camera;
        }

        public void Tick(float dt)
        {
            for (var i = 0; i < this._shakeDataList.Count; i++)
            {
                var needRemove = this._Tick(this._shakeDataList[i], dt);
                if (needRemove)
                {
                    this._shakeDataPool.Enqueue(this._shakeDataList[i]);
                    this._shakeDataList.RemoveAt(i);
                    i--;
                }
            }
        }

        bool _Tick(GameCameraShakeData data, float dt)
        {
            if (data.duration <= 0) return true;
            data.time += dt;
            var t = GameMathF.Min(data.time / data.duration, 1);
            var amp = data.amplitude * GameMathF.Sin(data.frequency * t * 2 * Mathf.PI);
            data.value = data.direction * amp * (1 - t);
            return t >= 1;
        }

        public void Apply()
        {
            var pos = this.camera.transform.position.GetVec2();
            for (var i = 0; i < this._shakeDataList.Count; i++)
            {
                pos += this._shakeDataList[i].value;
            }
            this.camera.transform.position = pos;
        }

        public void Shake(float angle, float amplitude, float frequency, float duration)
        {
            this._CreateData(angle, amplitude, frequency, duration);
        }
    }
}