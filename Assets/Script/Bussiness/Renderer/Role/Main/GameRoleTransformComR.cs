using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleTransformComR
    {
        private GameRoleEntityR _role;

        /// <summary> 是否正在变身 </summary>
        public bool isTransforming => this.model != null;

        /// <summary> 模型 </summary>
        public GameRoleModelR model { get; private set; }
        /// <summary> 身体组件 </summary>
        public GameRoleBodyCom bodyCom { get; private set; }

        /// <summary> 技能组件 </summary>
        public GameSkillComR skillCom { get; private set; }
        /// <summary> 动画组件 </summary>
        public GamePlayableCom animCom { get; private set; }

        public GameRoleTransformComR(
            GameRoleEntityR role,
            Animator animator
        )
        {
            this._role = role;
            this.skillCom = new GameSkillComR(_role);
            this.animCom = new GamePlayableCom(animator);
        }

        public void StartTransform(GameRoleModelR model, GameRoleBodyCom bodyCom)
        {
            this.model = model;
            this.bodyCom = bodyCom;
            this.bodyCom.SetActive(true);
            this._role.originalBodyCom.SetActive(false);
        }

        public void EndTransform()
        {
            if (!this.isTransforming) return;
            this.bodyCom.Destroy();
            this.model = null;
            this.bodyCom = null;
            this.skillCom.Clear();
        }
    }
}