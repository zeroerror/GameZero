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
        public bool IsPlaying => this.getCurrentGraph().IsValid() && this.getCurrentGraph().IsPlaying();

        private Dictionary<string, PlayableGraph> _graphDict;
        private AnimationLayerMixerPlayable _layerMixerPlayable;
        private AnimationClip[] _currentClips;

        /// <summary>
        /// <para>animator: 动画控制器</para>
        /// </summary>
        public GamePlayableCom(Animator animator, int maxLayer = 10)
        {
            this.animator = animator;
            animator.runtimeAnimatorController = null;// 取消默认的动画控制器

            this._graphDict = new Dictionary<string, PlayableGraph>();

            this._currentClips = new AnimationClip[maxLayer];
        }

        public void Destroy()
        {
            this.StopAll();
            // this._layerMixerPlayable.Destroy();
            // foreach (var v in this._graphDict.Values)
            // {
            //     v.Destroy();
            // }
            // this._graphDict.Clear();
        }

        public void Tick(float dt)
        {
            // 状态检测
            if (this.isPause) return;
            if (!this.getCurrentGraph().IsValid()) return;

            // 时间缩放
            dt *= this.timeScale;

            // 更新当前的 Graph
            this._currentClips?.Foreach((clip, layer) =>
            {
                if (!clip) return;
                var graphName = clip.name;
                if (!this._graphDict.TryGetValue(graphName, out var layerGraph)) return;
                if (!layerGraph.IsValid()) return;
                layerGraph.Evaluate(dt);

                // 判断是否播放完毕
                var curGraphTime = this.GetCurrentGraphTime(layer);
                var curClipLength = this.GetCurrentClipLength(layer);
                if (curGraphTime >= curClipLength && !clip.isLooping)
                {
                    this._Stop(graphName);
                }
            });
        }

        /// <summary>
        /// 播放动画
        /// <para>clip: 动画片段</para>
        /// <para>startTime: 开始时间</para>
        /// <para>layer: 层级</para>
        /// <para>weight: 权重</para>
        /// </summary>
        public void Play(AnimationClip clip, int layer = 0, float weight = 0.5f, float startTime = 0f)
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
            this.Play(clip.name, layer, weight, startTime);
        }

        /// <summary>
        /// 播放已有动画
        /// <para>name: 动画名称</para>
        /// <para>layer: 层级</para>
        /// <para>weight: 权重</para>
        /// <para>startTime: 开始时间</para>
        /// </summary>
        public void Play(string name, int layer = 0, float weight = 0.5f, float startTime = 0f)
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

            // 保证当前动画片段的正确连接
            var clipPlayable = graph.GetRootPlayable(0);
            if (!this._layerMixerPlayable.IsGraphEquals(layer, clipPlayable.GetGraph()))
            {
                this._layerMixerPlayable.ConnectInput(layer, clipPlayable, 0);
            }

            // 设置权重
            this._layerMixerPlayable.SetInputWeight(layer, weight);

            // 设置起始时间
            this._SetTime(graph, startTime);

            // 开始播放动画
            graph.Play();

            // 参数刷新
            this.isPause = false;
        }

        private PlayableGraph _CreateGraph(AnimationClip clip, int layer = 0)
        {
            // 创建一个 Graph
            var graph = PlayableGraph.Create();
            graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);// 为了符合设计的逻辑控制流程，设置为手动更新

            // 创建一个 ClipPlayable
            AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(graph, clip);

            // 创建一个 LayerMixerPlayable
            this._layerMixerPlayable = AnimationLayerMixerPlayable.Create(graph, layer + 1);

            // 将 ClipPlayable 连接到 LayerMixerPlayable 的第一个输出端口（即索引 0）
            this._layerMixerPlayable.ConnectInput(0, clipPlayable, 0);

            // 将 Graph 和 Animator 绑定
            var output = AnimationPlayableOutput.Create(graph, clip.name, this.animator);

            // 将 LayerMixerPlayable 设置为 Graph 的输出
            output.SetSourcePlayable(this._layerMixerPlayable);

            // 记录层级的Clip
            this._currentClips[layer] = clip;

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

        public void StopAll()
        {
            this.isPause = true;
            this._currentClips?.Foreach((clip, index) =>
            {
                if (!clip) return;
                var graphName = clip.name;
                if (!this._graphDict.TryGetValue(graphName, out var cacheGraph)) return;
                if (!cacheGraph.IsValid()) return;
                cacheGraph.Stop();
                this._currentClips[index] = null;
            });
        }

        private void _Stop(string graphName)
        {
            if (!this._graphDict.TryGetValue(graphName, out var graph)) return;
            if (!graph.IsValid()) return;
            graph.Stop();
            for (int i = 0; i < this._currentClips.Length; i++)
            {
                var clip = this._currentClips[i];
                if (!clip) continue;
                if (clip.name == graphName)
                {
                    this._currentClips[i] = null;
                    break;
                }
            }
        }

        private void _SetTime(PlayableGraph graph, double time)
        {
            if (!graph.IsValid()) return;
            var rootPlayable = graph.GetRootPlayable(0); // 获取根节点
            if (!rootPlayable.IsValid()) return;
            rootPlayable.SetTime(time); // 设置当前时间
        }

        /// <summary>
        /// 获取当前 Graph
        /// <para>layer: 层级</para>
        /// </summary>
        public PlayableGraph getCurrentGraph(int layer = 0)
        {
            var clip = this._currentClips[layer];
            if (!clip) return default;
            this._graphDict.TryGetValue(clip.name, out var graph);
            return graph;
        }

        /// <summary>
        /// 获取当前 Graph 的时间
        /// <para>graph: Graph</para>
        /// </summary>
        private double GetCurrentGraphTime(in PlayableGraph graph)
        {
            if (!graph.IsValid()) return -1;
            var rootPlayable = graph.GetRootPlayable(0); // 获取根节点
            if (!rootPlayable.IsValid()) return -1;
            return rootPlayable.GetTime(); // 获取当前时间
        }

        /// <summary>
        /// 获取当前 Graph 的时间
        /// <para>layer: 层级</para>
        /// </summary>
        public double GetCurrentGraphTime(int layer = 0)
        {
            var graph = this.getCurrentGraph(layer);
            return this.GetCurrentGraphTime(graph);
        }

        /// <summary>
        /// 获取当前 Clip 的长度
        /// <para>layer: 层级</para>
        /// </summary>
        public float GetCurrentClipLength(int layer = 0)
        {
            var clip = this._currentClips[layer];
            if (!clip) return -1;
            return clip.length;
        }
    }
}