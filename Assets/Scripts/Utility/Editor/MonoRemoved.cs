using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utility.Editor
{
    public static class MonoRemoved
    {
        [MenuItem("CONTEXT/MonoBehaviour/Remove Component", priority = int.MinValue)]
        private static void ComponentContextMenuItem(MenuCommand menuCommand)
        {
            if (menuCommand.context == null)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                }
                return;
            }
            try
            {
                var mb = (MonoBehaviour) menuCommand.context;
                var go = mb.gameObject;
                mb.CallMethodOnTarget("OnDestroyEditor");
                go.CallMethodOnGameObject("OnMonoBehaviourRemoved");
            }catch(Exception e){}
            
            RemoveComponent(menuCommand.context as Component);
        }
        public static void RemoveComponent([DisallowNull] Component comp)
        {
            var dependencies = ComponentDependencies(comp);
            if (!RemoveComponent(comp, dependencies))
            {
                //preserve built-in behavior
                if (CanRemoveComponent(comp, dependencies))
                    Undo.DestroyObjectImmediate(comp);
            }
        }
         public static IEnumerable<Component> ComponentDependencies([DisallowNull] Component component)
        {
            if (component == null)
                yield break;

            var componentType = component.GetType();
            foreach (var c in component.gameObject.GetComponents<Component>())
            {
                foreach (var rc in c.GetType().GetCustomAttributes(typeof(RequireComponent), true).Cast<RequireComponent>())
                {
                    if (rc.m_Type0 == componentType || rc.m_Type1 == componentType || rc.m_Type2 == componentType)
                    {
                        yield return c;
                        break;
                    }
                }
            }
        }

        public static bool CanRemoveComponent([DisallowNull] Component component, IEnumerable<Component> dependencies)
        {
            if (dependencies.Count() == 0)
                return true;

            Component firstDependency = dependencies.First();
            string error = $"Can't remove {component.GetType().Name} because {firstDependency.GetType().Name} depends on it.";
            EditorUtility.DisplayDialog("Can't remove component", error, "OK");
            return false;
        }

        public static bool RemoveComponent([DisallowNull] Component component, IEnumerable<Component> dependencies)
        {

            if (!CanRemoveComponent(component, dependencies))
                return false;

            bool removed = true;
            var isAssetEditing = EditorUtility.IsPersistent(component);
            try
            {
                if (isAssetEditing)
                {
                    AssetDatabase.StartAssetEditing();
                }
                Undo.SetCurrentGroupName($"Remove {component.GetType()} and additional data components");
                Undo.DestroyObjectImmediate(component);
            }
            catch
            {
                removed = false;
            }
            finally
            {
                if (isAssetEditing)
                {
                    AssetDatabase.StopAssetEditing();
                }
            }

            return removed;
        }
    }
}