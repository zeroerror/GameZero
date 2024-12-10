using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public interface GameFieldDomainApiR
    {
        public void AddToLayer(GameObject go, GameFieldLayerType layerType, int orderOfset = 0);
    }
}