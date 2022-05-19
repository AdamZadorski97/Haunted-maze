using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Camera Proporties")]
public class WallData : ScriptableObject
{
    public string objectName;
    public Mesh mesh;
    public List<Material> materials = new List<Material>();

    public List<Vector3> boxCollidersSizes = new List<Vector3>();
    public List<Vector3> boxCollidersOffset = new List<Vector3>();
    

}
