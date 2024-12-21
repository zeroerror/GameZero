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

        /// <summary>
        /// 添加一个延迟执行的命令
        /// <para>delay: 延迟时间(秒). 0表示会在当前帧的帧末就执行</para>
        /// <para>cmd: 命令</para>
        /// @return 返回指令Id
        /// </summary>
        public int AddDelayCmd(float delay, Action cmd)
        {
            _cmdBuffers.Add(new GameCmdBuffer() { delay = delay, cmd = cmd });
            return _cmdBuffers.Count - 1;
        }

        /// <summary>
        /// 添加一个间隔执行的命令
        /// <para>interval: 间隔时间(秒)</para>
        /// <para>cmd: 命令</para>
        /// @return 返回指令Id
        /// </summary>
        public int AddIntervalCmd(float interval, Action cmd)
        {
            _cmdBuffers.Add(new GameCmdBuffer() { interval = interval, cmd = cmd });
            return _cmdBuffers.Count - 1;
        }

        /// <summary>
        /// 移除一个指令
        /// <para>id: 指令Id</para>
        /// </summary>
        public void Remove(int id)
        {
            _cmdBuffers.RemoveAt(id);
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
                    if (cmdBuffer.interval != 0) continue;
                    _cmdBuffers.RemoveAt(i);
                    i--;
                    count--;
                }
            }
        }


        private class GameCmdBuffer
        {
            public Action cmd;
            public float delay;
            public float elapseTime;
            public float interval;
            private int _intervalCount;

            public bool isDone()
            {
                if (elapseTime < delay) return false;
                if (interval == 0) return true;

                var time = elapseTime - delay;
                var newCount = (int)(time / interval);
                if (newCount > _intervalCount)
                {
                    _intervalCount = newCount;
                    return true;
                }
                return false;
            }
        }
    }
}