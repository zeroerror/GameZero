using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public interface GameFieldDomainApiR
    {
        public void AddToLayer(GameObject go, GameFieldLayerType layerType, int orderOfset = 0);
    }
}