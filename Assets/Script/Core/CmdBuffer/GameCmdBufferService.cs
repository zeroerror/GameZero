using System;
using System.Collections.Generic;

namespace GamePlay.Core
{
    public class GameCmdBufferService
    {
        List<GameCmdBuffer> _cmdBuffers;

        public GameCmdBufferService()
        {
            _cmdBuffers = new List<GameCmdBuffer>();
        }

        public void Add(float delay, Action cmd)
        {
            _cmdBuffers.Add(new GameCmdBuffer() { delay = delay, cmd = cmd });
        }

        public void Tick()
        {
            var count = _cmdBuffers.Count;
            for (int i = 0; i < count; i++)
            {
                var cmdBuffer = _cmdBuffers[i];
                cmdBuffer.elapseTime += UnityEngine.Time.deltaTime;
                if (cmdBuffer.isDone())
                {
                    cmdBuffer.cmd();
                    _cmdBuffers.RemoveAt(i);
                    i--;
                    count--;
                }
            }
        }
    }
}