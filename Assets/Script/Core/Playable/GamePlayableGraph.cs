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

        /// <summary> 当前播放名称, 多动画片段播放时默认为第一个 </summary>
        public string playingName => this.playingClips[0]?.name ?? string.Empty;

        /// <summary> 当前播放时间长度, 多动画片段播放时默认为第一个 </summary>
        public float playingDuration => this.playingClips[0]?.length ?? -1.0f;

        /// <summary> 当前是否为循环播放, 多动画片段播放时默认为第一个 </summary>
        public bool isLoop
        {
            get
            {
                var isOutOfIndex = this.playingClips.Count == 0;
                if (isOutOfIndex)
                {
                    GameLogger.LogError("当前播放片段为空");
                }
                return this.playingClips[0]?.isLooping ?? false;
            }
        }

        /// <summary> 当前播放的时间 </summary>
        public float time => (float)this._graph.GetRootPlayable(0).GetTime();

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

            GameLogger.LogWarning($"名称: {this.playingName}, 时间: {this.time}/{this.playingDuration}");
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
        }

        public void Stop()
        {
            this._graph.Stop();
            this.playingClips.Clear();
        }

        public bool IsValid()
        {
            return this._graph.IsValid();
        }

        public bool IsPlaying()
        {
            if (!this.IsValid()) return false;
            return this._graph.IsPlaying();
        }

        public bool TryGetClip(string name, out AnimationClip clip)
        {
            return this._clipDict.TryGetValue(name, out clip);
        }
    }
}