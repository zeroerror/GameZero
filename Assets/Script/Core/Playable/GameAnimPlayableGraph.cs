using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using GamePlay.Infrastructure;

namespace GamePlay.Core
{
    public class GameAnimPlayableGraph
    {
        private Animator _animator;
        private Dictionary<string, AnimationClip> _clipDict = new();
        private PlayableGraph _graph;
        private AnimationMixerPlayable _mixerPlayable;
        private AnimationPlayableOutput _playableOutput;
        private int _curInputPort = 0;
        private int _nextInputPort = 0;
        public float _curMixDuration;
        public float _curMixDuration_elapsed;
        private readonly int mixCount = 2;

        /// <summary> 是否正在播放 </summary>
        public bool isPlaying => this._isPlaying;
        private bool _isPlaying;

        /// <summary> 当前播放名称 </summary>
        public string playingName => this._playingName;
        private string _playingName;

        /// <summary> 当前播放时间长度 </summary>
        public float playingDuration => this._playingDuration;
        private float _playingDuration;

        /// <summary> 当前是否为循环播放 </summary>
        public bool isLoop => this._isLoop;
        private bool _isLoop;

        /// <summary> 当前播放的片段 </summary>
        public AnimationClip playingClip { get; private set; }

        /// <summary> 当前播放的时间 </summary>
        public float elapsedTime
        {
            get
            {
                if (!this._graph.IsValid()) return -1.0f;
                var playable = this._graph.GetRootPlayable(0);
                if (!playable.IsValid()) return -1.0f;
                return (float)playable.GetTime();
            }
        }

        public GameAnimPlayableGraph(Animator animator)
        {
            this._graph = PlayableGraph.Create();
            this._graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            this._animator = animator;
            this._mixerPlayable = AnimationMixerPlayable.Create(this._graph, this.mixCount);
            this._playableOutput = AnimationPlayableOutput.Create(this._graph, "Animation", this._animator);
            this._playableOutput.SetSourcePlayable(this._mixerPlayable);
        }

        public void Destroy()
        {
            if (this._graph.IsValid())
            {
                this._graph.Destroy();
            }
        }

        public void Tick(float dt)
        {
            // 更新graph
            this._graph.Evaluate(dt);
            // 更新混合
            this._TickMixer(dt);
            // 循环检测
            if (!this._isLoop && this.elapsedTime >= this.playingDuration)
            {
                this.Stop();
            }
        }

        private void _TickMixer(float dt)
        {
            if (this._curMixDuration <= 0) return;
            this._curMixDuration_elapsed += dt;
            if (this._curMixDuration_elapsed < this._curMixDuration)
            {
                var weight = this._curMixDuration_elapsed / this._curMixDuration;
                this._mixerPlayable.SetInputWeight(this._curInputPort, 1.0f - weight);
                this._mixerPlayable.SetInputWeight(this._nextInputPort, weight);
            }
            else
            {
                this._mixerPlayable.SetInputWeight(this._curInputPort, 0.0f);
                this._mixerPlayable.SetInputWeight(this._nextInputPort, 1.0f);
                this._mixerPlayable.DisconnectInput(this._curInputPort);
                this._curInputPort = this._nextInputPort;
                this._nextInputPort = -1;
                this._curMixDuration = 0;
                this._curMixDuration_elapsed = 0;
            }
        }

        /// <summary> 播放动画
        /// <para>name: 动画片段名称. 必须是已经设置的片段</para>
        /// <para>startTime: 开始时间</para>
        /// <para>mixDuration: 混合时间</para>
        /// </summary>
        public void Play(string name, bool isLoop, float startTime, float mixDuration)
        {
            if (!this._clipDict.TryGetValue(name, out var clip))
            {
                GameLogger.LogError($"动画片段不存在: {name}");
                return;
            }
            this.Stop();

            // 设置动画作为playable的输出
            var clipPlayable = AnimationClipPlayable.Create(this._graph, clip);
            clipPlayable.SetDuration(isLoop ? double.MaxValue : clip.length);
            if (mixDuration > 0)
            {
                this._nextInputPort = (this._curInputPort + 1) % this.mixCount;
                this._mixerPlayable.DisconnectInput(this._nextInputPort);
                this._mixerPlayable.ConnectInput(this._nextInputPort, clipPlayable, 0);
                this._mixerPlayable.SetInputWeight(this._curInputPort, 1.0f);
                this._mixerPlayable.SetInputWeight(this._nextInputPort, 0.0f);
            }
            else
            {
                this._curInputPort = 0;
                this._nextInputPort = -1;
                this._DisconnectAllMixerInputs();
                this._mixerPlayable.ConnectInput(0, clipPlayable, 0);
                this._mixerPlayable.SetInputWeight(0, 1.0f);
                this._mixerPlayable.SetInputWeight(1, 0.0f);
            }
            this._curMixDuration = mixDuration;
            this._curMixDuration_elapsed = 0;

            // 播放graph，设置开始时间
            this._graph.Play();
            this._graph.GetRootPlayable(0).SetTime(startTime);

            // 刷新当前播放信息
            // 设置clip的loop
            this.playingClip = clip;
            this._playingName = name;
            this._playingDuration = clip.length;
            this._isLoop = isLoop;
            this._isPlaying = true;
        }

        private void _DisconnectAllMixerInputs()
        {
            for (int i = 0; i < this.mixCount; i++)
            {
                this._mixerPlayable.DisconnectInput(i);
            }
        }

        /// <summary> 设置动画片段 </summary>
        public void SetClip(string name, AnimationClip clip)
        {
            if (!this._clipDict.ContainsKey(name))
            {
                this._clipDict.Add(name, clip);
            }
        }

        public void Stop()
        {
            // 停止graph
            this._graph.Stop();
            // 清空播放信息
            this.playingClip = null;
            this._playingName = string.Empty;
            this._playingDuration = -1.0f;
            this._isLoop = false;
            this._isPlaying = false;
        }

        public bool IsValid()
        {
            return this._graph.IsValid();
        }

        public bool TryGetClip(string name, out AnimationClip clip)
        {
            return this._clipDict.TryGetValue(name, out clip);
        }
    }
}