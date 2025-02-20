using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace GamePlay.Infrastructure
{
    /// <summary>
    /// 游戏音频播放器
    /// <para> 用于播放单个音频片段, 由Tick驱动播放 </para>
    /// </summary>
    public class GameAudioPlayable
    {
        public readonly string url;
        private readonly AudioSource _audioSource;
        private readonly PlayableGraph _playableGraph;
        private readonly AudioClipPlayable _clipPlayable;
        private readonly AudioPlayableOutput _audioPlayableOutput;

        public AudioClip clip { get; private set; }
        public bool isPlaying => this._clipPlayable.GetTime() < this.clip.length;
        public float length => this.clip.length;
        public float volume { get => this._audioSource.volume; set => this._audioSource.volume = value; }
        public double time => this._clipPlayable.GetTime();

        public GameAudioPlayable(string url, AudioSource audioSource)
        {
            this.url = url;
            this._audioSource = audioSource;
            this.clip = audioSource.clip;
            audioSource.clip = null;

            // 创建一个Graph
            this._playableGraph = PlayableGraph.Create();
            this._playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            // 根据 Graph 和 Clip , 建立 音频片段
            this._clipPlayable = AudioClipPlayable.Create(this._playableGraph, this.clip, false);
            // 根据 Graph 和 AudioSource , 建立 音频输出
            this._audioPlayableOutput = AudioPlayableOutput.Create(this._playableGraph, "AudioOutPut", this._audioSource);
            // 将 音频片段 与 音频输出 关联
            this._audioPlayableOutput.SetSourcePlayable(this._clipPlayable);
        }

        public void Destroy()
        {
            this._playableGraph.Destroy();
        }

        public void Play(float startTime = 0, bool loop = false)
        {
            this._clipPlayable.SetLooped(loop);
            this._clipPlayable.SetTime(startTime);
            this._playableGraph.Play();
        }

        public void Stop()
        {
            this._playableGraph.Stop();
        }

        public void SetVolume(float volume)
        {
            this._audioSource.volume = volume;
        }

        public void SetSpeed(float speed)
        {
            this._clipPlayable.SetSpeed(speed);
        }
    }
}
