using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Utility.Attributes;
using FetchableFieldAttribute = Utility.Attributes.FetchableFieldAttribute;
using Object = UnityEngine.Object;

namespace Utility.Editor
{
    [CustomPropertyDrawer(typeof(FetchableFieldAttribute))]
    public class FetchableFieldDrawer : PropertyDrawer
    {
        private const float K_MaxSize = 20; 
        
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var type = fieldInfo.FieldType;
            if (!typeof(Component).IsAssignableFrom(type))
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            if (property.serializedObject.targetObject is not Component target)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }
            // First get the attribute since it contains the range for the slider
            FetchableFieldAttribute fetchableField = attribute as FetchableFieldAttribute;
            if(fetchableField == null)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            if ((property.isArray && fetchableField.FetchStrategy < FetchStrategy.GetComponents) || (!property.isArray && fetchableField.FetchStrategy >= FetchStrategy.GetComponents))
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            if (target.gameObject.scene.isDirty && fetchableField.AutoFetch)
            {
                if (!property.isArray && property.objectReferenceValue == null)
                {
                    Fetch(property, fetchableField, target);
                }
                if(property.isArray && Enumerable.Range(0, property.arraySize).Any(i => property.GetArrayElementAtIndex(i).objectReferenceValue == null))
                {
                    Fetch(property, fetchableField, target);
                }
            }
        
            var buttonSize = Mathf.Min(position.height, K_MaxSize);
            var leftRect = new Rect(position.x, position.y, position.width - buttonSize, position.height);
            var rightRect = new Rect(position.x + position.width - buttonSize, position.y + position.height - buttonSize, buttonSize, buttonSize);
            EditorGUI.BeginChangeCheck();

            if(property.isArray)
                EditorGUI.PropertyField(leftRect, property, label, true);
            else
                property.objectReferenceValue = SirenixEditorFields.UnityObjectField(leftRect, label, property.objectReferenceValue, type, true);
            
            var clickedButton = SirenixEditorGUI.SDFIconButton(rightRect, GUIContent.none, SdfIconType.Search);

            if (EditorGUI.EndChangeCheck() && clickedButton)
            {
                Fetch(property, fetchableField, target);
            }

            property.serializedObject.UpdateIfRequiredOrScript();
        }

        private void Fetch(SerializedProperty property, FetchableFieldAttribute fetchableField, Component target)
        {
            var type = fieldInfo.FieldType;

            if (!property.isArray)
            {
                var foundTarget = fetchableField.FetchStrategy switch
                {
                    FetchStrategy.GetComponent => target.GetComponent(type),
                    FetchStrategy.GetComponentInParent => target.GetComponentInParent(type),
                    FetchStrategy.GetComponentInChildren => target.GetComponentInChildren(type),
                    FetchStrategy.FindFirstInScene => target.gameObject.scene.GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren(type)).FirstOrDefault(),
                    _ => throw new ArgumentOutOfRangeException()
                };
                property.objectReferenceValue = foundTarget;
            }
            else
            {
                var foundTarget = fetchableField.FetchStrategy switch
                {
                    FetchStrategy.GetComponents => target.GetComponents(type),
                    FetchStrategy.GetComponentsInChildren => target.GetComponentsInChildren(type),
                    FetchStrategy.GetComponentsInParent => target.GetComponentsInParent(type),
                    FetchStrategy.FindAllInScene =>  target.gameObject.scene.GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren(type)).ToArray(),
                    _ => throw new ArgumentOutOfRangeException()
                };
                property.ClearArray();
                property.arraySize = foundTarget.Length;
                for (int i = 0; i < foundTarget.Length; i++)
                {
                    property.GetArrayElementAtIndex(i).objectReferenceValue = foundTarget[i];
                }
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }

}