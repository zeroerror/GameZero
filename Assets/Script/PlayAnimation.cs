using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public string animationName = "YourAnimationName"; // 动画名称

    private Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>(); // 获取Animation组件
        if (anim != null)
        {
            var c = anim.GetClip(animationName); // 获取指定的动画
            if (c != null)
            {
                anim.Play(animationName); // 播放指定的动画
            }
            else
            {
                Debug.LogError("没有找到指定的动画！");
            }
        }
        else
        {
            Debug.LogError("没有找到Animation组件！");
        }
    }
}
