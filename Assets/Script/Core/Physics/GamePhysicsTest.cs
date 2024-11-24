using UnityEngine;
using GamePlay.Core;
using GamePlay.Bussiness.Logic;
using System.Collections.Generic;

public class GamePhysicsTest : MonoBehaviour
{
    #region Main
    private List<GameColliderBase> colliderList = new List<GameColliderBase>();
    private List<Color> colorList = new List<Color>();
    private int _ctrlIndex;
    private List<System.Action> testList = new List<System.Action>();
    // test name list
    private List<string> testNameList = new List<string>();
    private int _testIndex = 0;

    public void Start()
    {
        this.testNameList.Add("Box vs Box");
        this.testList.Add(() =>
        {
            this.CreateBox(new GameTransformArgs()
            {
                position = new Vector2(0, 0),
                angle = 0,
                scale = 1
            }, Color.white);
            this.CreateBox(new GameTransformArgs()
            {
                position = new Vector2(0.8f, 0),
                angle = 0,
                scale = 1
            }, Color.blue);
        });
        this.testNameList.Add("Fan vs Fan");
        this.testList.Add(() =>
              {
                  this.CreateFan(new GameTransformArgs()
                  {
                      position = new Vector2(0, 0),
                      angle = 0,
                      scale = 1
                  }, 1, 135, Color.white);
                  this.CreateFan(new GameTransformArgs()
                  {
                      position = new Vector2(0.8f, 0.8f),
                      angle = 0,
                      scale = 1
                  }, 1, 135, Color.blue);
              });
        this.testNameList.Add("Box vs Circle");
        this.testList.Add(() =>
              {
                  this.CreateBox(new GameTransformArgs()
                  {
                      position = new Vector2(0, 0),
                      angle = 0,
                      scale = 1
                  }, Color.white);
                  this.CreateCircle(new GameTransformArgs()
                  {
                      position = new Vector2(0.7f, 0.9f),
                      angle = 0,
                      scale = 1
                  }, 1, Color.blue);
              });
        this.testNameList.Add("Box vs Fan");
        this.testList.Add(() =>
        {
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
        });
        this.testNameList.Add("Fan vs Point");
        this.testList.Add(() =>
        {
            this.CreateFan(new GameTransformArgs()
            {
                position = new Vector2(0, 0),
                angle = 0,
                scale = 1
            }, 1, 135, Color.white);
        });

        this.testList[this._testIndex]();
    }

    public void Update()
    {
        // ------------------------------- 碰撞绘制 -------------------------------
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
                // // ttttttttt
                // var p = this.colliderList[i - 1].worldCenterPos;
                // var c1 = this.colliderList[i];
                // var mtv = c1.GetResolvingMTV(p, false);
                // // 绘制mtv
                // var s = p;
                // var e = s + mtv;
                // Debug.DrawLine(s, e, color);
                // // 绘制箭头
                // var arrow = mtv.normalized * 0.1f;
                // var arrow1 = e + GameVectorUtil.RotateOnAxisZ(arrow, 135);
                // var arrow2 = e + GameVectorUtil.RotateOnAxisZ(arrow, -135);
                // Debug.DrawLine(e, arrow1, color);
                // Debug.DrawLine(e, arrow2, color);
                // // 绘制扇形normal
                // if (c1 is GameFanCollider)
                // {
                //     var c = c1 as GameFanCollider;
                //     var normal1 = c.normal1;
                //     var normalEnd1 = c.worldCenterPos + normal1 * 5f;
                //     Debug.DrawLine(c.worldCenterPos, normalEnd1, color);
                //     var normal2 = c.normal2;
                //     var normalEnd2 = c.worldCenterPos + normal2 * 5f;
                //     Debug.DrawLine(c.worldCenterPos, normalEnd2, color);
                // }
            }
        }
        // ------------------------------- 控制 -------------------------------
        if (Input.GetKeyDown(KeyCode.Space)) this._ctrlIndex = (this._ctrlIndex + 1) % this.colliderList.Count;
        var canCtrl = this.colliderList.Count > 0 && this._ctrlIndex >= 0 && this._ctrlIndex < this.colliderList.Count;
        if (canCtrl)
        {
            var speed = 1f;
            var offset = speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.W)) this.colliderList[this._ctrlIndex].binder.transformCom.position += new Vector2(0, offset);
            if (Input.GetKey(KeyCode.S)) this.colliderList[this._ctrlIndex].binder.transformCom.position += new Vector2(0, -offset);
            if (Input.GetKey(KeyCode.A)) this.colliderList[this._ctrlIndex].binder.transformCom.position += new Vector2(-offset, 0);
            if (Input.GetKey(KeyCode.D)) this.colliderList[this._ctrlIndex].binder.transformCom.position += new Vector2(offset, 0);
        }
        var testIndex = -99;
        if (Input.GetKeyDown(KeyCode.Q)) testIndex = this._testIndex - 1;
        if (Input.GetKeyDown(KeyCode.E)) testIndex = this._testIndex + 1;
        if (testIndex != -99)
        {
            testIndex = Mathf.Clamp(testIndex, 0, this.testList.Count - 1);
            this._testIndex = testIndex;
            this.colliderList.Clear();
            this.colorList.Clear();
            this.testList[this._testIndex]();
        }
    }

    public void OnGUI()
    {
        // ------------------------------- 测试列表绘制 ----------------------------
        for (int i = 0; i < this.testNameList.Count; i++)
        {
            var name = this.testNameList[i];
            var color = i == this._testIndex ? Color.red : Color.white;
            var rect = new Rect(10, 10 + i * 30, 200, 30);
            GUI.color = color;
            GUI.Label(rect, name);
        }
    }
    private void Draw(GameColliderBase c, Color color)
    {
        if (c is GameCircleCollider circle)
        {
            circle.Draw(color);
            return;
        }
        if (c is GameBoxCollider box)
        {
            box.Draw(color);
            return;
        }
        if (c is GameFanCollider fan)
        {
            fan.Draw(color);
            return;
        }
    }
    #endregion

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
}