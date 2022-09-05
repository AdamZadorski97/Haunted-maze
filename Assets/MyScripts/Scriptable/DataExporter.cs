using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataExporter : MonoBehaviour
{
    public WeaponsData weaponsData;
    public PlayerData playerData;

    [Button]
    public void ExportData()
    {
        string subDir = Path.Combine(Application.persistentDataPath, "Export", "Data");
        Directory.CreateDirectory(subDir);
        string messagepath = Path.Combine(subDir, "SaveData" + ".json");
        string jsonSaveString = JsonUtility.ToJson(weaponsData);
        File.WriteAllText(messagepath, jsonSaveString);
    }
    [Button]
    public void ShowExplorer()
    {
        string path = Application.persistentDataPath;
        path = path.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
    }
}
