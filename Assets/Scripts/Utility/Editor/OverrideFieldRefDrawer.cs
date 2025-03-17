using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Utility.Optional;

namespace Utility.Editor
{
    [CustomPropertyDrawer(typeof(OverrideFieldRef<>))]
    public class OverrideFieldRefDrawer<T> : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            if (label != null)
                rect = EditorGUI.PrefixLabel(rect, label);
            
            GUIHelper.PushLabelWidth(20);
            var hasOverride = property.FindPropertyRelative(nameof(OverrideFieldRef<T>.hasOverride));
            var value = property.FindPropertyRelative("_overrideValue");
            hasOverride.boolValue = EditorGUI.Toggle(rect.AlignLeft(20), value.boolValue, EditorStyles.toggle);
            EditorGUI.BeginDisabledGroup(!hasOverride.boolValue);
            if(hasOverride.boolValue)
                EditorGUI.PropertyField(rect.AlignRight(rect.width - 20), value, GUIContent.none);
            else
                EditorGUI.LabelField(rect.AlignRight(rect.width - 20), "Using default");
            EditorGUI.EndDisabledGroup();
            GUIHelper.PopLabelWidth();
        }
    }
}