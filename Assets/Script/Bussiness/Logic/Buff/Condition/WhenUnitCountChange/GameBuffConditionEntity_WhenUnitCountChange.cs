using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    /// <summary>
    /// buff条件实体 - 当单位数量变化时
    /// </summary>
    public class GameBuffConditionEntity_WhenUnitCountChange : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_WhenUnitCountChange model { get; private set; }
        private int _lastCount;

        public GameBuffConditionEntity_WhenUnitCountChange(GameBuffEntity buff, GameBuffConditionModel_WhenUnitCountChange model) : base(buff)
        {
            this.model = model;
        }

        public override void Reset()
        {
            this._lastCount = 0;
        }

        protected override bool _Check()
        {
            List<GameActionTargeterArgs> targeterList = null;
            var selector = this.model.selector;
            var selectedEntities = this._domainApi.entitySelectApi.SelectEntities(selector, _buff, false);
            var owner = this._buff.owner;
            if (this._lastCount != selectedEntities.Count && selectedEntities.Count == this.model.countCondition)
            {
                _setSatisfied();
            }
            this._lastCount = selectedEntities.Count;

            void _setSatisfied()
            {
                targeterList = new List<GameActionTargeterArgs>();
                selectedEntities?.Foreach(target =>
                {
                    var targeter = new GameActionTargeterArgs(
                        target,
                        target.transformCom.position,
                        (target.transformCom.position - owner.transformCom.position).normalized
                    );
                    targeterList.Add(targeter);
                });
                return;
            }

            // 不满足时, 返回false
            if (targeterList == null)
            {
                return false;
            }

            // 满足时, 同步目标选取器列表到buff 
            this._buff.actionTargeterCom.SetTargeterList(targeterList);
            this._buff.actionTargeterCom.foreachType = GameForeachType.Sequential;
            return true;
        }
    }
}