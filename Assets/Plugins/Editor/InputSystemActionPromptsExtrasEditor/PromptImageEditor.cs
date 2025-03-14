// using Plugins.InputSystemActionPromptsExtras;
// using Sirenix.OdinInspector.Editor;
// using Sirenix.Utilities.Editor;
// using System;
// using UnityEditor;
// using UnityEditor.AnimatedValues;
// using UnityEditor.UI;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Plugins.Editor.InputSystemActionPromptsExtrasEditor {
//
//     [CustomEditor(typeof(PromptImage), true)]
//     [CanEditMultipleObjects]
//     public class PromptImageEditor : OdinEditor
//     {
// 		    SerializedProperty m_Script;
//         InspectorProperty m_Action;
//         ImageEditor m_ImageEditor;
//         InspectorProperty m_SetNativeSize;
//         PropertyTree m_PropertyTree;
//         private Action _disableNativeSize;
//
//         private class Styles
//         {
//             public static GUIContent text = EditorGUIUtility.TrTextContent("Fill Origin");
//             public static GUIContent[] OriginHorizontalStyle =
//             {
//                 EditorGUIUtility.TrTextContent("Left"),
//                 EditorGUIUtility.TrTextContent("Right")
//             };
//
//             public static GUIContent[] OriginVerticalStyle =
//             {
//                 EditorGUIUtility.TrTextContent("Bottom"),
//                 EditorGUIUtility.TrTextContent("Top")
//             };
//
//             public static GUIContent[] Origin90Style =
//             {
//                 EditorGUIUtility.TrTextContent("BottomLeft"),
//                 EditorGUIUtility.TrTextContent("TopLeft"),
//                 EditorGUIUtility.TrTextContent("TopRight"),
//                 EditorGUIUtility.TrTextContent("BottomRight")
//             };
//
//             public static GUIContent[] Origin180Style =
//             {
//                 EditorGUIUtility.TrTextContent("Bottom"),
//                 EditorGUIUtility.TrTextContent("Left"),
//                 EditorGUIUtility.TrTextContent("Top"),
//                 EditorGUIUtility.TrTextContent("Right")
//             };
//
//             public static GUIContent[] Origin360Style =
//             {
//                 EditorGUIUtility.TrTextContent("Bottom"),
//                 EditorGUIUtility.TrTextContent("Right"),
//                 EditorGUIUtility.TrTextContent("Top"),
//                 EditorGUIUtility.TrTextContent("Left")
//             };
//         }
//
//         protected override void OnEnable()
//         {
//             base.OnEnable();
//
//             m_Script = serializedObject.FindProperty("m_Script");
//             m_ImageEditor = (ImageEditor) CreateEditor(target, typeof(ImageEditor));
//             m_PropertyTree = PropertyTree.Create(target);
//             m_Action               = m_PropertyTree.GetPropertyAtUnityPath("m_Action");
//             m_SetNativeSize                  = m_PropertyTree.GetPropertyAtUnityPath("_setNativeSize");
//             var methodInfo = typeof(GraphicEditor).GetMethod("SetShowNativeSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//
//             _disableNativeSize = () => methodInfo?.Invoke(m_ImageEditor, new object[]{false, true});
//             _disableNativeSize();
//             m_Action.ValueEntry.OnValueChanged += Refresh;
//         }
//
//         private void Refresh(int index = 0) {
// 	        ((PromptImage)target).RefreshIcon();
//         }
//
//         protected override void OnDisable()
//         {
// 	        m_Action.ValueEntry.OnValueChanged -= Refresh;
// 	        m_PropertyTree.Dispose();
//             base.OnDisable();
//         }
//
//         public override void OnInspectorGUI()
//         {
// 	        EditorGUI.BeginChangeCheck();
// 	        using (new EditorGUI.DisabledScope(true))
// 		        EditorGUILayout.PropertyField(m_Script, true);
// 	        serializedObject.Update();
// 	        m_ImageEditor.OnInspectorGUI();
// 	        EditorGUILayout.Separator();
// 	        m_Action.Draw();
// 	        m_SetNativeSize.Draw();
//
// 	        if (EditorGUI.EndChangeCheck()) {
// 		        m_PropertyTree.ApplyChanges();
// 		        serializedObject.ApplyModifiedProperties();
// 		        _disableNativeSize();
// 	        }
//         }
//
//         /// <summary>
//         /// All graphics have a preview.
//         /// </summary>
//
//         public override bool HasPreviewGUI() { return true; }
//     }
// }