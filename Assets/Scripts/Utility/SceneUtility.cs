using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Utility
{
    public static class SceneUtility
    {
        public static T FindInScene<T>(this Scene scene, bool includeInactive = true, Func<T, bool> predicate = null) where T : UnityEngine.Object
        {
            foreach (var root in scene.GetRootGameObjects())
            {
                var found = root.GetComponentInChildren<T>(includeInactive);
                if(found && (predicate?.Invoke(found) ?? true))
                    return found;
            }

            return null;
        }
        
        public static IEnumerable<T> FindAllInScene<T>(this Scene scene, bool includeInactive = true, Func<T, bool> predicate = null) where T : UnityEngine.Object
        {
            foreach (var root in scene.GetRootGameObjects())
            {
                var foundComponentsInChildren = root.GetComponentsInChildren<T>(includeInactive);

                foreach (var target in foundComponentsInChildren)
                {
                    if(target && (predicate?.Invoke(target) ?? true))
                        yield return target;
                }
            }
        }
    }
}