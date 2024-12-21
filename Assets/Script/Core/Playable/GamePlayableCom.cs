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

        // public bool IsPlaying => this.currentGraph.IsValid() && this.currentGraph.IsPlaying();
        public bool IsPlaying => this.time < 0.5;
        public float time { get; private set; }

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
            this.time += dt;
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
            graph.GetRootPlayable(0).SetTime(starTime);
            this.time = starTime;
            this.currentGraph = graph;
            this.isPause = false;
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

    }
}
