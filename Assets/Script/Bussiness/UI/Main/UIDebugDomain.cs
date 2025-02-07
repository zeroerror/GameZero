using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public class UIDebugDomain
    {
        public UIContext context { get; private set; }

        private int _enterStage;// 0-无输入阶段 1-输入阶段
        private int _enterId;// 输入的buffId

        public UIDebugDomain()
        {
        }

        public void Destroy()
        {
        }

        public void Inject(UIContext context)
        {
            this.context = context;
        }

        public void Tick()
        {
            this._TickDebugInput_TimeScale();
            this._TickDebugInput_AddBuffToUserRole();
        }

        private void _TickDebugInput_TimeScale()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                var timeScale = this.context.director.timeScaleCom.timeScale;
                timeScale = timeScale == 1.0f ? 0.1f : timeScale == 0.1f ? 0.0f : 1.0f;
                this.context.rendererApi.directorApi.SetTimeScale(timeScale);
                this.context.logicApi.directorApi.SetTimeScale(timeScale);
                this.context.director.timeScaleCom.SetTimeScale(timeScale);
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                var timeScale = this.context.director.timeScaleCom.timeScale;
                timeScale = timeScale == 1.0f ? 2.0f : timeScale == 2.0f ? 3.0f : 1.0f;
                this.context.rendererApi.directorApi.SetTimeScale(timeScale);
                this.context.logicApi.directorApi.SetTimeScale(timeScale);
                this.context.director.timeScaleCom.SetTimeScale(timeScale);
            }
        }

        private void _TickDebugInput_AddBuffToUserRole()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                var userRole_l = this.context.logicApi.roleApi.GetUserRole();
                if (userRole_l == null) return;
                this._enterStage = (this._enterStage + 1) % 2;
                if (this._enterStage == 1)
                {
                    this._enterId = 0;
                    GameLogger.DebugLog("请输入Id:");
                }
                else
                {
                    GameLogger.DebugLog("当前输入Id:" + this._enterId);
                }
            }
            // 输入阶段
            if (this._enterStage == 1)
            {
                var keys = Input.inputString;
                if (keys?.Length != 0)
                {
                    if (keys == "\b")
                    {
                        if (this._enterId > 0) this._enterId /= 10;// 退格
                    }
                    else
                    {
                        var key = keys[0];
                        if (key >= '0' && key <= '9') this._enterId = this._enterId * 10 + (key - '0');// 输入数字
                    }
                }
                // ctrl + v的输入也要处理
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
                {
                    var text = GUIUtility.systemCopyBuffer;
                    if (int.TryParse(text, out var actionId))
                    {
                        this._enterId = actionId;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (this._enterId > 0)
                {
                    var userRole_l = this.context.logicApi.roleApi.GetUserRole();
                    if (userRole_l)
                    {
                        GameLogger.DebugLog("执行行为:" + this._enterId);
                        userRole_l.actionTargeterCom.SetTargeter(new GameActionTargeterArgs { targetEntity = userRole_l, targetDirection = userRole_l.transformCom.forward, targetPosition = userRole_l.transformCom.position });
                        this.context.logicApi.actionApi.DoAction(this._enterId, userRole_l);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                if (this._enterId > 0)
                {
                    var userRole_l = this.context.logicApi.roleApi.GetUserRole();
                    if (userRole_l)
                    {
                        GameLogger.DebugLog("执行Buff:" + this._enterId);
                        this.context.logicApi.buffApi.TryAttachBuff(this._enterId, userRole_l, userRole_l, 1, out var realAttachLayer);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (this._enterId > 0)
                {
                    var userRole_l = this.context.logicApi.roleApi.GetUserRole();
                    if (userRole_l)
                    {
                        GameLogger.DebugLog("执行行为选项:" + this._enterId);
                        this.context.logicApi.actionApi.DoActionOption(this._enterId, userRole_l.idCom.campId);
                    }
                }
            }
        }
    }
}