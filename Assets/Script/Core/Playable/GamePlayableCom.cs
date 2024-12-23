using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GamePlay.Core
{
    public class GamePlayableCom
    {
        public Animator animator { get; private set; }
        public bool isPause { get; private set; }
        public float timeScale { get; set; } = 1.0f;
        public bool IsPlaying => this.currentGraph.IsValid() && this.currentGraph.IsPlaying();
        public float _duration { get; private set; }

        public PlayableGraph currentGraph
        {
            get
            {
                this._graphDict.TryGetValue(this._currentClipName, out var graph);
                return graph;
            }
        }

        /// <summary> 获取当前Graph的时间 </summary>
        public double currentTime
        {
            get
            {
                var graph = this.currentGraph;
                if (!graph.IsValid()) return 0;
                var rootPlayable = graph.GetRootPlayable(0); // 获取根节点
                if (!rootPlayable.IsValid()) return 0;
                return rootPlayable.GetTime(); // 获取当前时间
            }
        }

        private Dictionary<string, PlayableGraph> _graphDict;
        private AnimationLayerMixerPlayable _layerMixerPlayable;
        private string _currentClipName;

        /// <summary>
        /// <para>animator: 动画控制器</para>
        /// </summary>
        public GamePlayableCom(Animator animator)
        {
            this.animator = animator;
            animator.runtimeAnimatorController = null;// 取消默认的动画控制器

            this._graphDict = new Dictionary<string, PlayableGraph>();
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
            // 状态检测
            if (this.isPause) return;
            if (!this.currentGraph.IsValid()) return;

            // 时间缩放
            dt *= this.timeScale;

            // 更新当前的 Graph
            this.currentGraph.Evaluate(dt);

            // 判断是否播放完毕
            if (this.currentTime >= this._duration)
            {
                this.currentGraph.Stop();
            }
        }

        /// <summary>
        /// 播放动画
        /// <para>clip: 动画片段</para>
        /// <para>startTime: 开始时间</para>
        /// <para>layer: 层级</para>
        /// <para>weight: 权重</para>
        /// </summary>
        public void Play(AnimationClip clip, float startTime = 0, int layer = 0, float weight = 1.0f)
        {
            if (!clip)
            {
                GameLogger.LogError("播放Clip动画失败! clip为空");
                return;
            }

            // 没有缓存的 Graph, 创建一个新的 Graph
            if (!this._graphDict.TryGetValue(clip.name, out var cacheGraph))
            {
                cacheGraph = this._CreateGraph(clip);
                this._graphDict.Add(clip.name, cacheGraph);
            }

            // 动画片段已添加, 播放已有动画
            this.Play(clip.name, startTime, layer, weight);
        }

        /// <summary>
        /// 播放已有动画
        /// <para>name: 动画名称</para>
        /// <para>startTime: 开始时间</para>
        /// <para>layer: 层级</para>
        /// <para>weight: 权重</para>
        /// </summary>
        public void Play(string name, float startTime = 0, int layer = 0, float weight = 1.0f)
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

            // 如果层级大于输入的层级，则添加新的层级
            if (layer >= this._layerMixerPlayable.GetInputCount())
            {
                this._layerMixerPlayable.SetInputCount(layer + 1);
            }

            // 将当前的 ClipPlayable 连接到 LayerMixerPlayable 的第一个输出端口（即索引 0）
            // （保证当前动画片段的正确连接）
            var clipPlayable = graph.GetRootPlayable(0);
            this._layerMixerPlayable.DisconnectInput(layer);
            this._layerMixerPlayable.ConnectInput(layer, clipPlayable, 0);
            this._layerMixerPlayable.SetInputWeight(layer, weight);

            // 设置起始时间
            this._SetTime(graph, startTime);

            // 开始播放动画
            graph.Play();

            // 参数刷新
            this.isPause = false;
            this._duration = this._GetDuration(graph);
            this._currentClipName = name;
        }

        private PlayableGraph _CreateGraph(AnimationClip clip, int layer = 0)
        {
            // 创建一个 Graph
            var graph = PlayableGraph.Create();
            graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);// 为了符合设计的逻辑控制流程，设置为手动更新

            // 创建一个 ClipPlayable
            var clipPlayable = AnimationClipPlayable.Create(graph, clip);

            // 创建一个 LayerMixerPlayable
            this._layerMixerPlayable = AnimationLayerMixerPlayable.Create(graph, layer + 1);

            // 将 ClipPlayable 连接到 LayerMixerPlayable 的第一个输出端口（即索引 0）
            this._layerMixerPlayable.ConnectInput(layer, clipPlayable, 0);
            this._layerMixerPlayable.SetInputWeight(layer, 1.0f);

            // 将 Graph 和 Animator 绑定
            var output = AnimationPlayableOutput.Create(graph, clip.name, this.animator);

            // 将 LayerMixerPlayable 设置为 Graph 的输出
            output.SetSourcePlayable(this._layerMixerPlayable);

            // 返回创建的 Graph
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