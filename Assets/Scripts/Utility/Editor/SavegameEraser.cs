#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utility.Editor
{
    public class SavegameEraser : EditorWindow
    {
        [MenuItem("Tools/Erase Savegame %#e")] // Shortcut: Ctrl+Shift+E on Windows, Cmd+Shift+E on macOS
        public static void EraseSavegame()
        {
            string saveDataPath = Application.persistentDataPath + "/WorldState.json";
            if (File.Exists(saveDataPath))
            {
                File.Delete(saveDataPath);
                Debug.Log("Savegame erased.");
            }
            else
            {
                Debug.Log("No savegame to erase.");
            }
        }
    }
}
#endif