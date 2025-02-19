using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using GamePlay.Infrastructure;
using UnityEngine;

namespace GamePlay.Bussiness.Render
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
        }

        public void Destroy()
        {
        }

        public void BindEvents()
        {
            this._context.BindRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, this._OnDrawColliderModel);
            this._context.BindRC(GameDrawRCCollection.RC_DRAW_COLLIDER, this._OnDrawCollider);
            this._context.uiApi.directorApi.BindKeyAction(KeyCode.F, this._OnKeyAction_F);
        }

        public void UnbindEvents()
        {
            this._context.UnbindRC(GameDrawRCCollection.RC_DRAW_COLLIDER_MODEL, this._OnDrawColliderModel);
            this._context.UnbindRC(GameDrawRCCollection.RC_DRAW_COLLIDER, this._OnDrawCollider);
            this._context.uiApi.directorApi.UnbindKeyAction(KeyCode.F, this._OnKeyAction_F);
        }

        private void _OnDrawColliderModel(object args)
        {
            if (!this._drawEnable) return;
            var rcArgs = (GameRCArgs_DrawColliderModel)args;
            var colliderModel = rcArgs.colliderModel;
            var transformArgs = rcArgs.transformArgs;
            var color = rcArgs.color;
            this._drawContext.tasks.Add(new GameDrawTask()
            {
                drawAction = () =>
                {
                    colliderModel.Draw(transformArgs, color);
                },
                maintainTime = rcArgs.maintainTime,
            });
        }

        private void _OnDrawCollider(object args)
        {
            if (!this._drawEnable) return;
            var rcArgs = (GameRCArgs_DrawCollider)args;
            var collider = rcArgs.collider;
            var color = rcArgs.color;
            this._drawContext.tasks.Add(new GameDrawTask()
            {
                drawAction = () =>
                {
                    collider.Draw(color);
                },
                maintainTime = rcArgs.maintainTime,
            });
        }

        private void _OnKeyAction_F()
        {
            this._drawEnable = !this._drawEnable;
        }
        private bool _drawEnable = false;

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