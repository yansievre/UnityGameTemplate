using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utility.Editor
{
    public static class PrefabHelper
    {
        private const string K_NewPath = "Assets/Prefabs/Env";
        private const string K_BasePath = "Assets/Prefabs/Env/prefab_env_base.prefab";
        [MenuItem("Assets/Create Environment Prefab")]
        public static void CreateEnvironmentPrefab()
        {
            var gameObject = Selection.activeGameObject;
            var meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

            if (meshFilters.Length == 0)
                return;

            if (meshFilters.Length > 1)
            {
                MeshPickWindow.OpenWindow(meshFilters.ToList(), ContinueWithMeshFilter);

                return;
            }

            ContinueWithMeshFilter(meshFilters[0]);
        }

        private static void ContinueWithMeshFilter(MeshFilter filter)
        {
            var name = filter.sharedMesh.name.ToLower();
            var newPath = $"{K_NewPath}/prefab_{name}.prefab";
    
            GameObject prefabRef = (GameObject)AssetDatabase.LoadMainAssetAtPath(K_BasePath);
            GameObject instanceRoot = (GameObject)PrefabUtility.InstantiatePrefab(prefabRef);
            var modelParent = instanceRoot.transform.Find("model_parent");
            GameObject target = new GameObject(name);
            target.transform.parent = modelParent;
            target.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            target.AddComponent<MeshFilter>().sharedMesh = filter.sharedMesh;
            target.AddComponent<MeshRenderer>().sharedMaterial = filter.GetComponent<MeshRenderer>().sharedMaterial;
            GameObject pVariant = PrefabUtility.SaveAsPrefabAsset(instanceRoot, newPath);
            Selection.SetActiveObjectWithContext(pVariant, null);
            Object.DestroyImmediate(instanceRoot);
        }
        
        // Note that we pass the same path, and also pass "true" to the second argument.
        [MenuItem("Assets/Create Environment Prefab", true)]
        private static bool CreateEnvironmentPrefabValidation()
        {
            var gameObject = Selection.activeGameObject;
            if(gameObject == null)
                return false;
            if ((gameObject.scene.name != null && gameObject.scene.name != gameObject.name) ||
                gameObject.transform.parent != null) 
                return false;

            return true;

            // This returns true when the selected object is a Variable (the menu item will be disabled otherwise).
            //return Selection.activeObject is Variable;
        }
    }
}