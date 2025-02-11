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
            this._BindEvents();
        }

        public void Destroy()
        {
            this._UnbindEvents();
        }

        private void _BindEvents()
        {
        }

        private void _UnbindEvents()
        {
        }

        public void Tick(float dt)
        {
            var curField = this._fieldContext.curField;
            if (!curField) return;
            curField.model.monsterAreaModels?.Foreach((area, index) =>
            {
                if (curField.IsMonstersSpawned(index)) return;
                if (!this._IsTimesUp(area)) return;
                this._SpawnAreaMonsters(area, index);
            });
        }

        private bool _IsTimesUp(GameFieldMonsterAreaModel area)
        {
            var fsmCom = this._context.director.fsmCom;
            if (fsmCom.stateType != GameDirectorStateType.Fighting) return false;
            var stateTime = fsmCom.fightingState.stateTime;
            return stateTime >= area.spawnTime;
        }

        private void _SpawnAreaMonsters(GameFieldMonsterAreaModel area, int index)
        {
            var curField = this._fieldContext.curField;
            if (curField.IsMonstersSpawned(index)) return;
            curField.SetMonsterSpawned(index, true);

            var radius = area.radius;
            var areaPos = area.position + this._context.domainApi.directorApi.GetRoundAreaPosition();
            area.monsterSpawnModels?.Foreach((spawnModel, idx) =>
            {
                for (var i = 0; i < spawnModel.count; i++)
                {
                    var angle = GameMathF.Random(0, 360);
                    var x = radius * GameMathF.Cos(angle);
                    var y = radius * GameMathF.Sin(angle);
                    this._context.domainApi.roleApi.CreateEnemyRole(spawnModel.typeId, new GameTransformArgs
                    {
                        position = areaPos + new GameVec2(x, y),
                        scale = GameVec2.one,
                    });
                }
            });
        }

        public GameFieldEntity LoadField(int fieldId)
        {
            var repo = this._fieldContext.repo;
            if (!repo.TryFetch(fieldId, out var field)) field = this._fieldContext.factory.Load(fieldId);
            if (field == null)
            {
                GameLogger.LogError("场景加载失败, 没有找到场景模板: " + fieldId);
                return null;
            }
            this._fieldContext.curField = field;
            this._fieldContext.repo.TryAdd(field);
            // 提交RC
            this._context.SubmitRC(GameFieldRCCollection.RC_GAME_FIELD_CREATE, new GameFieldRCArgs_Create { typeId = fieldId });
            return field;
        }

        public void DestroyField(GameFieldEntity field)
        {
            if (!field) return;
            this._fieldContext.repo.TryRemove(field);
            this._context.domainApi.roleApi.RemoveAllRoles();
            this._context.domainApi.projectileApi.RemoveAllProjectiles();
            // 提交RC
            this._context.SubmitRC(GameFieldRCCollection.RC_GAME_FIELD_DESTROY, new GameFieldRCArgs_Destroy { typeId = field.model.typeId });
        }

        public bool HasSpawnedAllEnemyUnits()
        {
            var curField = this._fieldContext.curField;
            var monsterAreaModels = curField.model.monsterAreaModels;
            var hasAllSpawned = true;
            monsterAreaModels?.Foreach((area, index) =>
            {
                if (!curField.IsMonstersSpawned(index)) hasAllSpawned = false;
            });
            return hasAllSpawned;
        }
    }
}