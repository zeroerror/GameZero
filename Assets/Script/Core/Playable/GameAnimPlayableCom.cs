using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Core
{
    /// <summary>
    /// 基于 Playable 的动画控制组件, 用于平替 Animator
    /// </summary>
    public class GameAnimPlayableCom
    {
        public Animator animator { get; private set; }
        public bool isPlaying { get; private set; }
        public float timeScale { get; set; } = 1.0f;

        /// <summary> Graph 字典 </summary>
        private Dictionary<int, GameAnimPlayableGraph> _graphDict;

        /// <summary>
        /// <para>animator: 动画控制器</para>
        /// </summary>
        public GameAnimPlayableCom(Animator animator)
        {
            this.animator = animator;
            animator.runtimeAnimatorController = null;// 取消默认的动画控制器
            this._graphDict = new Dictionary<int, GameAnimPlayableGraph>();
        }

        public void Destroy()
        {
            foreach (var kvp in this._graphDict)
            {
                kvp.Value.Destroy();
            }
        }

        public void Tick(float dt)
        {
            // 状态检测
            if (!this.isPlaying) return;

            // 时间缩放
            dt *= this.timeScale;

            var hasPlaying = false;
            foreach (var kvp in this._graphDict)
            {
                var graph = kvp.Value;
                if (!graph.isPlaying) continue;
                hasPlaying = true;

                graph.Tick(dt);

                if (!graph.isLoop && graph.time >= graph.playingDuration)
                {
                    graph.Stop();
                }
            }
            this.isPlaying = hasPlaying;
        }

        /// <summary>
        /// 播放动画
        /// <para>clip: 动画片段</para>
        /// <para>startTime: 开始时间</para>
        /// <para>layer: 层级</para>
        /// </summary>
        public void Play(AnimationClip clip, int layer = 0, float startTime = 0f)
        {
            if (!clip)
            {
                GameLogger.LogError("播放Clip动画失败! clip为空");
                return;
            }

            var graph = this._CreateGraph(layer);
            graph.SetClip(clip.name, clip);
            this.Play(layer, clip.name, startTime);
        }

        private GameAnimPlayableGraph _CreateGraph(int layer)
        {
            if (!this._graphDict.TryGetValue(layer, out var graph))
            {
                graph = new GameAnimPlayableGraph(this.animator);
                this._graphDict[layer] = graph;
            }
            return graph;
        }

        public void Play(int layer, string name, float startTime = 0f)
        {
            if (!this._graphDict.TryGetValue(layer, out var graph))
            {
                GameLogger.LogError($"播放动画失败! Graph 不存在, layer: {layer}");
                return;
            }
            if (!graph.IsValid())
            {
                GameLogger.LogError($"播放动画失败! Graph 无效, layer: {layer}");
                return;
            }

            graph.Play(name, startTime);
            this.isPlaying = true;
        }

        public bool TryGetClip(string name, out AnimationClip clip)
        {
            clip = null;
            foreach (var kvp in this._graphDict)
            {
                var graph = kvp.Value;
                if (graph.TryGetClip(name, out clip))
                {
                    return true;
                }
            }
            return false;
        }

        public void Stop()
        {
            this.isPlaying = false;
            foreach (var kvp in this._graphDict)
            {
                var graph = kvp.Value;
                graph.Stop();
            }
        }
    }
}