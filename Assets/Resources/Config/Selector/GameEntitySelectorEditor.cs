using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEditor;
using UnityEngine;
namespace GamePlay.Config
{
    [CustomPropertyDrawer(typeof(GameEntitySelectorEM))]
    public class GameEntitySelectorEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var selectAnchorType_p = property.FindPropertyRelative("selectAnchorType");
            selectAnchorType_p.enumValueIndex = (int)(GameEntitySelectAnchorType)EditorGUI.EnumPopup(position, "选择锚点类型", (GameEntitySelectAnchorType)selectAnchorType_p.enumValueIndex);
            var campType_p = property.FindPropertyRelative("campType");
            campType_p.enumValueIndex = (int)(GameCampType)EditorGUI.EnumPopup(position, "阵营类型", (GameCampType)campType_p.enumValueIndex);
            var entityType_p = property.FindPropertyRelative("entityType");
            entityType_p.enumValueIndex = (int)(GameEntityType)EditorGUI.EnumPopup(position, "实体类型", (GameEntityType)entityType_p.enumValueIndex);

            var selColliderType_p = property.FindPropertyRelative("selColliderType");
            selColliderType_p.enumValueIndex = (int)(GameColliderType)EditorGUI.EnumPopup(position, "碰撞体类型", (GameColliderType)selColliderType_p.enumValueIndex);
            var boxColliderModel_p = property.FindPropertyRelative("boxColliderModel");
            boxColliderModel_p.objectReferenceValue = EditorGUILayout.ObjectField("盒子碰撞体", boxColliderModel_p.objectReferenceValue, typeof(GameBoxColliderModel), true);
            var circleColliderModel_p = property.FindPropertyRelative("circleColliderModel");
            circleColliderModel_p.objectReferenceValue = EditorGUILayout.ObjectField("圆形碰撞体", circleColliderModel_p.objectReferenceValue, typeof(GameCircleColliderModel), true);
            var fanColliderModel_p = property.FindPropertyRelative("fanColliderModel");
            fanColliderModel_p.objectReferenceValue = EditorGUILayout.ObjectField("扇形碰撞体", fanColliderModel_p.objectReferenceValue, typeof(GameFanColliderModel), true);

            var selectAnchorType = (GameEntitySelectAnchorType)selectAnchorType_p.enumValueIndex;
            if (selectAnchorType == GameEntitySelectAnchorType.None) EditorGUILayout.HelpBox("请选择一个选择锚点类型", MessageType.Warning);
            else
            {
                var go = Selection.activeGameObject;
                EditorGUILayout.HelpBox("可在场景中选择碰撞器来读取参数", MessageType.Info);
                var collider = go?.GetComponent<Collider>();
                if (!collider) go = null;

                var color = GUI.color;
                GUI.color = Color.green;
                if (go) EditorGUILayout.ObjectField("当前选中", go, typeof(GameObject), true);
                GUI.color = color;

                if (!go)
                {
                    switch ((GameColliderType)selColliderType_p.enumValueIndex)
                    {
                        case GameColliderType.Box:
                            this._ShowBoxModel(boxColliderModel_p);
                            break;
                        case GameColliderType.Circle:
                            this._ShowCircleModel(circleColliderModel_p);
                            break;
                        case GameColliderType.Fan:
                            this._ShowFanModel(fanColliderModel_p);
                            break;
                        default:
                            EditorGUILayout.HelpBox("未知的碰撞体类型", MessageType.Warning);
                            break;
                    }
                }
                else
                {
                    var trans = go?.transform;
                    var selColliderType = (GameColliderType)selColliderType_p.enumValueIndex;
                    switch (selColliderType)
                    {
                        case GameColliderType.Box:

                            this._ShowBoxModel(boxColliderModel_p, trans);
                            break;
                        case GameColliderType.Circle:
                            this._ShowCircleModel(circleColliderModel_p, trans);
                            break;
                        case GameColliderType.Fan:
                            this._ShowFanModel(fanColliderModel_p, trans);
                            break;
                        default:
                            EditorGUILayout.HelpBox("未处理的碰撞体类型", MessageType.Warning);
                            break;
                    }
                }
            }
        }

        private void _ShowBoxModel(SerializedProperty prop, Transform refTrans = null)
        {
            if (prop == null) return;
            var offset_p = prop.FindPropertyRelative("offset");
            var angle_p = prop.FindPropertyRelative("angle");
            var width_p = prop.FindPropertyRelative("width");
            var height_p = prop.FindPropertyRelative("height");
            if (refTrans)
            {
                offset_p.vector2Value = refTrans.position;
                angle_p.floatValue = refTrans.eulerAngles.z;
                width_p.floatValue = refTrans.lossyScale.x;
                height_p.floatValue = refTrans.lossyScale.y;
            }
            EditorGUILayout.Vector2Field("偏移", offset_p.vector2Value);
            EditorGUILayout.FloatField("角度", angle_p.floatValue);
            EditorGUILayout.FloatField("宽度", width_p.floatValue);
            EditorGUILayout.FloatField("高度", height_p.floatValue);
        }

        private void _ShowCircleModel(SerializedProperty prop, Transform refTrans = null)
        {
            if (prop == null) return;
            var offset_p = prop.FindPropertyRelative("offset");
            var angle_p = prop.FindPropertyRelative("angle");
            var radius_p = prop.FindPropertyRelative("radius");
            if (refTrans)
            {
                offset_p.vector2Value = refTrans.position;
                angle_p.floatValue = refTrans.eulerAngles.z;
                radius_p.floatValue = refTrans.lossyScale.x;
            }
            EditorGUILayout.Vector2Field("偏移", offset_p.vector2Value);
            EditorGUILayout.FloatField("角度", angle_p.floatValue);
            EditorGUILayout.FloatField("半径", radius_p.floatValue);
        }

        private void _ShowFanModel(SerializedProperty prop, Transform refTrans = null)
        {
            if (prop == null) return;
            var offset_p = prop.FindPropertyRelative("offset");
            var angle_p = prop.FindPropertyRelative("angle");
            var fanAngle_p = prop.FindPropertyRelative("fanAngle");
            var radius_p = prop.FindPropertyRelative("radius");
            if (refTrans)
            {
                offset_p.vector2Value = refTrans.position;
                angle_p.floatValue = refTrans.eulerAngles.z;
                radius_p.floatValue = refTrans.lossyScale.x;
            }
            fanAngle_p.floatValue = EditorGUILayout.FloatField("扇形角度", fanAngle_p.floatValue);// 由于unity没有提供扇形的编辑器，所以这里只能手动输入
            EditorGUILayout.Vector2Field("偏移", offset_p.vector2Value);
            EditorGUILayout.FloatField("角度", angle_p.floatValue);
            EditorGUILayout.FloatField("半径", radius_p.floatValue);
        }
    }
}