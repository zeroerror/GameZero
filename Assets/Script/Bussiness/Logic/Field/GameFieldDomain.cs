using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;

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

        public void Destroy()
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
            return;
            var curField = this._fieldContext.curField;
            curField.model.monsterAreaModels?.Foreach((area, index) =>
            {
                if (curField.IsMonstersSpawned(index)) return;
                if (!this._IsTimesUp(area)) return;
                this._SpawnAreaMonsters(area, index);
            });
        }

        private bool _IsTimesUp(GameFieldMonsterAreaModel area)
        {
            var gameTime = this._context.director.timeScaleCom.gameTime;
            return gameTime >= area.spawnTime;
        }

        private void _SpawnAreaMonsters(GameFieldMonsterAreaModel area, int index)
        {
            var curField = this._fieldContext.curField;
            if (curField.IsMonstersSpawned(index)) return;
            curField.SetMonsterSpawned(index, true);

            var radius = area.radius;
            area.monsterSpawnModels?.Foreach((spawnModel, idx) =>
            {
                for (var i = 0; i < spawnModel.count; i++)
                {
                    var angle = GameMathF.Random(0, 360);
                    var x = radius * GameMathF.Cos(angle);
                    var y = radius * GameMathF.Sin(angle);
                    this._context.domainApi.roleApi.CreateMonsterRole(spawnModel.typeId, new GameTransformArgs
                    {
                        position = area.position + new GameVec2(x, y),
                        scale = GameVec2.one,
                    });
                }
            });
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