using UnityEngine;
namespace GamePlay.Bussiness.Logic
{
    public class GameTimelineTest : MonoBehaviour
    {
        private GameTimelineComponent _timeline;

        private void Start()
        {
            _timeline = new GameTimelineComponent();
            _timeline.Play(1, 2, 0, () =>
            {
                Debug.Log($" Timeline Complete");
            });
            _timeline.AddEventByTime(0.5f, () =>
            {
                Debug.Log($" Timeline Event 0.5s");
            });
            _timeline.AddEventByTime(0.51f, () =>
            {
                Debug.Log($" Timeline Event 0.51s");
            });
            _timeline.AddEventByTime(0.52f, () =>
            {
                Debug.Log($" Timeline Event 0.52s");
            });
            _timeline.AddEventByTime(0.53f, () =>
            {
                Debug.Log($" Timeline Event 0.53s");
            });
            _timeline.AddEventByTime(0.54f, () =>
            {
                Debug.Log($" Timeline Event 0.54s");
            });
            _timeline.AddEventByTime(1f, () =>
            {
                Debug.Log($" Timeline Event 1s");
            });
        }

        private void Update()
        {
            _timeline.Tick(Time.deltaTime);
            Debug.Log($" Timeline Time: {_timeline.time}");
        }
    }
}