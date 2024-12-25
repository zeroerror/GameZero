using GamePlay.Bussiness.Logic;
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
            this._BindEvent();
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

        private void _BindEvent()
        {
            this._context.BindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
            this.fsmDomain.BindEvents();
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameRoleRCCollection.RC_GAME_ROLE_CREATE, this._OnRoleCreate);
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

        public void Collect()
        {
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
                this._context.domainApi.fielApi.AddToLayer(role.root, GameFieldLayerType.Entity);
                var orderOffset = GameFieldLayerCollection.EnvironmentLayerZ - GameFieldLayerCollection.GroundLayerZ;
                this._context.domainApi.fielApi.AddToLayer(role.shadow, GameFieldLayerType.Ground, orderOffset);
            }
            role.idCom.SetByArgs(idArgs);
            role.root.name = $"role_{idArgs.typeId}_{role.idCom.entityId}";
            role.transformCom.SetByArgs(transArgs);
            role.SyncTrans();
            this._roleContext.repo.TryAdd(role);
            role.setActive(true);
            if (isUser)
            {
                this._roleContext.userRole = role;
                this._context.cameraEntity.followCom.Set(role.root, Vector2.zero);
            }

            var attributeBarCom = role.attributeBarCom;
            attributeBarCom.WorldToScreenPoint = this.WorldToScreenPoint;

            var isEnemy = role.idCom.campId != this._roleContext.userRole.idCom.campId;
            var hpSlider = this._roleContext.factory.LoadHPSlider(isEnemy);
            this._context.uiContext.AddToUIRoot(hpSlider.transform);
            attributeBarCom.hpSlider.SetSlider(hpSlider, new Vector2(0, 150));

            this._roleContext.factory.template.TryGet(role.idCom.typeId, out var model);
            var hasMPSkill = model.skills.Find((skill) => skill.skillType == GameSkillType.MagicAttack) != null;
            if (hasMPSkill)
            {
                var mpSlider = this._roleContext.factory.LoadMPSlider();
                this._context.uiContext.AddToUIRoot(mpSlider.transform);
                attributeBarCom.mpSlider.SetSlider(mpSlider, new Vector2(0, 90f));
            }

            attributeBarCom.hpSlider.SetSize(new Vector2(100, 15));
            attributeBarCom.mpSlider.SetSize(new Vector2(100, 15));

            return role;
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
            repo.ForeachEntities((entity) =>
            {
                entity.Tick(dt);
                this.fsmDomain.Tick(entity, dt);
            });
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
            string[] keys = { "idle", "move", "dead" };
            var has = keys.Find((key) => animName.Contains(key));
            if (!string.IsNullOrEmpty(has))
            {
                var upperAnimName = animName + "_l";
                this._PlayAnim(entity, upperAnimName, 2);
            }
        }

        public void PlayAnim(GameRoleEntityR entity, AnimationClip clip)
        {
            this._StopAnim(entity);
            this._PlayAnim(entity, clip.name, 0);
        }
    }
}
