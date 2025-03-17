using Plugins.SerializableGuid;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Plugins.Editor
{
    
    public class SGuidPropertyDrawer : OdinValueDrawer<SGuid>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            SGuid value = this.ValueEntry.SmartValue;
            var guidString = 
                ((Guid) value).ToString();
            
            
            GUIHelper.PushLabelWidth(20);
            rect = rect.AddXMin(-20);
            var buttonRect = rect.TakeFromLeft(20).Padding(1.5f);
            var labelRect = rect;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.LabelField(labelRect, guidString);
            EditorGUI.EndDisabledGroup();
            if(GUI.Button(buttonRect, ""))
            {
                value.Regenerate();
                ValueEntry.SmartValue = value;
            }
            SdfIcons.DrawIcon(buttonRect, SdfIconType.Hammer);
            
            GUIHelper.PopLabelWidth();

        }
    }
}