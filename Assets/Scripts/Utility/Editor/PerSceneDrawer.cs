using UnityEditor;
using UnityEngine;
using Utility.PerScene;

namespace Utility.Editor
{

    [CustomPropertyDrawer(typeof(PerScene<>))]
    public class PerSceneDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("_instance"), label);
            EditorGUI.EndDisabledGroup();
        }
    }
}