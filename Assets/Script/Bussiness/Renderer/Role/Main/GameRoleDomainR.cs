using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.UI;
using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleDomainR : GameRoleDomainApiR
    {
        GameContextR _context;
        GameRoleContextR _roleContext => this._context.roleContext;

        public GameRoleInputDomainR inputDomain { get; private set; }
        public GameRoleFSMDomainR fsmDomain { get; private set; }

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
            this._BindEvents();
        }

        public void Destroy()
        {
            this._UnbindEvents();
            this.fsmDomain.Destroy();
            this._roleContext.repo.ForeachEntities_IncludePool((entity) =>
            {
                entity.Destroy();
            });
        }

        private void _BindEvents()
        {
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_TRANSFORM, this._OnTransformRole);
            this.fsmDomain.BindEvents();
        }

        private void _UnbindEvents()
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
            this._CreateRole(rcArgs.idArgs, rcArgs.transArgs, rcArgs.isUser);
        }

        private GameRoleEntityR _LoadRole(in GameIdArgs idArgs, in GameTransformArgs transArgs, bool isUser = false)
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
                this._context.domainApi.fielApi.AddToLayer(role.bodyCom.root, GameFieldLayerType.Entity);
                var orderOffset = GameFieldLayerCollection.EnvironmentLayerZ - GameFieldLayerCollection.GroundLayerZ;
                this._context.domainApi.fielApi.AddToLayer(role.bodyCom.shadow, GameFieldLayerType.Ground, orderOffset);
            }
            role.idCom.SetByArgs(idArgs);
            role.bodyCom.root.name = $"role_{idArgs.typeId}_{role.idCom.entityId}";
            role.transformCom.SetByArgs(transArgs);
            role.SyncTrans();
            role.setActive(true);
            if (isUser)
            {
                this._roleContext.userRole = role;
                // this._context.cameraEntity.followCom.Set(role.bodyCom.root, Vector2.zero);
            }

            var attributeBarCom = role.attributeBarCom;
            attributeBarCom.WorldToScreenPoint = this.WorldToScreenPoint;

            var isEnemy = role.idCom.campId != this._roleContext.userRole?.idCom.campId;
            var hpSlider = attributeBarCom.hpSlider.slider ?? this._roleContext.factory.LoadHPSlider(isEnemy);
            this._context.uiApi.layerApi.AddToUIRoot(hpSlider.transform, UILayerType.Scene);
            attributeBarCom.hpSlider.SetSlider(hpSlider, new Vector2(0, 150));
            attributeBarCom.hpSlider.SetSize(new Vector2(150, 20));

            this._roleContext.factory.template.TryGet(role.idCom.typeId, out var model);
            var hasMPSkill = model.skillSOs.Find((skill) => skill.skillType == GameSkillType.MagicAttack) != null;
            if (hasMPSkill)
            {
                var mpSlider = attributeBarCom.mpSlider.slider ?? this._roleContext.factory.LoadMPSlider();
                this._context.uiApi.layerApi.AddToUIRoot(mpSlider.transform, UILayerType.Scene);
                attributeBarCom.mpSlider.SetSlider(mpSlider, new Vector2(0, 135));
                attributeBarCom.mpSlider.SetSize(new Vector2(150, 15));
            }

            var shieldSlider = attributeBarCom.shieldSlider.slider ?? this._roleContext.factory.LoadShieldSlider();
            this._context.uiApi.layerApi.AddToUIRoot(shieldSlider.transform, UILayerType.Scene);
            attributeBarCom.shieldSlider.SetSlider(shieldSlider, new Vector2(0, 120));
            attributeBarCom.shieldSlider.SetSize(new Vector2(150, 15));

            return role;
        }

        private GameRoleEntityR _CreateRole(in GameIdArgs idArgs, in GameTransformArgs transArgs, bool isUser = false)
        {
            var role = this._LoadRole(idArgs, transArgs, isUser);
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

            var isUser = role == this._roleContext.userRole;
            var newRole = this._LoadRole(idArgs, role.transformCom.ToArgs(), isUser);

            // 将新角色替换就旧的角色, 若未处于变身状态, 将oldRole记录在变身角色仓库
            var oldRole = this._roleContext.repo.Replace(newRole);
            if (!this._roleContext.transfromRepo.TryFindByEntityId(role.idCom.entityId, out var _))
            {
                this._roleContext.transfromRepo.TryAdd(oldRole);
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

            clip = factory.LoadAnimationClip(role.model.typeId, animName);
            animCom.Play(clip, layer);
        }

        private void _StopAnim(GameRoleEntityR entity)
        {
            var animCom = entity.animCom;
            animCom.Stop();
        }

        public void PlayAnim(GameRoleEntityR entity, string animName)
        {
            this._StopAnim(entity);
            var isMultyAnimationLayer = entity.model.isMultyAnimationLayer;
            if (!isMultyAnimationLayer)
            {
                this._PlayAnim(entity, animName, 0);
                return;
            }
            this._PlayAnim(entity, animName, 1);
            string[] keys = { "idle", "move" };
            var isKey = keys.Find((key) => animName.Contains(key)) != null;
            if (isKey)
            {
                var upperAnimName = animName + "_l";
                this._PlayAnim(entity, upperAnimName, 2);
            }
        }

        public GameRoleTemplateR GetRoleTemplate()
        {
            return this._context.roleContext.factory.template;
        }
    }
}
