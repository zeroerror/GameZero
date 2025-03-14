using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.UI;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;
namespace GamePlay.Bussiness.Render
{
    public class GameRoleDomainR : GameRoleDomainApiR
    {
        GameContextR _context;
        GameRoleContextR _roleContext => this._context.roleContext;

        public GameRoleInputDomainR inputDomain { get; private set; }
        public GameRoleFSMDomainR fsmDomain { get; private set; }
        public GameRoleFSMDomainApiR fsmApi => this.fsmDomain;

        public GameRoleDomainR()
        {
            this.inputDomain = new GameRoleInputDomainR();
            this.fsmDomain = new GameRoleFSMDomainR();
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this.inputDomain.Inject(context);
            this.fsmDomain.Inject(context);
        }

        public void Destroy()
        {
            this.fsmDomain.Destroy();
            this._roleContext.repo.ForeachEntities_IncludePool((entity) =>
            {
                entity.Destroy();
            });
        }

        public void BindEvents()
        {
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_TRANSFORM, this._OnTransformRole);
            this.fsmDomain.BindEvents();
        }

        public void UnbindEvents()
        {
            this._context.UnbindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
            this._context.UnbindRC(GameRoleRCCollection.RC_GAME_ROLE_TRANSFORM, this._OnTransformRole);
            this.fsmDomain.UnbindEvents();
        }

        private void _OnRoleCreate(object args)
        {
            if (this._context.fieldContext.curField == null)
            {
                this._context.DelayRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, args);
                return;
            }
            var rcArgs = (GameRoleRCArgs_Create)args;
            this.CreateRole(rcArgs.idArgs, rcArgs.transArgs);
        }

        private GameRoleEntityR _LoadRole(in GameIdArgs idArgs, in GameTransformArgs transArgs)
        {
            var repo = this._roleContext.repo;
            if (!repo.TryFetch(idArgs.typeId, out var role))
            {
                role = this._roleContext.factory.Load(idArgs.typeId);
                if (role == null)
                {
                    GameLogger.LogError("GameRoleDomainR._Create: typeId not found: " + idArgs.typeId);
                    return null;
                }
                this._context.domainApi.fielApi.AddToLayer(role.bodyCom.tmRoot, GameFieldLayerType.Entity);
                this._context.domainApi.fielApi.AddToLayer(role.bodyCom.shadow, GameFieldLayerType.Ground, 1);
            }
            role.idCom.SetByArgs(idArgs);
            role.bodyCom.tmRoot.name = $"role_{idArgs.typeId}_{role.idCom.entityId}";
            role.transformCom.SetByArgs(transArgs);
            role.SyncTrans();
            role.setActive(true);

            // 注入世界坐标转屏幕坐标的接口, 供组件内部调用更新属性条位置
            var attributeBarCom = role.attributeBarCom;
            attributeBarCom.WorldToScreenPoint = this.WorldToScreenPoint;

            // 将旧的血条则销毁, 根据当前阵营信息加载新的血条
            var existSlider = attributeBarCom.hpSlider.slider;
            if (existSlider) GameObject.Destroy(existSlider.gameObject);
            var isEnemy = role.idCom.campId != GameCampCollection.PLAYER_CAMP_ID;
            var uiHPSlider = this._roleContext.factory.LoadHPSlider(isEnemy);
            this._context.uiApi.layerApi.AddToUIRoot(uiHPSlider.transform, UILayerType.Scene);

            var hpSlider = attributeBarCom.hpSlider;
            var hpSliderOffset = GameRoleCollectionR.ROLE_ATTRIBUTE_SLIDER_HP_OFFSET;
            hpSlider.SetSlider(uiHPSlider, hpSliderOffset);
            Vector2 hpSliderSize = GameRoleCollectionR.ROLE_ATTRIBUTE_SLIDER_HP_SIZE;
            hpSlider.SetSize(hpSliderSize);
            hpSlider.SetSlitLineMat(this._roleContext.factory.CreateSplitLineMaterialInst());

            // 判断是否有魔法攻击技能, 有则加载魔法条
            this._roleContext.factory.template.TryGet(role.idCom.typeId, out var model);
            var hasMPSkill = model.skillSOs.Find((skill) => skill.skillType == GameSkillType.MagicAttack) != null;
            if (hasMPSkill)
            {
                var mpSlider = attributeBarCom.mpSlider;
                var uiMPSlider = mpSlider.slider ?? this._roleContext.factory.LoadMPSlider();
                this._context.uiApi.layerApi.AddToUIRoot(uiMPSlider.transform, UILayerType.Scene);
                var mpSliderOffset = GameRoleCollectionR.ROLE_ATTRIBUTE_SLIDER_MP_OFFSET;
                mpSlider.SetSlider(uiMPSlider, mpSliderOffset);
                var mpSliderSize = GameRoleCollectionR.ROLE_ATTRIBUTE_SLIDER_MP_SIZE;
                mpSlider.SetSize(mpSliderSize);
            }

            // 护盾条不分敌我, 加载或使用旧的护盾条
            var shieldSlider = attributeBarCom.shieldSlider;
            var uiShieldSlider = shieldSlider.slider ?? this._roleContext.factory.LoadShieldSlider();
            this._context.uiApi.layerApi.AddToUIRoot(uiShieldSlider.transform, UILayerType.Scene);
            var shieldSliderOffset = GameRoleCollectionR.ROLE_ATTRIBUTE_SLIDER_SHIELD_OFFSET;
            shieldSlider.SetSlider(uiShieldSlider, shieldSliderOffset);
            var shieldSliderSize = GameRoleCollectionR.ROLE_ATTRIBUTE_SLIDER_SHIELD_SIZE;
            shieldSlider.SetSize(shieldSliderSize);
            shieldSlider.SetSlitLineMat(this._roleContext.factory.CreateSplitLineMaterialInst());

            // 初始化默认材质
            role.bodyCom.renderers?.Foreach((renderer) =>
            {
                renderer.material = this._roleContext.factory.GetDefaultMaterial();
            });
            return role;
        }

        public GameRoleEntityR CreateRole(in GameIdArgs idArgs, in GameTransformArgs transArgs)
        {
            var role = this._LoadRole(idArgs, transArgs);
            this._roleContext.repo.TryAdd(role);
            return role;
        }

        private void _OnTransformRole(object args)
        {
            if (this._context.fieldContext.curField == null)
            {
                this._context.DelayRC(GameRoleRCCollection.RC_GAME_ROLE_TRANSFORM, args);
                return;
            }
            var rcArgs = (GameRoleRCArgs_CharacterTransform)args;
            this._TransformRole(rcArgs.idArgs);
        }

        private void _TransformRole(in GameIdArgs idArgs)
        {
            if (!this._roleContext.repo.TryFindByEntityId(idArgs.entityId, out var role))
            {
                GameLogger.LogError("变身失败, 受变身角色不存在: " + idArgs.entityId);
                return;
            }

            var transToRoleId = idArgs.typeId;
            if (!this._roleContext.factory.template.TryGet(transToRoleId, out var model))
            {
                GameLogger.LogError("变身失败, 模板不存在: " + transToRoleId);
            }

            var newRole = this._LoadRole(idArgs, role.transformCom.ToArgs());

            // 将新角色替换就旧的角色, 若未处于变身状态, 将oldRole记录在变身角色仓库
            var oldRole = this._roleContext.repo.Replace(newRole);
            if (!this._roleContext.transformRepo.TryFindByEntityId(role.idCom.entityId, out var _))
            {
                this._roleContext.transformRepo.TryAdd(oldRole);
            }

            // 转移buff
            this._context.domainApi.buffApi.TranserBuffCom(oldRole.buffCom, newRole);

            // 隐藏旧角色
            oldRole.SetInvalid();
            oldRole.setActive(false);
        }

        /// <summary>
        /// 世界坐标转屏幕坐标
        /// <para> v: 世界坐标 </para>
        /// </summary>
        public Vector3 WorldToScreenPoint(in Vector3 v)
        {
            var worldCam = this._context.cameraEntity.camera;
            var pos = RectTransformUtility.WorldToScreenPoint(worldCam, v);
            pos.x -= Screen.width / 2;
            pos.y -= Screen.height / 2;
            return pos;
        }

        public void Tick(float dt)
        {
            this.inputDomain.Tick();
            var repo = this._roleContext.repo;
            var entitys = new GameRoleEntityR[repo.Count];
            repo.ForeachEntities((entity, index) =>
            {
                entity.Tick(dt);
                this.fsmDomain.Tick(entity, dt);
                entitys[index] = entity;
            });
            entitys.Sort((a, b) =>
            {
                var posA = a.transform.position;
                var posB = b.transform.position;
                return posA.y.CompareTo(posB.y);
            });
            for (int i = 0; i < entitys.Length; i++)
            {
                var entity = entitys[i];
                var attributeBarCom = entity.attributeBarCom;
                attributeBarCom.hpSlider.SetSiblingIndex(i);
                attributeBarCom.mpSlider.SetSiblingIndex(i);
            }
        }

        private void _PlayAnim(GameRoleEntityR role, string animName, int layer)
        {
            var factory = this._roleContext.factory;
            var animCom = role.animCom;
            if (animCom.TryGetClip(animName, out var clip))
            {
                animCom.Play(clip, layer);
                return;
            }

            clip = factory.LoadRoleAnimationClip(role.model.typeId, animName);
            animCom.Play(clip, layer);
        }

        public void PlayAnim(GameRoleEntityR entity, string animName)
        {
            var isMultyAnimationLayer = entity.model.isMultyAnimationLayer;
            if (!isMultyAnimationLayer)
            {
                this._PlayAnim(entity, animName, 0);
                return;
            }
            this._PlayAnim(entity, animName, 0);
            string[] keys = GameRoleCollectionR.ROLE_MULTY_LAYER_ANIM_KEYS;
            var isKey = keys.Find((key) => animName.Contains(key)) != null;
            if (isKey)
            {
                var upperAnimName = animName + "_l";
                this._PlayAnim(entity, upperAnimName, 1);
            }
        }

        public GameRoleTemplateR GetRoleTemplate()
        {
            return this._context.roleContext.factory.template;
        }

        public GameRoleEntityR FindByEntityId(int entityId)
        {
            return this._roleContext.repo.FindByEntityId(entityId);
        }

    }
}
