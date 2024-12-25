using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GamePlay.Core
{
    public class GamePlayableGraph
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
        public float time => this._graph.IsValid() ? (float)this._graph.GetRootPlayable(0).GetTime() : 0.0f;

        /// <summary> 当前播放的所有片段, 可能有多个, 比如同时播放上半身和下半身动画 </summary>
        public List<AnimationClip> playingClips { get; private set; }

        public GamePlayableGraph(Animator animator)
        {
            this._graph = PlayableGraph.Create();
            this._animator = animator;
            this._clipDict = new Dictionary<string, AnimationClip>();
            this.playingClips = new List<AnimationClip>(4);
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
            if (!this.isLoop && this.time >= this.playingDuration)
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
            var playableOutput = AnimationPlayableOutput.Create(this._graph, "Animation", this._animator);
            var clipPlayable = AnimationClipPlayable.Create(this._graph, clip);
            playableOutput.SetSourcePlayable(clipPlayable);

            this.Stop();

            this._graph.Play();
            this._graph.GetRootPlayable(0).SetTime(startTime);
            this.playingClips.Add(clip);

            this._playingName = name;
            this._playingDuration = clip.length;
            this._isLoop = clip.isLooping;
            this._isPlaying = true;
        }

        public void Stop()
        {
            this._graph.Stop();
            this.playingClips.Clear();
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