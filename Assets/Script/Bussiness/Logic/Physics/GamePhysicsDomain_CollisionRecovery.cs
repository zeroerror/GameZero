using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;

namespace GamePlay.Bussiness.Logic
{
    public class GamePhysicsDomain_CollisionRecovery
    {
        GameContext _context;
        GamePhysicsContext _physicsContext => _context.physicsContext;

        public GamePhysicsDomain_CollisionRecovery()
        {
        }

        public void Dispose()
        {
        }

        public void Inject(GameContext context)
        {
            this._context = context;
        }

        public void Tick(float dt)
        {
            var physicsComs_notTrigger = _physicsContext.physicsComs_notTrigger;
            for (int i = 0; i < physicsComs_notTrigger.Count; i++)
            {
                var physicsCom1 = physicsComs_notTrigger[i];
                var collider1 = physicsCom1.collider;
                if (collider1 == null || !collider1.isEnable) continue;
                for (int j = i + 1; j < physicsComs_notTrigger.Count; j++)
                {
                    var physicsCom2 = physicsComs_notTrigger[j];
                    var collider2 = physicsCom2.collider;
                    if (collider2 == null || !collider2.isEnable) continue;
                    var mtv = GamePhysicsResolvingUtil.GetResolvingMTV(collider1, collider2);
                    if (mtv == GameVec2.zero) continue;
                    var mtv1 = mtv * 0.5f;
                    var mtv2 = -mtv1;
                    collider1.binder.transformCom.position += mtv1;
                    collider2.binder.transformCom.position += mtv2;
                }
            }
        }
    }
}
