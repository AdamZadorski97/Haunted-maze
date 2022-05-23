using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(menuName = "Camera Proporties")]
public class WallData : ScriptableObject
{
    [VerticalGroup("row1")]
    public string objectName;

    [VerticalGroup("row1")]
    [PreviewField(100)]
    public Mesh mesh;
   
    [PreviewField(100)]
    public List<Material> materials = new List<Material>();
    [VerticalGroup("row2")]
    public List<Vector3> boxCollidersCenter = new List<Vector3>();
    [VerticalGroup("row2")]
    public List<Vector3> boxCollidersSize = new List<Vector3>();
    

}
