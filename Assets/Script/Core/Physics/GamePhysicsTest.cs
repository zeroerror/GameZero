using UnityEngine;
using GamePlay.Core;
using GamePlay.Bussiness.Logic;
using GamePlay.Bussiness.Renderer;
using System.Collections.Generic;

public class GamePhysicsTest : MonoBehaviour
{
    private List<GameColliderBase> colliderList = new List<GameColliderBase>();
    private List<Color> colorList = new List<Color>();
    private int _ctrlIndex;

    public void Start()
    {
        // TEST 1 box和box
        // this.CreateBox(new GameTransformArgs()
        // {
        //     position = new Vector2(0, 0),
        //     angle = 0,
        //     scale = 1
        // }, Color.white);
        // this.CreateBox(new GameTransformArgs()
        // {
        //     position = new Vector2(0.8f, 0),
        //     angle = 0,
        //     scale = 1
        // }, Color.blue);

        // TEST 2 circle和circle
        // this.CreateFan(new GameTransformArgs()
        // {
        //     position = new Vector2(0, 0),
        //     angle = 0,
        //     scale = 1
        // }, 1, 135, Color.white);
        // this.CreateFan(new GameTransformArgs()
        // {
        //     position = new Vector2(0.8f, 0.8f),
        //     angle = 0,
        //     scale = 1
        // }, 1, 135, Color.blue);

        // TEST 3 box和circle
        // this.CreateBox(new GameTransformArgs()
        // {
        //     position = new Vector2(0, 0),
        //     angle = 0,
        //     scale = 1
        // }, Color.white);
        // this.CreateCircle(new GameTransformArgs()
        // {
        //     position = new Vector2(0.7f, 0.9f),
        //     angle = 0,
        //     scale = 1
        // }, 1, Color.blue);

        // TEST 4 box和fan WIP
        this.CreateBox(new GameTransformArgs()
        {
            position = new Vector2(0, 0),
            angle = 0,
            scale = 1
        }, Color.white);
        this.CreateFan(new GameTransformArgs()
        {
            position = new Vector2(0, 0),
            angle = 0,
            scale = 2
        }, 1, 135, Color.blue);
    }

    public void Update()
    {
        for (int i = 0; i < this.colliderList.Count; i++)
        {
            this.colliderList[i].UpdateTRS();
            var color = this.colorList[i];
            this.Draw(this.colliderList[i], color);
            if (i > 0)
            {
                var c1 = this.colliderList[i];
                var c2 = this.colliderList[i - 1];
                var mtv = GameResolvingUtil.GetResolvingMTV(c1, c2);
                // 绘制mtv
                var s = c1.worldCenterPos;
                var e = s + mtv;
                Debug.DrawLine(s, e, color);
                // 绘制箭头
                var arrow = mtv.normalized * 0.1f;
                var arrow1 = e + GameVectorUtil.RotateOnAxisZ(arrow, 135);
                var arrow2 = e + GameVectorUtil.RotateOnAxisZ(arrow, -135);
                Debug.DrawLine(e, arrow1, color);
                Debug.DrawLine(e, arrow2, color);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this._ctrlIndex = (this._ctrlIndex + 1) % this.colliderList.Count;
        }
        var speed = 1f;
        var offset = speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) this.colliderList[this._ctrlIndex].binder.transformCom.position += new Vector2(0, offset);
        if (Input.GetKey(KeyCode.S)) this.colliderList[this._ctrlIndex].binder.transformCom.position += new Vector2(0, -offset);
        if (Input.GetKey(KeyCode.A)) this.colliderList[this._ctrlIndex].binder.transformCom.position += new Vector2(-offset, 0);
        if (Input.GetKey(KeyCode.D)) this.colliderList[this._ctrlIndex].binder.transformCom.position += new Vector2(offset, 0);
    }

    private void Draw(GameColliderBase c, Color color)
    {
        if (c is GameCircleCollider)
        {
            this.DrawCircle(c as GameCircleCollider, color);
            return;
        }
        if (c is GameBoxCollider)
        {
            this.DrawBox(c as GameBoxCollider, color);
            return;
        }
        if (c is GameFanCollider)
        {
            this.DrawFan(c as GameFanCollider, color);
            return;
        }
    }

    #region Fan
    private void CreateFan(in GameTransformArgs args, float radius, float fanAngle, Color color)
    {
        var role = new GameRoleEntity(1);
        role.transformCom.SetByArgs(args);
        var model = new GameFanColliderModel(
            Vector2.zero, 0, radius, fanAngle
        );
        var id = 1;
        var fan = new GameFanCollider(role, model, id);
        this.colliderList.Add(fan);
        this.colorList.Add(color);
    }
    private void DrawFan(GameFanCollider fan, Color color)
    {
        var worldCenterPos = fan.worldCenterPos;
        var worldP1 = fan.worldP1;// 右上角
        var worldP2 = fan.worldP2;// 右下角
        Debug.DrawLine(worldCenterPos, worldP1, color);
        Debug.DrawLine(worldCenterPos, worldP2, color);
        var stepAngle = 0.1f;
        var lastPos = worldP2;
        for (float i = -fan.fanAngle / 2; i <= fan.fanAngle / 2; i += stepAngle)
        {
            var radian = i * Mathf.Deg2Rad;
            var x = worldCenterPos.x + fan.worldRadius * Mathf.Cos(radian);
            var y = worldCenterPos.y + fan.worldRadius * Mathf.Sin(radian);
            var pos = new Vector2(x, y);
            Debug.DrawLine(lastPos, pos, color);
            lastPos = pos;
        }
    }
    #endregion

    #region Circle
    private void CreateCircle(in GameTransformArgs args, float radius, Color color)
    {
        var role = new GameRoleEntity(1);
        role.transformCom.SetByArgs(args);
        var model = new GameCircleColliderModel(
            Vector2.zero, 0, radius
        );
        var id = 1;
        var circle = new GameCircleCollider(role, model, id);
        this.colliderList.Add(circle);
        this.colorList.Add(color);
    }
    private void DrawCircle(GameCircleCollider circle, Color color)
    {
        var worldCenterPos = circle.worldCenterPos;
        var worldRadius = circle.worldRadius;
        var stepAngle = 0.1f;
        var lastPos = worldCenterPos + new Vector2(worldRadius, 0);
        for (float i = 0; i < 360; i += stepAngle)
        {
            var radian = i * Mathf.Deg2Rad;
            var x = worldCenterPos.x + worldRadius * Mathf.Cos(radian);
            var y = worldCenterPos.y + worldRadius * Mathf.Sin(radian);
            var pos = new Vector2(x, y);
            Debug.DrawLine(lastPos, pos, color);
            lastPos = pos;
        }
    }
    #endregion

    #region Box
    private void CreateBox(in GameTransformArgs args, Color color)
    {
        var role = new GameRoleEntity(1);
        role.transformCom.SetByArgs(args);
        var model = new GameBoxColliderModel(
            Vector2.zero, 0, args.scale, args.scale
        );
        var id = 1;
        var box = new GameBoxCollider(role, model, id);
        this.colliderList.Add(box);
        this.colorList.Add(color);
    }
    private void DrawBox(GameBoxCollider box, Color color)
    {
        var worldP1 = box.worldP1;
        var worldP2 = box.worldP2;
        var worldP3 = box.worldP3;
        var worldP4 = box.worldP4;
        Debug.DrawLine(worldP1, worldP2, color);
        Debug.DrawLine(worldP2, worldP4, color);
        Debug.DrawLine(worldP4, worldP3, color);
        Debug.DrawLine(worldP3, worldP1, color);
    }
    #endregion
}