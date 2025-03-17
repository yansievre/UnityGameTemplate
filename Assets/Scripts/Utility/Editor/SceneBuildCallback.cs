using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Utility.Editor
{
    internal class SceneBuildProcessor : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(UnityEngine.SceneManagement.Scene scene, BuildReport report)
        {
            Debug.Log("SceneBuildProcessor.OnProcessScene " + scene.name);
            var roots = scene.GetRootGameObjects();

            foreach (var root in roots)
            {
                foreach (var callbackRequester in root.GetComponentsInChildren<IOnSceneBuild>())
                {
                    callbackRequester.OnSceneBuild();
                }
            }
        }
    }
}