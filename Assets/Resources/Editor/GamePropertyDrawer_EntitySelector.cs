using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;
namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameEntitySelectorEM))]
    public class GamePropertyDrawer_EntitySelector : GamePropertyDrawer
    {
        protected override void _OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var selectAnchorType_p = property.FindPropertyRelative("selectAnchorType");
            var selectAnchorType = selectAnchorType_p.DrawProperty_EnumPopup<GameEntitySelectAnchorType>("选择锚点类型");
            if (selectAnchorType == GameEntitySelectAnchorType.None) EditorGUILayout.HelpBox("请选择一个选择锚点类型", MessageType.Warning);

            var onlySelectDead_p = property.FindPropertyRelative("onlySelectDead");
            onlySelectDead_p.DrawProperty("是否仅选择死亡单位");

            var selColliderType_p = property.FindPropertyRelative("selColliderType");
            var selColliderType = selColliderType_p.DrawProperty_EnumPopup<GameColliderType>("碰撞体类型");

            var go = Selection.activeGameObject;
            EditorGUILayout.HelpBox("可在场景中选择碰撞器来读取参数", MessageType.Info);
            var collider = go?.GetComponent<Collider>();
            if (!collider) go = null;

            var color = GUI.color;
            GUI.color = Color.green;
            if (go) EditorGUILayout.ObjectField("当前选中", go, typeof(GameObject), true);
            GUI.color = color;

            switch (selColliderType)
            {
                case GameColliderType.Box:
                    var boxColliderModel_p = property.FindPropertyRelative("boxColliderModel");
                    boxColliderModel_p.DrawProperty("盒子碰撞体");
                    if (go) this._ModifyBoxModel(boxColliderModel_p, go.transform);
                    break;
                case GameColliderType.Circle:
                    var circleColliderModel_p = property.FindPropertyRelative("circleColliderModel");
                    circleColliderModel_p.DrawProperty("圆形碰撞体");
                    if (go) this._ModifyCircleModel(circleColliderModel_p, go.transform);
                    break;
                case GameColliderType.Fan:
                    var fanColliderModel_p = property.FindPropertyRelative("fanColliderModel");
                    fanColliderModel_p.DrawProperty("扇形碰撞体");
                    if (go) this._ModifyFanModel(fanColliderModel_p, go.transform);
                    break;
            }

            if (selColliderType != GameColliderType.None)
            {
                var campType_p = property.FindPropertyRelative("campType");
                campType_p.DrawProperty_EnumPopup<GameCampType>("阵营类型");
                var entityType_p = property.FindPropertyRelative("entityType");
                entityType_p.DrawProperty_EnumPopup<GameEntityType>("实体类型");
            }
        }

        private void _ModifyBoxModel(SerializedProperty prop, Transform refTrans)
        {
            if (prop == null) return;
            var offset_p = prop.FindPropertyRelative("offset");
            var angle_p = prop.FindPropertyRelative("angle");
            var width_p = prop.FindPropertyRelative("width");
            var height_p = prop.FindPropertyRelative("height");
            offset_p.vector2Value = refTrans.position;
            angle_p.floatValue = refTrans.eulerAngles.z;
            width_p.floatValue = refTrans.lossyScale.x;
            height_p.floatValue = refTrans.lossyScale.y;
        }

        private void _ModifyCircleModel(SerializedProperty prop, Transform refTrans)
        {
            if (prop == null) return;
            var offset_p = prop.FindPropertyRelative("offset");
            var angle_p = prop.FindPropertyRelative("angle");
            var radius_p = prop.FindPropertyRelative("radius");
            offset_p.vector2Value = refTrans.position;
            angle_p.floatValue = refTrans.eulerAngles.z;
            radius_p.floatValue = refTrans.lossyScale.x;
        }

        private void _ModifyFanModel(SerializedProperty prop, Transform refTrans)
        {
            if (prop == null) return;
            var offset_p = prop.FindPropertyRelative("offset");
            var angle_p = prop.FindPropertyRelative("angle");
            var fanAngle_p = prop.FindPropertyRelative("fanAngle");
            var radius_p = prop.FindPropertyRelative("radius");
            offset_p.vector2Value = refTrans.position;
            angle_p.floatValue = refTrans.eulerAngles.z;
            radius_p.floatValue = refTrans.lossyScale.x;
        }
    }
}