using GamePlay.Core;
using UnityEditor;

namespace GamePlay.Bussiness.Logic
{
    public class GameBuffDomain : GameBuffDomainApi
    {
        GameContext _context;
        GameBuffContext _buffContext => this._context.buffContext;

        public GameBuffDomain()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(float dt)
        {
            this._context.roleContext.repo.ForeachAllEntities(role =>
            {
                this._TickBuff(role, dt);
            });
        }

        private void _TickBuff(GameRoleEntity role, float dt)
        {
            var buffCom = role.buffCom;
            for (int i = 0; i < buffCom.buffList.Count; i++)
            {
                var buff = buffCom.buffList[i];
                buff.Tick(dt);
                // 行为触发
                var isSatisfied_action = buff.conditionSetEntity_action.CheckSatisfied();
                if (isSatisfied_action)
                {
                    buff.model.actionIds?.Foreach(actionId =>
                    {
                        buff.physicsCom.ClearCollided();
                        this._context.domainApi.actionApi.DoAction(actionId, buff);
                    });
                }
                // 移除触发
                var isSatisfied_remove = buff.conditionSetEntity_remove.CheckSatisfied();
                if (isSatisfied_remove)
                {
                    this._context.cmdBufferService.AddDelayCmd(0, () =>
                    {
                        this.TryDetachBuff(role, buff.model.typeId, 0);
                    });
                }
            }
        }

        public bool AttachBuff(int typeId, GameEntityBase actor, GameEntityBase target, int layer, out int realAttachLayer)
        {
            realAttachLayer = 0;

            if (!target.TryGetLinkEntity<GameRoleEntity>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持挂载Buff");
                return false;
            }

            var buffCom = targetRole.buffCom;
            if (buffCom.TryGet(typeId, out var existBuff))
            {
                realAttachLayer = existBuff.AttachLayer(layer);
                _Attach(existBuff);
                return true;
            }

            var repo = this._buffContext.repo;
            if (!repo.TryFetch(typeId, out var newBuff))
            {
                newBuff = this._buffContext.factory.Load(typeId);
            }
            if (newBuff == null)
            {
                GameLogger.LogError("Buff创建失败, BuffID不存在：" + typeId);
                return false;
            }

            // 绑定父子关系
            newBuff.idCom.SetParent(targetRole);
            // 组件绑定
            newBuff.BindTransformCom(targetRole.transformCom);

            buffCom.Add(newBuff);
            this._buffContext.repo.TryAdd(newBuff);
            _Attach(newBuff);
            return true;

            // 内置方法
            void _Attach(GameBuffEntity buff)
            {
                // 设置行为目标
                buff.actionTargeterCom.SetTargeter(new GameActionTargeterArgs
                {
                    targetEntity = targetRole,
                    targetPosition = targetRole.transformCom.position,
                    targetDirection = targetRole.forward
                });
                // buff属性效果
                buff.model.attributeModels?.Foreach(attrModel =>
                {

                    var args = GameActionUtil_AttributeModify.CalcAttributeModify(actor, target, attrModel);
                    var attrType = args.modifyType;
                    var buffAttr = new GameAttribute(attrType, args.modifyValue);
                    var buffOldValue = buff.attributeCom.GetValue(attrType);// 记录buff属性效果旧值
                    buff.attributeCom.SetAttribute(buffAttr);

                    // 刷新buff对目标属性的影响
                    var roleOldValue = targetRole.attributeCom.GetValue(attrType);
                    var roleNewValue = roleOldValue + buffAttr.value - buffOldValue;
                    targetRole.attributeCom.SetAttribute(attrType, roleNewValue);

                    GameLogger.LogWarning($"附加Buff属性效果: {attrType} {roleOldValue} -> {roleNewValue}");
                });
                // 提交RC事件
                this._context.SubmitRC(GameBuffRCCollection.RC_GAME_BUFF_ATTACH, new GameBuffRCArgs_Attach
                {
                    buffIdArgs = buff.idCom.ToArgs(),
                    targetIdArgs = targetRole.idCom.ToArgs(),
                    layer = layer
                });
            }
        }

        public bool TryDetachBuff(GameEntityBase target, int buffId, int layer, out GameBuffEntity removeBuff, out int detachLayer)
        {
            removeBuff = null;
            detachLayer = 0;

            if (!target.TryGetLinkEntity<GameRoleEntity>(out var targetRole))
            {
                GameLogger.LogError("目标不是角色, 暂不支持移除Buff");
                return false;
            }

            var buffCom = targetRole.buffCom;
            if (!buffCom.TryDetachBuff(buffId, layer, out removeBuff, out detachLayer))
            {
                return false;
            }

            this._context.SubmitRC(GameBuffRCCollection.RC_GAME_BUFF_REMOVE, new GameBuffRCArgs_Remove
            {
                targetIdArgs = targetRole.idCom.ToArgs(),
                detachLayer = detachLayer,
            });

            return true;
        }

        public bool TryDetachBuff(GameEntityBase target, int buffId, int layer)
        {
            return this.TryDetachBuff(target, buffId, layer, out _, out _);
        }
    }
}