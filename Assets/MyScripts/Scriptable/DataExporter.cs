using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataExporter : MonoBehaviour
{

    
    [Button]
    public void ShowExplorer()
    {
        string path = Application.persistentDataPath;
        path = path.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
    }
}
