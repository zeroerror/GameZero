using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
/// <summary>
/// 游戏动画Playable组件
/// </summary>
public class GameAnimPlayableCom
{
    private Dictionary<string, PlayableGraph> _graphDict = new Dictionary<string, PlayableGraph>();
    public Animator animator { get; private set; }
    public bool isPause { get; private set; }
    public float timeScale { get; set; } = 1.0f;

    public GameAnimPlayableCom(Animator animator)
    {
        this.animator = animator;
    }

    public void Dispose()
    {
        this._graphDict.Clear();
    }

    public void Tick(float dt)
    {
        if (this.isPause) return;
        if (!this.currentGraph.IsValid()) return;
        this.currentGraph.Evaluate(dt * this.timeScale);
    }

    public void Play(AnimationClip clip)
    {
        if (!this._graphDict.TryGetValue(clip.name, out var cacheGraph))
        {
            cacheGraph = this._Create(clip);
            if (!cacheGraph.IsValid())
            {
                GameLogger.Error($"Create PlayableGraph Failed: {clip.name}");
                return;
            }
            this._graphDict.Add(clip.name, cacheGraph);
        }
        cacheGraph.Play();
        this.currentGraph = cacheGraph;
        this.isPause = false;
    }
    public PlayableGraph currentGraph { get; private set; }

    private PlayableGraph _Create(AnimationClip clip)
    {
        var graph = PlayableGraph.Create();
        var playable = AnimationClipPlayable.Create(graph, clip);
        var output = AnimationPlayableOutput.Create(graph, "output", this.animator);
        output.SetSourcePlayable(playable);
        graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
        GameLogger.Log($"Create PlayableGraph: {clip.name}");
        return graph;
    }

    public void Pause(string name)
    {
        if (!this._graphDict.TryGetValue(name, out var cacheGraph)) return;
        if (!cacheGraph.IsValid()) return;
        this.isPause = true;
    }
}
