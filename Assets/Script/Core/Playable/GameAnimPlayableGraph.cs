using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using GamePlay.Infrastructure;

namespace GamePlay.Core
{
    /// <summary>
    /// 基于 Playable 的动画控制组件, 用于平替 Animator
    /// <para> 一个Graph可以缓存多个Clip, 但只存在一个正在播放的Clip </para>
    /// </summary>
    public class GameAnimPlayableGraph
    {
        private Animator _animator;
        private PlayableGraph _graph;
        private Dictionary<string, AnimationClip> _clipDict;

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

        /// <summary> 当前播放的片段 </summary>
        public AnimationClip playingClip { get; private set; }

        public GameAnimPlayableGraph(Animator animator)
        {
            this._graph = PlayableGraph.Create();
            this._graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            this._animator = animator;
            this._clipDict = new Dictionary<string, AnimationClip>();
            this._SetClipOutput(null);
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
            this._graph.Evaluate(dt);
            if (!this._isLoop && this.elapsedTime >= this.playingDuration)
            {
                this.Stop();
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

        /// <summary> 播放动画
        /// <para>name: 动画片段名称. 必须是已经设置的片段</para>
        /// <para>startTime: 开始时间</para>
        /// </summary>
        public void Play(string name, float startTime = 0.0f)
        {
            if (!this._clipDict.TryGetValue(name, out var clip))
            {
                GameLogger.LogError($"动画片段不存在: {name}");
                return;
            }
            this.Stop();

            // 设置动画作为playable的输出
            this._SetClipOutput(clip);

            this._graph.Play();
            this._graph.GetRootPlayable(0).SetTime(startTime);

            // 记录当前播放信息
            this.playingClip = clip;
            this._playingName = name;
            this._playingDuration = clip.length;
            this._isLoop = clip.isLooping;
            this._isPlaying = true;
        }

        /// <summary> 设置动画片段的输出 </summary>
        private void _SetClipOutput(AnimationClip clip)
        {
            var clipPlayable = AnimationClipPlayable.Create(this._graph, clip);
            var playableOutput = AnimationPlayableOutput.Create(this._graph, "Animation", this._animator);
            playableOutput.SetSourcePlayable(clipPlayable);
        }

        public void Stop()
        {
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