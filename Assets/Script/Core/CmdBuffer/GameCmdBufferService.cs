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
        /// <para>delay: 延迟时间(秒). 0表示会在当前帧末执行</para>
        /// <para>cmd: 命令</para>
        /// @return 返回指令Id
        /// </summary>
        public int AddDelayCmd(float delay, Action cmd)
        {
            var cmdBuffer = new GameCmdBuffer() { delay = delay, cmd = cmd };
            _cmdBuffers.Add(cmdBuffer);
            return cmdBuffer.id;
        }

        /// <summary>
        /// 添加一个间隔执行的命令
        /// <para>interval: 间隔时间(秒)</para>
        /// <para>cmd: 命令</para>
        /// @return 返回指令Id
        /// </summary>
        public int AddIntervalCmd(float interval, Action cmd)
        {
            var cmdBuffer = new GameCmdBuffer() { interval = interval, cmd = cmd };
            _cmdBuffers.Add(cmdBuffer);
            return cmdBuffer.id;
        }

        /// <summary>
        /// 移除一个指令
        /// <para>id: 指令Id</para>
        /// </summary>
        public void Remove(int id)
        {
            var count = _cmdBuffers.Count;
            for (int i = 0; i < count; i++)
            {
                var cmdBuffer = _cmdBuffers[i];
                if (cmdBuffer.id == id)
                {
                    _cmdBuffers.RemoveAt(i);
                    return;
                }
            }
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
            private static int autoId = 0;
            public int id;

            public Action cmd;
            public float delay;
            public float elapseTime;
            public float interval;
            private int _intervalCount;

            public GameCmdBuffer()
            {
                id = autoId++;
                cmd = null;
                delay = 0;
                elapseTime = 0;
                interval = 0;
                _intervalCount = 0;
            }

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