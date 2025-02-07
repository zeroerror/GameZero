namespace GamePlay.Bussiness.Renderer
{
    public interface GameDirectorDomainApiR
    {
        /// <summary> 有限状态机接口 </summary>
        public GameDirectorFSMDomainApiR directorFSMApi { get; }

        /// <summary> 设置时间缩放 </summary>
        public void SetTimeScale(float timeScale);
    }
}