using GamePlay.Bussiness.Logic;
using UnityEngine;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Renderer
{
    public class GameRoleEntityR : GameEntityBase
    {
        public GameRoleFSMComR fsmCom { get; private set; }
        public GameObject go { get; private set; }
        public GameObject foot { get; private set; }
        public GameObject body { get; private set; }
        public Animation animation { get; private set; }
        public SpriteRenderer spriteRenderer { get; private set; }

        public GameRoleEntityR(GameObject go) : base(0, GameEntityType.Role)
        {
            this.go = go;
            go.name = "role_" + this.idCom.entityId;
            this.foot = go.transform.Find("foot").gameObject;
            this.body = go.transform.Find("body").gameObject;
            this.animation = this.body.AddComponent<Animation>();
            this.spriteRenderer = this.body.AddComponent<SpriteRenderer>();
            this.fsmCom = new GameRoleFSMComR(this);
        }

        public override void Tick(float dt)
        {
            this.fsmCom.Tick(dt);
        }

        public override void Reset(float dt) { }

        public GameVec2 position { get { return this.go.transform.position; } set { this.go.transform.position = value; } }
    }
}