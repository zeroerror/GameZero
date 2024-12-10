using GamePlay.Bussiness.Logic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Renderer
{
    public class GameDrawDomainR : GameDrawDomainApiR
    {
        GameContextR _context;
        GameDrawContextR _drawContext;

        public GameDrawDomainR()
        {
            this._drawContext = new GameDrawContextR();
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
            this._context.BindRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, this._OnDrawColliderModel);
            this._context.BindRC(GameDrawRCCollection.RC_DRAW_COLLIDER, this._OnDrawCollider);
        }

        private void _UnbindEvents()
        {
            this._context.UnbindRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, this._OnDrawColliderModel);
            this._context.UnbindRC(GameDrawRCCollection.RC_DRAW_COLLIDER, this._OnDrawCollider);
        }

        private void _OnDrawColliderModel(object args)
        {
            var evArgs = (GameRCArgs_DrawColliderModel)args;
            var colliderModel = evArgs.colliderModel;
            var transformArgs = evArgs.transformArgs;
            var color = evArgs.color;
            this._drawContext.tasks.Add(new GameDrawTask()
            {
                drawAction = () =>
                {
                    colliderModel.Draw(transformArgs, color);
                },
                maintainTime = evArgs.maintainTime,
            });
        }

        private void _OnDrawCollider(object args)
        {
            var evArgs = (GameRCArgs_DrawCollider)args;
            var collider = evArgs.collider;
            var color = evArgs.color;
            this._drawContext.tasks.Add(new GameDrawTask()
            {
                drawAction = () =>
                {
                    collider.Draw(color);
                },
                maintainTime = evArgs.maintainTime,
            });
        }

        public void Tick(float dt)
        {
            var tasks = this._drawContext.tasks;
            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                task.drawAction();
                task.elapsedTime += dt;
                if (task.isOver())
                {
                    tasks.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}