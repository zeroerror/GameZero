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
            this._roleContext.repo.ForeachAllEntities((entity) =>
            {
                entity.Destroy();
            });
        }

        private void _BindEvents()
        {
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_TRANSFORM, this._OnRoleTransform);
            this.fsmDomain.BindEvents();
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
            this._context.UnbindRC(GameRoleRCCollection.RC_GAME_ROLE_TRANSFORM, this._OnRoleTransform);
            this.fsmDomain.UnbindEvents();
        }

        private void _OnRoleCreate(object args)
        {
            if (this._context.fieldContext.curField == null)
            {
                this._context.DelayRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, args);
                return;
            }
            var evArgs = (GameRoleRCArgs_Create)args;
            this._Create(evArgs.idArgs, evArgs.transArgs, evArgs.isUser);
        }

        private GameRoleEntityR _Create(in GameIdArgs idArgs, in GameTransformArgs transArgs, bool isUser = false)
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
            this._roleContext.repo.TryAdd(role);
            role.setActive(true);
            if (isUser)
            {
                this._roleContext.userRole = role;
                // this._context.cameraEntity.followCom.Set(role.bodyCom.root, Vector2.zero);
            }

            var attributeBarCom = role.attributeBarCom;
            attributeBarCom.WorldToScreenPoint = this.WorldToScreenPoint;

            var isEnemy = role.idCom.campId != this._roleContext.userRole.idCom.campId;
            var hpSlider = this._roleContext.factory.LoadHPSlider(isEnemy);
            this._context.uiApi.layerApi.AddToUIRoot(hpSlider.transform, GameUILayerType.Scene);
            attributeBarCom.hpSlider.SetSlider(hpSlider, new Vector2(0, 150));
            attributeBarCom.hpSlider.SetSize(new Vector2(150, 20));

            this._roleContext.factory.template.TryGet(role.idCom.typeId, out var model);
            var hasMPSkill = model.skillSOs.Find((skill) => skill.skillType == GameSkillType.MagicAttack) != null;
            if (hasMPSkill)
            {
                var mpSlider = this._roleContext.factory.LoadMPSlider();
                this._context.uiApi.layerApi.AddToUIRoot(mpSlider.transform, GameUILayerType.Scene);
                attributeBarCom.mpSlider.SetSlider(mpSlider, new Vector2(0, 135));
                attributeBarCom.mpSlider.SetSize(new Vector2(150, 15));
            }

            return role;
        }

        private void _OnRoleTransform(object args)
        {
            if (this._context.fieldContext.curField == null)
            {
                this._context.DelayRC(GameRoleRCCollection.RC_GAME_ROLE_TRANSFORM, args);
                return;
            }
            var evArgs = (GameRoleRCArgs_CharacterTransform)args;
            this._CharacterTransform(evArgs.idArgs, evArgs.transRoleId);
        }

        private void _CharacterTransform(in GameIdArgs idArgs, int transRoleId)
        {
            if (!this._roleContext.repo.TryFetch(idArgs.typeId, out var role))
            {
                GameLogger.LogError("GameRoleDomainR._CharacterTransform: roleId not found: " + idArgs.typeId);
                return;
            }

            if (!this._roleContext.factory.template.TryGet(transRoleId, out var model))
            {
                GameLogger.LogError("GameRoleDomainR._CharacterTransform: 变身模板不存在: " + transRoleId);
                return;
            }

            // 变身前默认结束变身
            role.roleTransformCom.EndTransform();
            // 变身
            var bodyCom = this._roleContext.factory.GetBodyCom(model);
            role.roleTransformCom.StartTransform(model, bodyCom);

            // 更新buff特效的挂载节点
            role.buffCom.ForeachAllBuffs((buff) =>
            {
                var vfxEntity = buff.vfxEntity;
                if (vfxEntity == null) return;
                var playArgs = vfxEntity.playArgs;
                if (!playArgs.attachNode) return;
                playArgs.attachNode = bodyCom.root;
                if (playArgs.isAttachParent)
                {
                    vfxEntity.SetParent(playArgs.attachNode.transform);
                }
            });
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
                attributeBarCom.hpSlider.rectTransform.SetSiblingIndex(i);
                attributeBarCom.mpSlider.rectTransform.SetSiblingIndex(i);
            }
        }

        private void _PlayAnim(GameRoleEntityR entity, string animName, int layer)
        {
            var factory = this._roleContext.factory;
            var animCom = entity.animCom;
            if (animCom.TryGetClip(animName, out var clip))
            {
                animCom.Play(clip, layer);
                return;
            }

            clip = factory.LoadAnimationClip(entity.idCom.typeId, animName);
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
    }
}
