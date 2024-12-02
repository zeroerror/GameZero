using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameFieldDomainR : GameFieldDomainApiR
    {
        GameContextR _context;
        GameFieldContextR _fieldContext => this._context.fieldContext;

        public GameFieldDomainR()
        {
        }

        public void Inject(GameContextR context)
        {
            this._context = context;
            this._BindEvent();
        }

        public void Dispose()
        {
            this._UnbindEvents();
        }

        private void _BindEvent()
        {
            this._context.BindRC(GameFieldRCCollection.RC_GAME_FIELD_CREATE, this._OnCreateField);
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
            var entityLayer = this._fieldContext.curField.entityLayer;
            for (var i = 0; i < entityLayer.transform.childCount; i++)
            {
                var child = entityLayer.transform.GetChild(i);
                var pos = child.position;
                pos.z = pos.y;
                child.position = pos;
            }
        }

        private void _OnCreateField(object args)
        {
            var evArgs = (GameFieldRCArgs_Create)args;
            var fieldId = evArgs.typeId;
            this.LoadField(fieldId);
        }

        public void LoadField(int fieldId)
        {
            var repo = this._fieldContext.repo;
            if (!repo.TryFetch(fieldId, out var field)) field = this._fieldContext.factory.Load(fieldId);
            if (field == null)
            {
                GameLogger.LogError("场景加载失败, 没有找到场景模板: " + fieldId);
                return;
            }
            this._fieldContext.curField = field;
        }

        public void AddToEntityLayer(GameObject go)
        {
            var curField = this._fieldContext.curField;
            var entityLayer = curField.entityLayer;
            go.transform.SetParent(entityLayer.transform);
        }
    }
}