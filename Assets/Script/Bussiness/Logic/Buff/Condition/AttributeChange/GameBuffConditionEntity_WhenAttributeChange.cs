namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionEntity_WhenAttributeChange : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_WhenAttributeChange model { get; private set; }

        public GameBuffConditionEntity_WhenAttributeChange(GameBuffEntity buff, GameBuffConditionModel_WhenAttributeChange model) : base(buff)
        {
            this.model = model;
        }

        protected override bool _Check()
        {
            var actor = this._buff.owner;
            var targetEntity = this._buff.actionTargeterCom.getCurTargeter().targetEntity;
            var needRefTarget = this.model.refTypeA.IsTargetRef() || this.model.refTypeB.IsTargetRef();
            if (needRefTarget && !targetEntity)
            {
                // 需要参考目标, 但是目标不存在
                return false;
            }

            var refValueA = _GetRefValue(actor, targetEntity, this.model.valueA, this.model.valueFormatA, this.model.refTypeA);
            var refValueB = _GetRefValue(actor, targetEntity, this.model.valueB, this.model.valueFormatB, this.model.refTypeB);
            var compareResult = this.model.compareType.Compare(refValueA, refValueB);
            return compareResult;
        }

        private float _GetRefValue(GameEntityBase actor, GameEntityBase targetEntity, int value, GameActionValueFormat valueFormat, GameActionValueRefType refType)
        {
            // 数值格式化
            var formatValue = valueFormat.FormatValue(value);
            // 参考属性值
            var refAttrValue = refType.GetRefAttributeValue(actor, targetEntity);
            // 参考值
            var refValue = refAttrValue * formatValue;
            return refValue;
        }

        public override void Clear()
        {
        }
    }
}