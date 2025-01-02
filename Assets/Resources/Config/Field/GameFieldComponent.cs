using UnityEngine;

namespace GamePlay.Config
{
    public class GameFieldComponent : MonoBehaviour
    {
        [Header("场地模板")]
        public GameFieldSO fieldSO;

        public void Save()
        {
            fieldSO.UpdateData();
            Debug.Log("保存场地模板数据 ✔");
        }
    }
}