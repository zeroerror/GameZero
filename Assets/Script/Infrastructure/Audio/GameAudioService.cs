using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Infrastructure
{
    /// <summary>
    /// 游戏音频服务
    /// <para> 提供音频播放、暂停、停止等功能, 同时管理音频资源的加载和卸载 </para>
    /// </summary>
    public class GameAudioService
    {
        /// <summary> 当前正在播放的音效 </summary>
        private List<GameAudioPlayable> _playingSFXs = new List<GameAudioPlayable>();

        /// <summary> 正在播放的BGM </summary>
        private GameAudioPlayable _playingBGM;

        /// <summary> 音效实体对象池 key: url </summary>
        private readonly Dictionary<string, List<GameAudioPlayable>> _poolDict = new Dictionary<string, List<GameAudioPlayable>>();

        public GameAudioService() { }

        // 每帧检测回收音效
        public void Tick(float dt)
        {
            // SFX 音效
            for (int i = 0; i < _playingSFXs.Count; i++)
            {
                var sfx = _playingSFXs[i];
                if (!sfx.isPlaying)
                {
                    _playingSFXs.Remove(sfx);
                    this._AddToPool(sfx);
                    i--;
                    continue;
                }
            }

            // BGM 音效
            if (_playingBGM != null && !_playingBGM.isPlaying)
            {
                this._AddToPool(_playingBGM);
                _playingBGM = null;
            }
        }

        public void Destroy()
        {
            this._playingSFXs?.Foreach((sfx) =>
            {
                sfx.Destroy();
            });
            this._playingBGM?.Destroy();
        }

        private void _AddToPool(GameAudioPlayable GameAudioPlayable)
        {
            if (!_poolDict.ContainsKey(GameAudioPlayable.url))
            {
                _poolDict[GameAudioPlayable.url] = new List<GameAudioPlayable>();
            }
            _poolDict[GameAudioPlayable.url].Add(GameAudioPlayable);
        }

        /// <summary>
        /// 播放音效
        /// <para> sfxUrl: AudioSource对应的prefab资源路径 </para>
        /// <para> loop: 是否循环播放 </para>
        /// </summary>
        public GameAudioPlayable PlaySFX(string sfxUrl, float startTime = 0, bool loop = false)
        {
            var playable = this._GetPlayable(sfxUrl);
            playable.Play(startTime, loop);
            _playingSFXs.Add(playable);
            return playable;
        }

        private GameAudioPlayable _GetPlayable(string url)
        {
            var pool = _poolDict.ContainsKey(url) ? _poolDict[url] : new List<GameAudioPlayable>();
            var count = pool?.Count ?? 0;
            if (count != 0)
            {
                return pool.Pop();
            }
            var playable = new GameAudioPlayable(url, this._LoadAudioSource(url));
            return playable;
        }

        private AudioSource _LoadAudioSource(string url)
        {
            var isExist = GameResourceManager.IsExist<GameObject>(url, false);
            if (isExist)
            {
                var prefab = GameResourceManager.Load<GameObject>(url);
                var prefabGO = Object.Instantiate(prefab);
                var audioSource = prefabGO.GetComponent<AudioSource>();
                var clipName = audioSource.clip.name;
                prefabGO.name = "audio_playable_" + clipName;
                return audioSource;
            }

            // 没有相关预制体, 使用默认
            var clip = GameResourceManager.Load<AudioClip>(url);
            var defaultGO = new GameObject("audio_playable_" + clip.name);
            var defaultAudioSource = defaultGO.AddComponent<AudioSource>();
            defaultAudioSource.clip = clip;
            return defaultAudioSource;
        }

        /// <summary>
        /// 播放背景音乐
        /// <para> url: 音频资源路径 </para>
        /// <para> loop: 是否循环播放 </para>
        /// </summary>
        public void PlayBGM(string url, float startTime = 0, bool loop = true)
        {
            if (_playingBGM != null)
            {
                var isSame = _playingBGM.url == url;
                if (isSame)
                {
                    _playingBGM.Play(startTime, loop);
                    return;
                }
                _playingBGM.Stop();
                _playingBGM = null;
            }

            var playable = this._GetPlayable(url);
            playable.Play(0, loop);
            _playingBGM = playable;
        }
    }
}
