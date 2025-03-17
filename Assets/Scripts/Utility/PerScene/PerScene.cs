using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility.PerScene
{
    [Serializable]
    public class PerScene<T> where T : Component
    {
        private static Dictionary<Scene, T> S_Instances = new Dictionary<Scene, T>();

        internal static T GetFromScene(Scene? scene)
        {
            if(scene == null)
                throw new ArgumentNullException(nameof(scene), "You cannot request a PerScene instance without providing a scene or game object.");
            if(S_Instances.TryGetValue(scene.Value, out var instance))
                return instance;
            instance = scene?.GetRootGameObjects().Select(x => x.GetComponentInChildren<T>()).FirstOrValue(null);

            if (instance == null)
            {
                var go = new GameObject("PerScene " + typeof(T).GetNiceName())
                {
                    hideFlags = HideFlags.HideInHierarchy,
                };
                SceneManager.MoveGameObjectToScene(go, scene.Value);
                instance = go.AddComponent<T>();
            }
            S_Instances[scene.Value] = instance;
            return instance;
        }
        
        [SerializeField]
        private T _instance;

        public PerScene(Scene scene)
        {
            _instance = GetFromScene(scene);
        }
        
        public PerScene(GameObject gameObject)
        {
            _instance = GetFromScene(gameObject.scene);
        }

        private PerScene()
        {
        }

        /// <summary>
        /// Requests the instance of the component from the scene. If the instance does not exist, it will create one.
        /// </summary>
        public T GetInstance(GameObject gameObject)
        {
            return GetInstance(gameObject.scene);
        }
        
        /// <summary>
        /// Requests the instance of the component from the scene. If the instance does not exist, it will create one.
        /// </summary>
        public T GetInstance(Scene scene)
        {
            return _instance ??= GetFromScene(scene);
        }
        
        /// <summary>
        /// Requests the instance of the component from the scene. 
        /// Scene or GameObject must have been provided beforehand.
        /// </summary>
        public T GetInstance()
        {
            return _instance;
        }

        public T Instance => _instance;
    }
}