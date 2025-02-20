using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Audio;

public class ManualAudioLoop : MonoBehaviour
{
    public AudioClip audioClip;
    private PlayableGraph playableGraph;
    private AudioClipPlayable audioClipPlayable;

    void Start()
    {
        // 创建 PlayableGraph
        playableGraph = PlayableGraph.Create("ManualAudioGraph");
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);

        // 创建 AudioSource
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        var audioOutput = AudioPlayableOutput.Create(playableGraph, "AudioOutput", audioSource);

        // 创建 AudioClipPlayable，启用循环播放
        audioClipPlayable = AudioClipPlayable.Create(playableGraph, audioClip, true); // `true` 代表循环
        audioOutput.SetSourcePlayable(audioClipPlayable);

        // 播放之前设置时间，确保从正确的地方开始
        audioClipPlayable.SetTime(0.0f);

        playableGraph.Play();
    }

    void Update()
    {
        var updateMode = playableGraph.GetTimeUpdateMode();
        if (updateMode == DirectorUpdateMode.Manual)
        {
            // 手动推进 PlayableGraph
            playableGraph.Evaluate(0.02f);

            // 强制同步音频时间
            audioClipPlayable.SetTime(audioClipPlayable.GetTime() + 0.02f);
        }
    }

    void OnDestroy()
    {
        playableGraph.Destroy(); // 清理资源
    }
}
