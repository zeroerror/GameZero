using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
/// <summary>
/// 动态播放AnimationClip的组件
/// </summary>
namespace GamePlay.Core
{
    public class GamePlayableCom
    {
        private Dictionary<string, PlayableGraph> _graphDict = new Dictionary<string, PlayableGraph>();
        public Animator animator { get; private set; }
        public bool isPause { get; private set; }
        public float timeScale { get; set; } = 1.0f;
        public PlayableGraph currentGraph { get; private set; }

        public bool IsPlaying => this.currentGraph.IsValid() && this.currentGraph.IsPlaying();
        public float _duration { get; private set; }

        public GamePlayableCom(Animator animator)
        {
            this.animator = animator;
            animator.runtimeAnimatorController = null;
        }

        public void Destroy()
        {
            var vs = this._graphDict.Values;
            foreach (var v in vs)
            {
                v.Destroy();
            }
            this._graphDict.Clear();
        }

        public void Tick(float dt)
        {
            if (this.isPause) return;
            if (!this.currentGraph.IsValid()) return;
            dt *= this.timeScale;
            this.currentGraph.Evaluate(dt);
            var time = this.GetCurrentTime();
            if (time >= this._duration)
            {
                this.currentGraph.Stop();
            }
        }

        public void Play(AnimationClip clip, float starTime = 0)
        {
            if (!clip)
            {
                GameLogger.LogError("播放Clip动画失败! clip为空");
                return;
            }
            if (!this._graphDict.TryGetValue(clip.name, out var cacheGraph))
            {
                cacheGraph = this._Create(clip);
                this._graphDict.Add(clip.name, cacheGraph);
            }
            clip.events = null;
            this.Play(clip.name, starTime);
        }

        public void Play(string name, float starTime = 0)
        {
            if (!this._graphDict.TryGetValue(name, out var graph))
            {
                GameLogger.LogError($"播放名称动画失败! {name} 不存在");
                return;
            }
            if (!graph.IsValid())
            {
                GameLogger.LogError($"播放名称动画失败! {name} 无效");
                return;
            }
            graph.Play();
            this._SetTime(graph, starTime);
            this.currentGraph = graph;
            this.isPause = false;
            this._duration = this._GetDuration(graph);
        }

        private PlayableGraph _Create(AnimationClip clip)
        {
            var graph = PlayableGraph.Create();
            var playable = AnimationClipPlayable.Create(graph, clip);
            var output = AnimationPlayableOutput.Create(graph, clip.name, this.animator);
            output.SetSourcePlayable(playable);
            graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            return graph;
        }

        public bool hasClip(string name)
        {
            return this._graphDict.ContainsKey(name);
        }

        public void Pause(string name)
        {
            if (!this._graphDict.TryGetValue(name, out var cacheGraph)) return;
            if (!cacheGraph.IsValid()) return;
            this.isPause = true;
        }

        /// <summary> 获取当前Graph的时间 </summary>
        public double GetCurrentTime()
        {
            var graph = this.currentGraph;
            if (!graph.IsValid()) return 0;
            var rootPlayable = graph.GetRootPlayable(0); // 获取根节点
            if (!rootPlayable.IsValid()) return 0;
            return rootPlayable.GetTime(); // 获取当前时间
        }

        /// <summary> 获取 Graph 的时长 </summary>
        private float _GetDuration(in PlayableGraph graph)
        {
            if (!graph.IsValid()) return 0;
            var rootPlayable = graph.GetRootPlayable(0);
            if (!rootPlayable.IsValid()) return 0;
            if (rootPlayable.IsPlayableOfType<AnimationClipPlayable>())
            {
                var clipPlayable = (AnimationClipPlayable)rootPlayable;
                var clip = clipPlayable.GetAnimationClip();
                if (clip != null)
                {
                    return clip.length;
                }
            }
            return 0;
        }

        private void _SetTime(PlayableGraph graph, double time)
        {
            if (!graph.IsValid()) return;
            var rootPlayable = graph.GetRootPlayable(0); // 获取根节点
            if (!rootPlayable.IsValid()) return;
            rootPlayable.SetTime(time); // 设置当前时间
        }

    }
}
