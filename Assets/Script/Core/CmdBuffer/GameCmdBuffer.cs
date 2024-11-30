using System;
namespace GamePlay.Core
{
    public class GameCmdBuffer
    {
        public Action cmd;
        public float delay;
        public float elapseTime;
        public bool isDone()
        {
            return elapseTime >= delay;
        }

    }
}