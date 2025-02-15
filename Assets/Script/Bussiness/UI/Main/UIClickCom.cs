using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.Bussiness.UI
{
    public class UIClickCom : MonoBehaviour, IPointerClickHandler
    {
        public Action onClick;
        public void OnPointerClick(PointerEventData eventData)
        {
            this.onClick?.Invoke();
        }
    }
}