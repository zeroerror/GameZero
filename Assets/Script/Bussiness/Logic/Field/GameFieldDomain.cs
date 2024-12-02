using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameFieldDomain : GameFieldDomainApi
    {
        GameContext _context;
        GameFieldContext _fieldContext => this._context.fieldContext;

        public GameFieldDomain()
        {
        }

        public void Inject(GameContext context)
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
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
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

            // 提交RC
            this._context.SubmitRC(GameFieldRCCollection.RC_GAME_FIELD_CREATE, new GameFieldRCArgs_Create { typeId = fieldId });
        }
    }
}