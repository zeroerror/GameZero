using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine.Analytics;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameRoleAIDomain_Follow
    {
        GameContext _context;
        GameRoleContext _roleContext => this._context.roleContext;

        public GameRoleAIDomain_Follow()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Destroy()
        {
        }

        public void Tick(GameRoleEntity role, float dt)
        {
            var inputCom = role.inputCom;
            if (inputCom.HasInput())
            {
                // 已有输入, 切换至攻击状态
                this._context.domainApi.roleAIApi.TryEnter(role, GameRoleAIStateType.Attack);
                return;
            }

            var followState = role.aiCom.followState;
            var followEntity = followState.followEntity;
            if (followState.isFarAway())
            {
                followState.farAwayDirty = true;
                inputCom.moveDir = (followEntity.logicBottomPos - role.logicBottomPos).normalized;
                return;
            }

            if (followState.farAwayDirty)
            {
                if (!followState.isNearBy())
                {
                    // 触发了一次远离后, 需要移动到更近的位置
                    inputCom.moveDir = (followEntity.logicBottomPos - role.logicBottomPos).normalized;
                }
                else
                {
                    // 距离足够近, 切换至待机状态
                    followState.farAwayDirty = false;
                    this._context.domainApi.roleAIApi.TryEnter(role, GameRoleAIStateType.Idle);
                }
            }

            return;
        }

    }
}