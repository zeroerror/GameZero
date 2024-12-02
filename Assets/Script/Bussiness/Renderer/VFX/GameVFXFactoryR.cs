using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameVFXFactoryR
    {
        public GameVFXFactoryR()
        {
        }

        public GameVFXEntityR Load()
        {
            var go = new GameObject();
            go.AddComponent<Animator>().runtimeAnimatorController = null;
            go.AddComponent<SpriteRenderer>();
            go.transform.localPosition = new Vector3(0, 0, 0);
            var entity = new GameVFXEntityR(go);
            return entity;
        }
    }
}